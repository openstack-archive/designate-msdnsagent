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

namespace OpenStack.Designate.MicrosoftDNS
{
    partial class Config
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.configManagerBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.RMQDetails = new System.Windows.Forms.GroupBox();
            this.RMQPasswordConfirmInput = new System.Windows.Forms.TextBox();
            this.RMQPasswordConfirmLabel = new System.Windows.Forms.Label();
            this.RMQPasswordInput = new System.Windows.Forms.TextBox();
            this.RMQPasswordLabel = new System.Windows.Forms.Label();
            this.RMQUserInput = new System.Windows.Forms.TextBox();
            this.RMQUserLabel = new System.Windows.Forms.Label();
            this.RMQPortInput = new System.Windows.Forms.NumericUpDown();
            this.RMQPortLabel = new System.Windows.Forms.Label();
            this.RMQHostsInput = new System.Windows.Forms.TextBox();
            this.RQMHostsLabel = new System.Windows.Forms.Label();
            this.DNSSettings = new System.Windows.Forms.GroupBox();
            this.MasterDNSInput = new System.Windows.Forms.TextBox();
            this.MasterDNSLabel = new System.Windows.Forms.Label();
            this.QuitButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.AgentSettings = new System.Windows.Forms.GroupBox();
            this.AgentNameInput = new System.Windows.Forms.TextBox();
            this.AgentNameLabel = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.ValidateButon = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.configManagerBindingSource)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.RMQDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RMQPortInput)).BeginInit();
            this.DNSSettings.SuspendLayout();
            this.AgentSettings.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // configManagerBindingSource
            // 
            this.configManagerBindingSource.DataSource = typeof(OpenStack.Designate.MicrosoftDNS.ConfigManager);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.RMQDetails);
            this.flowLayoutPanel1.Controls.Add(this.DNSSettings);
            this.flowLayoutPanel1.Controls.Add(this.AgentSettings);
            this.flowLayoutPanel1.Controls.Add(this.QuitButton);
            this.flowLayoutPanel1.Controls.Add(this.SaveButton);
            this.flowLayoutPanel1.Controls.Add(this.ValidateButon);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(12, 21);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(309, 493);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // RMQDetails
            // 
            this.RMQDetails.Controls.Add(this.RMQPasswordConfirmInput);
            this.RMQDetails.Controls.Add(this.RMQPasswordConfirmLabel);
            this.RMQDetails.Controls.Add(this.RMQPasswordInput);
            this.RMQDetails.Controls.Add(this.RMQPasswordLabel);
            this.RMQDetails.Controls.Add(this.RMQUserInput);
            this.RMQDetails.Controls.Add(this.RMQUserLabel);
            this.RMQDetails.Controls.Add(this.RMQPortInput);
            this.RMQDetails.Controls.Add(this.RMQPortLabel);
            this.RMQDetails.Controls.Add(this.RMQHostsInput);
            this.RMQDetails.Controls.Add(this.RQMHostsLabel);
            this.RMQDetails.Location = new System.Drawing.Point(0, 3);
            this.RMQDetails.Name = "RMQDetails";
            this.RMQDetails.Size = new System.Drawing.Size(306, 237);
            this.RMQDetails.TabIndex = 0;
            this.RMQDetails.TabStop = false;
            this.RMQDetails.Text = "Rabbit MQ Connection Details";
            // 
            // RMQPasswordConfirmInput
            // 
            this.RMQPasswordConfirmInput.Location = new System.Drawing.Point(116, 208);
            this.RMQPasswordConfirmInput.Name = "RMQPasswordConfirmInput";
            this.RMQPasswordConfirmInput.PasswordChar = '*';
            this.RMQPasswordConfirmInput.Size = new System.Drawing.Size(184, 20);
            this.RMQPasswordConfirmInput.TabIndex = 10;
            // 
            // RMQPasswordConfirmLabel
            // 
            this.RMQPasswordConfirmLabel.AutoSize = true;
            this.RMQPasswordConfirmLabel.Location = new System.Drawing.Point(6, 211);
            this.RMQPasswordConfirmLabel.Name = "RMQPasswordConfirmLabel";
            this.RMQPasswordConfirmLabel.Size = new System.Drawing.Size(91, 13);
            this.RMQPasswordConfirmLabel.TabIndex = 9;
            this.RMQPasswordConfirmLabel.Text = "Confirm Password";
            // 
            // RMQPasswordInput
            // 
            this.RMQPasswordInput.Location = new System.Drawing.Point(116, 177);
            this.RMQPasswordInput.Name = "RMQPasswordInput";
            this.RMQPasswordInput.PasswordChar = '*';
            this.RMQPasswordInput.Size = new System.Drawing.Size(184, 20);
            this.RMQPasswordInput.TabIndex = 8;
            // 
            // RMQPasswordLabel
            // 
            this.RMQPasswordLabel.AutoSize = true;
            this.RMQPasswordLabel.Location = new System.Drawing.Point(6, 180);
            this.RMQPasswordLabel.Name = "RMQPasswordLabel";
            this.RMQPasswordLabel.Size = new System.Drawing.Size(104, 13);
            this.RMQPasswordLabel.TabIndex = 7;
            this.RMQPasswordLabel.Text = "RabbitMQ Password";
            // 
            // RMQUserInput
            // 
            this.RMQUserInput.Location = new System.Drawing.Point(116, 147);
            this.RMQUserInput.Name = "RMQUserInput";
            this.RMQUserInput.Size = new System.Drawing.Size(184, 20);
            this.RMQUserInput.TabIndex = 6;
            // 
            // RMQUserLabel
            // 
            this.RMQUserLabel.AutoSize = true;
            this.RMQUserLabel.Location = new System.Drawing.Point(6, 150);
            this.RMQUserLabel.Name = "RMQUserLabel";
            this.RMQUserLabel.Size = new System.Drawing.Size(80, 13);
            this.RMQUserLabel.TabIndex = 5;
            this.RMQUserLabel.Text = "RabbitMQ User";
            // 
            // RMQPortInput
            // 
            this.RMQPortInput.Location = new System.Drawing.Point(116, 117);
            this.RMQPortInput.Maximum = new decimal(new int[] {
            63535,
            0,
            0,
            0});
            this.RMQPortInput.Name = "RMQPortInput";
            this.RMQPortInput.Size = new System.Drawing.Size(184, 20);
            this.RMQPortInput.TabIndex = 4;
            // 
            // RMQPortLabel
            // 
            this.RMQPortLabel.AutoSize = true;
            this.RMQPortLabel.Location = new System.Drawing.Point(6, 119);
            this.RMQPortLabel.Name = "RMQPortLabel";
            this.RMQPortLabel.Size = new System.Drawing.Size(77, 13);
            this.RMQPortLabel.TabIndex = 2;
            this.RMQPortLabel.Text = "RabbitMQ Port";
            // 
            // RMQHostsInput
            // 
            this.RMQHostsInput.Location = new System.Drawing.Point(116, 20);
            this.RMQHostsInput.Multiline = true;
            this.RMQHostsInput.Name = "RMQHostsInput";
            this.RMQHostsInput.Size = new System.Drawing.Size(184, 83);
            this.RMQHostsInput.TabIndex = 1;
            // 
            // RQMHostsLabel
            // 
            this.RQMHostsLabel.AutoSize = true;
            this.RQMHostsLabel.Location = new System.Drawing.Point(6, 23);
            this.RQMHostsLabel.Name = "RQMHostsLabel";
            this.RQMHostsLabel.Size = new System.Drawing.Size(85, 13);
            this.RQMHostsLabel.TabIndex = 0;
            this.RQMHostsLabel.Text = "RabbitMQ Hosts";
            // 
            // DNSSettings
            // 
            this.DNSSettings.Controls.Add(this.MasterDNSInput);
            this.DNSSettings.Controls.Add(this.MasterDNSLabel);
            this.DNSSettings.Location = new System.Drawing.Point(0, 246);
            this.DNSSettings.Name = "DNSSettings";
            this.DNSSettings.Size = new System.Drawing.Size(306, 109);
            this.DNSSettings.TabIndex = 1;
            this.DNSSettings.TabStop = false;
            this.DNSSettings.Text = "DNS Settings";
            // 
            // MasterDNSInput
            // 
            this.MasterDNSInput.Location = new System.Drawing.Point(116, 13);
            this.MasterDNSInput.Multiline = true;
            this.MasterDNSInput.Name = "MasterDNSInput";
            this.MasterDNSInput.Size = new System.Drawing.Size(184, 83);
            this.MasterDNSInput.TabIndex = 12;
            // 
            // MasterDNSLabel
            // 
            this.MasterDNSLabel.AutoSize = true;
            this.MasterDNSLabel.Location = new System.Drawing.Point(6, 16);
            this.MasterDNSLabel.Name = "MasterDNSLabel";
            this.MasterDNSLabel.Size = new System.Drawing.Size(104, 13);
            this.MasterDNSLabel.TabIndex = 11;
            this.MasterDNSLabel.Text = "Master DNS Servers";
            // 
            // QuitButton
            // 
            this.QuitButton.Location = new System.Drawing.Point(231, 417);
            this.QuitButton.Name = "QuitButton";
            this.QuitButton.Size = new System.Drawing.Size(75, 23);
            this.QuitButton.TabIndex = 3;
            this.QuitButton.Text = "Quit";
            this.QuitButton.UseVisualStyleBackColor = true;
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(150, 417);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 2;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = true;
            // 
            // AgentSettings
            // 
            this.AgentSettings.Controls.Add(this.AgentNameInput);
            this.AgentSettings.Controls.Add(this.AgentNameLabel);
            this.AgentSettings.Location = new System.Drawing.Point(0, 361);
            this.AgentSettings.Name = "AgentSettings";
            this.AgentSettings.Size = new System.Drawing.Size(306, 50);
            this.AgentSettings.TabIndex = 4;
            this.AgentSettings.TabStop = false;
            this.AgentSettings.Text = "Agent Settings";
            // 
            // AgentNameInput
            // 
            this.AgentNameInput.Location = new System.Drawing.Point(116, 18);
            this.AgentNameInput.Name = "AgentNameInput";
            this.AgentNameInput.Size = new System.Drawing.Size(184, 20);
            this.AgentNameInput.TabIndex = 12;
            // 
            // AgentNameLabel
            // 
            this.AgentNameLabel.AutoSize = true;
            this.AgentNameLabel.Location = new System.Drawing.Point(6, 21);
            this.AgentNameLabel.Name = "AgentNameLabel";
            this.AgentNameLabel.Size = new System.Drawing.Size(66, 13);
            this.AgentNameLabel.TabIndex = 11;
            this.AgentNameLabel.Text = "Agent Name";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.ProgressBar});
            this.statusStrip1.Location = new System.Drawing.Point(0, 504);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(333, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(23, 17);
            this.StatusLabel.Text = "OK";
            // 
            // ProgressBar
            // 
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(100, 16);
            this.ProgressBar.Visible = false;
            // 
            // ValidateButon
            // 
            this.ValidateButon.Location = new System.Drawing.Point(69, 417);
            this.ValidateButon.Name = "ValidateButon";
            this.ValidateButon.Size = new System.Drawing.Size(75, 23);
            this.ValidateButon.TabIndex = 5;
            this.ValidateButon.Text = "Validate";
            this.ValidateButon.UseVisualStyleBackColor = true;
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 526);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "Config";
            this.Text = "OpenStack Designate MicrosoftDNS Agent Config";
            this.Load += new System.EventHandler(this.Config_Load);
            ((System.ComponentModel.ISupportInitialize)(this.configManagerBindingSource)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.RMQDetails.ResumeLayout(false);
            this.RMQDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RMQPortInput)).EndInit();
            this.DNSSettings.ResumeLayout(false);
            this.DNSSettings.PerformLayout();
            this.AgentSettings.ResumeLayout(false);
            this.AgentSettings.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.BindingSource configManagerBindingSource;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox RMQDetails;
        private System.Windows.Forms.Label RQMHostsLabel;
        private System.Windows.Forms.TextBox RMQHostsInput;
        private System.Windows.Forms.Label RMQPortLabel;
        private System.Windows.Forms.NumericUpDown RMQPortInput;
        private System.Windows.Forms.TextBox RMQPasswordInput;
        private System.Windows.Forms.Label RMQPasswordLabel;
        private System.Windows.Forms.TextBox RMQUserInput;
        private System.Windows.Forms.Label RMQUserLabel;
        private System.Windows.Forms.TextBox RMQPasswordConfirmInput;
        private System.Windows.Forms.Label RMQPasswordConfirmLabel;
        private System.Windows.Forms.GroupBox DNSSettings;
        private System.Windows.Forms.TextBox MasterDNSInput;
        private System.Windows.Forms.Label MasterDNSLabel;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.Button QuitButton;
        private System.Windows.Forms.GroupBox AgentSettings;
        private System.Windows.Forms.TextBox AgentNameInput;
        private System.Windows.Forms.Label AgentNameLabel;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.ToolStripProgressBar ProgressBar;
        private System.Windows.Forms.Button ValidateButon;
    }
}

