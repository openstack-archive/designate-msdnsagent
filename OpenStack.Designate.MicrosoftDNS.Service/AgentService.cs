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
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenStack.Designate.MicrosoftDNS;

namespace OpenStack.Designate.MicrosoftDNS.Service
{
    public partial class AgentService : ServiceBase
    {
        private AgentConsumer _AgentConsumer = null;
        private ServiceStatus _ServiceStatus = new ServiceStatus() { dwWaitHint = 100000 };

        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public long dwServiceType;
            public ServiceState dwCurrentState;
            public long dwControlsAccepted;
            public long dwWin32ExitCode;
            public long dwServiceSpecificExitCode;
            public long dwCheckPoint;
            public long dwWaitHint;
        };

        private ConfigManager _Config;
        private Logging LOG;
        private Thread _ServiceThread;

        public AgentService()
        {
            this._Config = new ConfigManager();
            this.LOG = Logging.GetLogger(LoggingContext.EventLog, _Config);
            InitializeComponent();

            this._AgentConsumer = new AgentConsumer(this._Config);
        }

        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.
            this._ServiceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            SetServiceStatus(this.ServiceHandle, ref this._ServiceStatus);

            // Start the Service
            this._AgentConsumer.Start();

            // Update the service state to Running.
            this._ServiceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref this._ServiceStatus);

            // Begin Consuming Messgaes
            _ServiceThread = new Thread(this._AgentConsumer.Consume);
            _ServiceThread.Start();
        }

        protected override void OnStop()
        {
            // Update the service state to Stop Pending.
            this._ServiceStatus.dwCurrentState = ServiceState.SERVICE_STOP_PENDING;
            SetServiceStatus(this.ServiceHandle, ref this._ServiceStatus);

            // Stop the Service
            this._AgentConsumer.Stop();
            this._ServiceThread.Join();

            // Update the service state to Stopped.
            this._ServiceStatus.dwCurrentState = ServiceState.SERVICE_STOPPED;
            SetServiceStatus(this.ServiceHandle, ref this._ServiceStatus);
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);
    }
}
