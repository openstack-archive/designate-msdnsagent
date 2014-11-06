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
using System.Security;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace OpenStack.Designate.MicrosoftDNS
{
     public class ConfigManager
    {
        //Registry Base Key
        private RegistryKey _LocalMachine = Registry.LocalMachine;
        private RegistryKey _Software;
        private RegistryKey _DesignateMSDNS;

        //Config Values
        private string[] _RMQHosts;
        private string[] _DNSMasters;
        private int _RMQPort;
        private string _RMQUser;
        private string _RMQPassword;
        private string _AgentName;
        private bool _WriteToRegistry = false;
        private string _WMIHost;
        private string _WMIUser;
        private string _WMIPassword;
        private string _WMIAuthority;
        private LogLevel _LogLevel;
                
        public ConfigManager(bool WriteToRegistry = true, bool ReadFromRegistry = true)
        {
            this._WriteToRegistry = WriteToRegistry;
            if (WriteToRegistry)
            {
                this._Setup();
                if (ReadFromRegistry)
                {
                    this._InitValues();
                }
            }
        }

        public ConfigManager(bool WriteToRegistry, bool ReadFromRegistry, string[] RMQHosts = null, string[] DNSMasters = null, string RMQUser = null, string RMQPassword = null, int RMQPort = 0, string AgentName = null)
            : this(WriteToRegistry, ReadFromRegistry) 
        {
            this._RMQHosts = RMQHosts;
            this._DNSMasters = DNSMasters;
            this._RMQUser = RMQUser;
            this._RMQPassword = RMQPassword;
            this._RMQPort = RMQPort;
            this._AgentName = AgentName;
        }

        private void _Setup()
        {
            //Init the registry connection
            this._Software = this._LocalMachine.OpenSubKey("SOFTWARE", RegistryKeyPermissionCheck.ReadWriteSubTree, System.Security.AccessControl.RegistryRights.FullControl);
            this._DesignateMSDNS = this._Software.CreateSubKey("DesignateMSDNS", RegistryKeyPermissionCheck.ReadWriteSubTree);
            this._Software.Flush();
        }

        private void _InitValues(){
            //Get Current Values
            this._RMQHosts = (string[])this._DesignateMSDNS.GetValue("RMQHosts", new string[0] { });
            this._DNSMasters = (string[])this._DesignateMSDNS.GetValue("DNSMasters", new string[0] { });
            this._RMQPort = (int)this._DesignateMSDNS.GetValue("RMQPort", 5672);
            this._RMQUser = (string)this._DesignateMSDNS.GetValue("RMQUser", "guest");
            this._RMQPassword = (string)this._DesignateMSDNS.GetValue("RMQPassword", "guest");
            this._AgentName = (string)this._DesignateMSDNS.GetValue("AgentName", System.Environment.MachineName);
            this._WMIHost = (string)this._DesignateMSDNS.GetValue("WMIHost", "127.0.0.1");
            this._WMIUser = (string)this._DesignateMSDNS.GetValue("WMIUser", null);
            this._WMIPassword = (string)this._DesignateMSDNS.GetValue("WMIPassword", null);
            this._WMIAuthority = (string)this._DesignateMSDNS.GetValue("WMIAuthority", null);
            this._LogLevel = (LogLevel)this._DesignateMSDNS.GetValue("LogLevel", LogLevel.Information);
        }

        private void _SaveValue(string key, string value)
        {
            if (this._WriteToRegistry)
            {
                this._DesignateMSDNS.SetValue(key, value, RegistryValueKind.String);
                this._DesignateMSDNS.Flush();
            }
        }

        private void _SaveValue(string key, string[] value)
        {
            if (this._WriteToRegistry)
            {
                this._DesignateMSDNS.SetValue(key, value, RegistryValueKind.MultiString);
                this._DesignateMSDNS.Flush();
            }
        }
        private void _SaveValue(string key, int value)

        {
            if (this._WriteToRegistry)
            {
                this._DesignateMSDNS.SetValue(key, value, RegistryValueKind.DWord);
                this._DesignateMSDNS.Flush();
            }
        }        
    
        //Getters and Setters
        public string[] RMQHosts
        {
            get { return _RMQHosts; }
            set { _RMQHosts = value; this._SaveValue("RMQHosts", value); }
        }

        public string[] DNSMasters
        {
            get { return _DNSMasters; }
            set { _DNSMasters = value; this._SaveValue("DNSMasters", value); }
        }

        public string RMQUser
        {
            get { return _RMQUser; }
            set { _RMQUser = value; this._SaveValue("RMQUser", value); }
        }

        public string RMQPassword
        {
            get { return _RMQPassword; }
            set { _RMQPassword = value; this._SaveValue("RMQPassword", value); }
        }

        public int RMQPort
        {
            get { return _RMQPort; }
            set { _RMQPort = value; this._SaveValue("RMQPort", value); }
        }
        public string AgentName
        {
            get { return _AgentName; }
            set { _AgentName = value; this._SaveValue("AgentName", value); }
        }

        public string WMIHost
        {
            get { return _WMIHost; }
            set { _WMIHost = value; this._SaveValue("WMIHost", value); }
        }

        public string WMIUser
        {
            get { return _WMIUser; }
            set { _WMIUser = value; this._SaveValue("WMIUser", value); }
        }

        public string WMIPassword
        {
            get { return _WMIPassword; }
            set { _WMIPassword = value; this._SaveValue("WMIPassword", value); }
        }

        public string WMIAuthority
        {
            get { return _WMIAuthority; }
            set { _WMIAuthority = value; this._SaveValue("WMIAuthority", value); }
        }

        public LogLevel LogLevel
        {
            get { return _LogLevel; }
            set { _LogLevel = value; this._SaveValue("LogLevel", (int) value); }
        }
    }
}
