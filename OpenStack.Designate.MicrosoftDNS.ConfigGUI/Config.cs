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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OpenStack.Designate.MicrosoftDNS;
using RabbitMQ.Client;

namespace OpenStack.Designate.MicrosoftDNS
{
    public partial class Config : Form
    {

        private ConfigManager _Config;
        private AgentConsumer _AgentConsumer;

        private Logging LOG;

        public Config()
        {
            InitializeComponent();
            this._Config = new ConfigManager();
            this.LOG = Logging.GetLogger(LoggingContext.EventLog, _Config);
            this._AddEvents();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            
            //Get current values, & populate inputs
            this.RMQPortInput.Value = this._Config.RMQPort;
            this.RMQUserInput.Text = this._Config.RMQUser;
            this.RMQPasswordInput.Text = this._Config.RMQPassword;
            this.RMQPasswordConfirmInput.Text = this._Config.RMQPassword;
            this.RMQHostsInput.Lines = this._Config.RMQHosts;
            this.MasterDNSInput.Lines = this._Config.DNSMasters;
            this.AgentNameInput.Text = this._Config.AgentName;
        }

        private void _AddEvents()
        {
            this.QuitButton.Click += QuitButton_Click;
            this.SaveButton.Click += SaveButton_Click;
            this.ValidateButon.Click += ValidateButon_Click;
        }

        void ValidateButon_Click(object sender, EventArgs e)
        {
            this.Validate();
        }

        void SaveButton_Click(object sender, EventArgs e)
        {
            if (this.Validate()){
                this._Save();
                this._Quit();
            }
        }

        private void _ToggleButtons()
        {
            this.ValidateButon.Enabled = !this.ValidateButon.Enabled;
            this.SaveButton.Enabled = !this.SaveButton.Enabled;
            this.QuitButton.Enabled = !this.QuitButton.Enabled;
        }

        new public bool Validate()
        {
            this.StatusLabel.Text = "Validating";
            this.StatusLabel.ForeColor = Color.Black;
            this.ProgressBar.Value = 40;
            this.ProgressBar.Visible = true;
            this._ToggleButtons();
            try
            {
                this.RMQHostsInput.Lines = this._StripBlankLines(this.RMQHostsInput.Lines);
                this.MasterDNSInput.Lines = this._StripBlankLines(this.MasterDNSInput.Lines);
                this._Validate();
                
                this.StatusLabel.Text = "Values OK";
                this.StatusLabel.ForeColor = Color.Green;
                this.ProgressBar.Value = 100;
                this._ToggleButtons();
                
                return true;
            }
            catch (Exception error)
            {
                MessageBox.Show("There is an error in the values supplied:" + error.Message, "Validation Error");
                
                this.StatusLabel.Text = "Error in Validation";
                this.StatusLabel.ForeColor = Color.Red;
                this.ProgressBar.Value = 100;
                this._ToggleButtons();
                
                return false;
            }

        }

        void QuitButton_Click(object sender, EventArgs e)
        {
            this._Quit();
        }

        private void _Validate()
        {
            bool Errors = false;
            string ErrorMessage = "";
            this.ProgressBar.Value = 40;
            if (this.RMQPortInput.Value == 0)
            {
                ErrorMessage += Environment.NewLine + "RabbitMQ Port Number is invalid";
                Errors = true;
            }
            if (this.RMQPasswordInput.Text != this.RMQPasswordConfirmInput.Text)
            {
                ErrorMessage += Environment.NewLine + "RabbitMQ passwords do not match";
                Errors = true;
                throw new Exception(ErrorMessage);
            }
            if (this.RMQHostsInput.Lines.Length == 0){
                ErrorMessage += Environment.NewLine + "RabbitMQ hosts is blank.";
                Errors = true;
            }
            if (this.MasterDNSInput.Lines.Length == 0)
            {
                ErrorMessage += Environment.NewLine + "Master DNS Servers is blank.";
                Errors = true;
            }
            if (this.RMQUserInput.TextLength == 0)
            {
                ErrorMessage += Environment.NewLine + "RabbitMQ Username is blank.";
                Errors = true;
            }
            if (this.AgentNameInput.TextLength == 0)
            {
                ErrorMessage += Environment.NewLine + "Agent Name is blank.";
                Errors = true;
            }
            if (this.RMQPasswordInput.TextLength == 0)
            {
                ErrorMessage += Environment.NewLine + "RabbitMQ Password is blank.";
                Errors = true;
            }
            this.ProgressBar.Value = 60;
            try
            {
                foreach (string host in this.RMQHostsInput.Lines)
                {
                    string[] _TestHosts = new string[1] {host};
                    ConfigManager _TestConfig = new ConfigManager(false, false, _TestHosts, null, this.RMQUserInput.Text, this.RMQPasswordInput.Text, (int)this.RMQPortInput.Value);
                    this._AgentConsumer = new AgentConsumer(_TestConfig);
                    this._AgentConsumer.Start();
                    this._AgentConsumer.Stop();
                }
            }
            catch (RabbitMQ.Client.Exceptions.BrokerUnreachableException e)
            {
                this._AgentConsumer.Stop();
                if (e.InnerException != null && e.InnerException is RabbitMQ.Client.Exceptions.AuthenticationFailureException)
                {
                    ErrorMessage += Environment.NewLine + "RabbitMQ Authentication problem. Please check the RabbitMQ username and password values";
                    Errors = true;
                }
                else
                {
                    ErrorMessage += Environment.NewLine + "RabbitMQ Connection problem. Please check the values set for Rabbit MQ Hosts and try again";
                    Errors = true;
                }
            }
            catch (Exception)
            {
                ErrorMessage += Environment.NewLine + "Unspecified RabbitMQ problem. Please check all RabbitMQ values";
                Errors = true;
            }

            if (Errors)
            {
                throw new Exception(ErrorMessage);
            }
        }

        private string[] _StripBlankLines(string[] input)
        {
            for (int i = 0; i < input.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(input[i]))
                {
                    input = input.Where(w => w != input[i]).ToArray();
                }
            }
            return input;
        }

        private void _Save()
        {
            this._Config.DNSMasters = this.MasterDNSInput.Lines;
            this._Config.RMQHosts = this.RMQHostsInput.Lines;
            this._Config.RMQUser =this.RMQUserInput.Text;
            this._Config.RMQPassword = this.RMQPasswordInput.Text;
            this._Config.RMQPort = (int)this.RMQPortInput.Value;
            this._Config.AgentName = this.AgentNameInput.Text;
        }

        private void _Quit()
        {
            Application.Exit();
        }
    }
}
