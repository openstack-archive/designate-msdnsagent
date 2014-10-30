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
using System.Linq;
using System.Management;

namespace OpenStack.Designate.MicrosoftDNS
{
    // Exceptions
    public class DNSServerException : ApplicationException
    {
        public DNSServerException(string message) : base(message)
        {
            Logging LOG = Logging.GetLogger();
            LOG.Log(LogLevel.Critical, message);
        }
    }

    // A DNS Server class that handles all access to a particular MSDNS Server
    public class DNSServer
    {
        private ManagementScope _MgmtScope = null;
        private ConfigManager _Config;

        private Logging LOG;

        public DNSServer(ConfigManager config)
        {
            LOG = Logging.GetLogger();
            this._Config = config;

            ConnectionOptions co = new ConnectionOptions();
            co.Username = _Config.WMIUser;
            co.Password = _Config.WMIPassword;
            co.Authority = _Config.WMIAuthority;

            this._MgmtScope = new ManagementScope(String.Format(@"\\{0}\Root\MicrosoftDNS", _Config.WMIHost), co);
        }

        public void Connect()
        {
            try
            {
                LOG.Log(LogLevel.Verbose, "Connecting to WMI on: " + this._MgmtScope.Path);
                this._MgmtScope.Connect();
            }
            catch (ManagementException ex)
            {
                throw new DNSServerException("Failed to connect to WMI" + ex.Message);
            }
            finally
            {
                if (this._MgmtScope.IsConnected)
                {
                    LOG.Log(LogLevel.Information, "Connected to WMI sucessfully");
                }
                else
                {
                    LOG.Log(LogLevel.Error, "Failed to connect to WMI");
                }
            }
        }

        public class DNSZone
        {
            private ManagementObject _mgmtObject = null;

            public DNSZone(ManagementObject mgmtObject)
            {
                this._mgmtObject = mgmtObject;
            }

            public string Name
            {
                get { return (string)this._mgmtObject["Name"]; }
                set { this._mgmtObject["Name"] = value; }
            }

            public string[] MasterServers
            {
                get { return (string[])this._mgmtObject["MasterServers"]; }
                set { this._mgmtObject["MasterServers"] = value; }
            }

            public void UpdateZone()
            {
                this._mgmtObject.Put();
            }

            public void DeleteZone()
            {
                try
                {
                    this._mgmtObject.Delete();
                }
                catch (ManagementException)
                {
                    throw new DNSServerException("Something EXPLODED");
                }
            }
        }

        public void CreateNewZone(string zoneName, string[] masters)
        {
            try
            {
                ManagementObject mc = new ManagementClass(this._MgmtScope, new ManagementPath("MicrosoftDNS_Zone"), null);
                mc.Get();

                ManagementBaseObject parameters = mc.GetMethodParameters("CreateZone");

                parameters["ZoneName"] = zoneName;
                parameters["IpAddr"] = masters;
                parameters["ZoneType"] = 1; // 1 == Secondary
                parameters["DsIntegrated"] = false;

                mc.InvokeMethod("CreateZone", parameters, null);

            }
            catch (ManagementException ex)
            {
                throw new DNSServerException("Something EXPLODED: "+ ex.Message);
            }
        }

        public DNSZone FetchZone(string zoneName)
        {
            try
            {

                if (zoneName[zoneName.Length-1] == '.')
                    zoneName = zoneName.Substring(0, zoneName.Length - 1);

                string query = String.Format("SELECT * FROM MicrosoftDNS_Zone WHERE Name='{0}'", zoneName);
                ManagementObjectSearcher s = new ManagementObjectSearcher(this._MgmtScope, new ObjectQuery(query));

                DNSZone zone = null;

                ManagementObjectCollection c = s.Get();

                if (c.Count != 1)
                {
                    throw new DNSServerException("Done Borked");
                }
                else
                {
                    foreach (ManagementObject z in c)
                    {
                        zone = new DNSZone(z);
                    }
                }               

                return zone;
            }
            catch (ManagementException)
            {
                throw new DNSServerException("Something EXPLODED");
            }
        }


        public ManagementBaseObject ReturnValue { get; set; }
    }
}
