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
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace OpenStack.Designate.MicrosoftDNS
{
    // Agent Consumer
    public class AgentConsumer
    {

        Logging LOG = Logging.GetLogger();

        private ConnectionFactory _ConnectionFactory = null;
        private IConnection _Connection = null;
        private IModel _Channel = null;
        private QueueingBasicConsumer _Consumer = null;

        private AgentConsumerHandler _Handler;

        private ConfigManager _Config;

        private bool _Stopping = false;

        public AgentConsumer(ConfigManager config)
        {
            this._Config = config;
            
            this._Handler = new AgentConsumerHandler(this);

            this._ConnectionFactory = new ConnectionFactory();
            this._ConnectionFactory.UserName = this._Config.RMQUser;
            this._ConnectionFactory.Password = this._Config.RMQPassword;
            this._ConnectionFactory.Port = this._Config.RMQPort;
        }

        public void Start()
        {
            this._Connect();
        }

        private void _Connect()
        {
            LOG.Log(LogLevel.Information, "AgentConsumer Connecting");

            this._CreateConnection();
            this._Channel = this._Connection.CreateModel();
            this._Consumer = new QueueingBasicConsumer(this._Channel);
        }

        private void _CreateConnection()
        {

            Exception LastError = null;

            foreach (string host in this._Config.RMQHosts)
            {
                try
                {
                    this._ConnectionFactory.HostName = host;
                    this._Connection = this._ConnectionFactory.CreateConnection();
                    if (this._Connection != null && this._Connection.IsOpen)
                    {
                        LOG.Log(LogLevel.Information, "Connected to Rabbit MQ Server: " + host);
                        break;
                    }
                        
                }
                catch (Exception e)
                {
                    if (e.InnerException != null && e.InnerException is RabbitMQ.Client.Exceptions.AuthenticationFailureException)
                    {
                        throw e;
                    }
                    else
                    {
                        LastError = e;
                    }
                }
            }
            if (LastError != null)
            {
                throw LastError;
            }    
        }

        private void _Initialize()
        {
            LOG.Log(LogLevel.Information, "AgentConsumer Initializing");

            string agent_queue = String.Format(@"agent.{0}", this._Config.AgentName);
            LOG.Log(LogLevel.Verbose, "agent_queue: " + agent_queue);
            string agent_fanout_queue = String.Format(@"agent_fanout_{0}", this._Config.AgentName);
            LOG.Log(LogLevel.Verbose, "agent_fanout_queue: " + agent_fanout_queue);
            this._Channel.ExchangeDeclare("designate", "topic");
            LOG.Log(LogLevel.Verbose, "Declared Topic Exchange");
            this._Channel.ExchangeDeclare("agent_fanout", "fanout");
            LOG.Log(LogLevel.Verbose, "Declared Fanout Exchange");

            this._Channel.QueueDeclare("agent", false, false, false, null);
            LOG.Log(LogLevel.Verbose, "Declared agent queue");
            this._Channel.QueueDeclare(agent_queue, false, false, false, null);
            LOG.Log(LogLevel.Verbose, "Declared " + agent_queue + " queue");
            this._Channel.QueueDeclare(agent_fanout_queue, false, false, true, null);
            LOG.Log(LogLevel.Verbose, "Declared " + agent_fanout_queue + " queue");

            this._Channel.QueueBind("agent", "designate", "agent");
            LOG.Log(LogLevel.Verbose, "Bound to agent queue");
            this._Channel.QueueBind(agent_queue, "designate", agent_queue);
            LOG.Log(LogLevel.Verbose, "Bound to " + agent_queue + " queue");
            this._Channel.QueueBind(agent_fanout_queue, "agent_fanout", "agent");
            LOG.Log(LogLevel.Verbose, "Bound to " + agent_fanout_queue + " queue");

            this._Channel.BasicConsume("agent", true, this._Consumer);
            LOG.Log(LogLevel.Verbose, "Consuming from agent queue");
            this._Channel.BasicConsume(agent_queue, true, this._Consumer);
            LOG.Log(LogLevel.Verbose, "Consuming from " + agent_queue + " queue");
            this._Channel.BasicConsume(agent_fanout_queue, true, this._Consumer);
            LOG.Log(LogLevel.Verbose, "Consuming from " + agent_fanout_queue + " queue");
        }

        public void Consume()
        {
            this._Initialize();
            LOG.Log(LogLevel.Information, "AgentConsumer Processing Messages");

            while (this._Stopping != true)
            {
                try
                {
                    BasicDeliverEventArgs e = (BasicDeliverEventArgs) this._Consumer.Queue.Dequeue();
                    this._Handler.Dispatch(Encoding.UTF8.GetString(e.Body));

                    //channel.BasicAck(e.DeliveryTag, false);
                }
                catch (EndOfStreamException)
                {
                    // The consumer was cancelled, the model closed, or the
                    // connection went away.
                    if (!this._Stopping)
                    {
                        LOG.Log(LogLevel.Error, "AgentConsumer EndOfStreamException");
                        this._Connect();
                        this._Initialize();
                    }
                }
            }
        }

        public void Stop()
        {
            LOG.Log(LogLevel.Information, "Stopping AgentConsumer");

            this._Stopping = true;
            if (this._Connection != null)
            {
                LOG.Log(LogLevel.Information, "Closing Connection");
                this._Connection.Close();
                LOG.Log(LogLevel.Information, "Connection Closed");
            }
        }

        public void Reply(string queue, string message)
        {
            using (var channel = this._Connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(message);
                IBasicProperties properties = channel.CreateBasicProperties();
                properties.ContentType = "application/json";
                channel.BasicPublish(queue, queue, properties, body);
                LOG.Log(LogLevel.Verbose, "Replied with message: "+ message + " to Queue: " + queue);
            }
        }
    }
}
