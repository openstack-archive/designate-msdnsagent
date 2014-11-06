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
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenStack.Designate.MicrosoftDNS
{
    [DataContract]
    public class OsloMessagingEnvelope
    {
        [DataMember(Name = "oslo.version", IsRequired = true)]
        public string OsloVersion;

        [DataMember(Name = "oslo.message", IsRequired = true)]
        public string OsloMessage;
    }

    [DataContract]
    public class OsloMessagingReplyMessage
    {
        [DataMember(Name = "_msg_id", IsRequired = true)]
        public string MessageId;
        [DataMember(Name = "_unique_id", IsRequired = true)]
        public string UniqueID;
        [DataMember(Name = "failure", IsRequired = false)]
        public string failure;
        [DataMember(Name = "result", IsRequired = false)]
        public string result;
        [DataMember(Name = "ending", IsRequired = true)]
        public bool ending;

    }

    [DataContract]
    public class OsloMessagingMessage
    {
        [DataMember(Name = "method", IsRequired = true)]
        public string Method;

        [DataMember(Name = "_reply_q", IsRequired = true)]
        public string ReplyQueue;

        [DataMember(Name = "_msg_id", IsRequired = true)]
        public string MessageId;

        [DataMember(Name = "_unique_id", IsRequired = true)]
        public string UniqueId;

        [DataMember(Name = "args", IsRequired = true)]
        public BackendCreateDomainArguments Args;
    }

    [DataContract]
    public class BackendCreateDomainArguments
    {
        [DataMember(Name = "domain", IsRequired = false)]
        public DesignateDomainObject Domain;
    }

    [DataContract]
    public abstract class DesignateObject
    {
        [DataMember(Name = "designate_object.changes", IsRequired = true)]
        public string[] Changes;

        [DataMember(Name = "designate_object.name", IsRequired = true)]
        public string Name;

        [DataMember(Name = "designate_object.original_values", IsRequired = true)]
        public Dictionary<string, object> OriginalValues;
    }

    [DataContract]
    public class DesignateDomainObject : DesignateObject
    {
        [DataMember(Name = "designate_object.data", IsRequired = true)]
        public DesignateDomainObjectData Data;
    }

    [DataContract]
    public class DesignateDomainObjectData
    {
        [DataMember(Name = "id", IsRequired = true)]
        public string ID;

        [DataMember(Name = "name", IsRequired = true)]
        public string Name;
    }
}
