// Copyright 2014 Hewlett-Packard Development Company, L.P.

// Licensed under the Apache License, Version 2.0 (the "License"); you may
// not use this file except in compliance with the License. You may obtain
// a copy of the License at

//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
// WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
// License for the specific language governing permissions and limitations
// under the License.

using System;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;


namespace OpenStack.Designate.MicrosoftDNS
{
    //Exceptions

    public class MethodNotImplementedException : NotImplementedException
    {
        public MethodNotImplementedException(string method)
            : base()
        {
            Logging LOG = Logging.GetLogger();
            LOG.Log(LogLevel.Verbose, method + " Method Not Implemented");
        }
    }

    class AgentConsumerHandler
    {
        private DataContractJsonSerializer _OsloMessagingEnvelopeSerializer = new DataContractJsonSerializer(typeof(OsloMessagingEnvelope));
        private DataContractJsonSerializer _OsloMessagingMessageSerializer = new DataContractJsonSerializer(typeof(OsloMessagingMessage));
        private DataContractJsonSerializer _OsloMessagingReplyMessageSerializer = new DataContractJsonSerializer(typeof(OsloMessagingReplyMessage));

        private Logging LOG;
        private DNSServer _DNS;
        private ConfigManager _Config;
        private AgentConsumer _Consumer;

        public AgentConsumerHandler(AgentConsumer consumer)
        {
            _Consumer = consumer;
            LOG = Logging.GetLogger();
            _Config = new ConfigManager();
            _DNS = new DNSServer(_Config);
            _DNS.Connect();
            
        }

        public OsloMessagingMessage Deserialize(string message)
        {
            OsloMessagingEnvelope ome = this._DeserializeOsloMessagingEnvelope(message);
            OsloMessagingMessage omm = this._DeserializeOsloMessagingMessage(ome.OsloMessage);

            return omm;
        }

        private string _SerialiseReplyMessage(string msg_id){

            OsloMessagingReplyMessage replyMessageDataContract = new OsloMessagingReplyMessage();
            OsloMessagingEnvelope replyEnvelopeDataContract = new OsloMessagingEnvelope();
            
            MemoryStream replyMessageStream = new MemoryStream();
            MemoryStream replyEnvelopeStream = new MemoryStream();

            replyMessageDataContract.UniqueID = Guid.NewGuid().ToString("N");
            replyMessageDataContract.ending = true;
            replyMessageDataContract.result = null;
            replyMessageDataContract.failure = null;
            replyMessageDataContract.MessageId = msg_id;
            this._OsloMessagingReplyMessageSerializer.WriteObject(replyMessageStream, replyMessageDataContract);
            string replyMessage = ASCIIEncoding.ASCII.GetString(replyMessageStream.ToArray());
            LOG.Log(LogLevel.Verbose, "Reply Message: " + replyMessage);

            replyEnvelopeDataContract.OsloMessage = replyMessage;
            replyEnvelopeDataContract.OsloVersion = "2.0";

            this._OsloMessagingEnvelopeSerializer.WriteObject(replyEnvelopeStream, replyEnvelopeDataContract);
            string replyEnvelope = ASCIIEncoding.ASCII.GetString(replyEnvelopeStream.ToArray());
            LOG.Log(LogLevel.Verbose, "Reply: " + replyEnvelope);
                        
            return replyEnvelope;
        }

        public void Reply(string msg_id, string reply_queue)
        {
            string message = this._SerialiseReplyMessage(msg_id);
            this._Consumer.Reply(reply_queue, message);
        }


        private OsloMessagingEnvelope _DeserializeOsloMessagingEnvelope(string message)
        {
            
            MemoryStream ms = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(message));
            OsloMessagingEnvelope om = null;

            try
            {
                om = (OsloMessagingEnvelope)this._OsloMessagingEnvelopeSerializer.ReadObject(ms);
            }
            catch (SerializationException ex)
            {
                LOG.Log(LogLevel.Error, "Failed to deserialize OsloMessagingEnvelope: " + ex.Message);
                throw ex;
            }

            return om;
        }

        private OsloMessagingMessage _DeserializeOsloMessagingMessage(string message)
        {
            MemoryStream ms = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(message));
            OsloMessagingMessage om = null;

            try
            {
                om = (OsloMessagingMessage)this._OsloMessagingMessageSerializer.ReadObject(ms);
            }
            catch (SerializationException ex)
            {
                LOG.Log(LogLevel.Error, "Failed to deserialize OsloMessagingMessage: " + ex.Message);
                throw ex;
            }

            return om;
        }

        public void Dispatch(string message)
        {
            LOG.Log(LogLevel.Verbose, "Received message");
            
            try
            {
                OsloMessagingMessage om = this.Deserialize(message);

                switch (om.Method)
                {
                    case "create_domain":
                        _DNS.CreateNewZone(om.Args.Domain.Data.Name, this._Config.DNSMasters);
                        LOG.Log(LogLevel.Information, "Created Domain: " + om.Args.Domain.Data.Name);
                        this.Reply(om.MessageId, om.ReplyQueue);
                        break;
                    case "delete_domain":
                        DNSServer.DNSZone zone = _DNS.FetchZone(om.Args.Domain.Data.Name);
                        zone.DeleteZone();
                        LOG.Log(LogLevel.Information, "Deleted Domain: " + om.Args.Domain.Data.Name);
                        this.Reply(om.MessageId, om.ReplyQueue);
                        break;
                    default:
                        throw new MethodNotImplementedException(om.Method);
                }
            }
            catch (SerializationException ex)
            {
                LOG.Log(LogLevel.Error, "Failed to dispatch message: " + ex.Message);
            }
        }
    }
    

}
