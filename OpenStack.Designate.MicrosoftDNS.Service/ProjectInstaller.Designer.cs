﻿// Copyright 2014 Hewlett-Packard Development Company, L.P.

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

namespace OpenStack.Designate.MicrosoftDNS.Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.agentServiceProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.agentServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // agentServiceProcessInstaller
            // 
            this.agentServiceProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.agentServiceProcessInstaller.Password = null;
            this.agentServiceProcessInstaller.Username = null;
            this.agentServiceProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.serviceProcessInstaller1_AfterInstall);
            // 
            // agentServiceInstaller
            // 
            this.agentServiceInstaller.Description = "OpenStack Designate Microsoft DNS Agent";
            this.agentServiceInstaller.DisplayName = "OpenStack Designate Microsoft DNS Agent";
            this.agentServiceInstaller.ServiceName = "AgentService";
            this.agentServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.agentServiceProcessInstaller,
            this.agentServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller agentServiceProcessInstaller;
        private System.ServiceProcess.ServiceInstaller agentServiceInstaller;
    }
}