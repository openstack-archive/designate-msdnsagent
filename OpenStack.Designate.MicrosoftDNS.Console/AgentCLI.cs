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
using System.Threading;
using CommandLine;
using CommandLine.Text;
using OpenStack.Designate.MicrosoftDNS;

namespace OpenStack.Designate.MicrosoftDNS.Console
{
    class AgentCLI
    {
        class Options
        {
            public Options()
            {
                this.AgentService = new AgentServiceSubOptions { };
            }

            //Top Level Options

            [Option('d', "debug", DefaultValue = false, HelpText = "Enable Debug Level Logging")]
            public bool Debug { get; set; }

            [VerbOption("agent-service", HelpText = "Start the Agent Service")]
            public AgentServiceSubOptions AgentService { get; set; }

            [VerbOption("add-zone", HelpText = "Add a Zone Manually")]
            public AddZoneSubOptions AddZone { get; set; }

            [VerbOption("get-zone", HelpText = "Get a Zone Details")]
            public GetZoneSubOptions GetZone { get; set; }

            [VerbOption("delete-zone", HelpText = "Delete a zone manually")]
            public DeleteZoneSubOptions DeleteZone { get; set; }

            [VerbOption("get-config", HelpText = "Get a Local Config Details")]
            public GetConfigSubOptions GetConfig { get; set; }
            //Internal Parser
            
            [ParserState]
            public IParserState LastParserState { get; set; }

            [HelpVerbOption]
            public string GetUsage(string verb)
            {
                return HelpText.AutoBuild(this, verb);
            }
        }

        //2nd Level Option Classes
        class GetConfigSubOptions {}

        class AgentServiceSubOptions {}

        class AddZoneSubOptions 
        {
            [Option("zone-name",Required=true , HelpText = "Zone Name")]
            public string ZoneName { get; set; }
        }

        class GetZoneSubOptions 
        {
            [Option("zone-name",Required=true , HelpText = "Zone Name")]
            public string ZoneName { get; set; }
        }

        class DeleteZoneSubOptions 
        {
            [Option("zone-name",Required=true , HelpText = "Zone Name")]
            public string ZoneName { get; set; }
        }
                
        public static void Main(string[] args)
        {
            string invokedVerb = null;
            object invokedVerbInstance = null;

            var options = new Options();

            if (!CommandLine.Parser.Default.ParseArguments(args, options,
              (verb, subOptions) =>
              {
                  // if parsing succeeds the verb name and correct instance
                  // will be passed to onVerbCommand delegate (string,object)
                  invokedVerb = verb;
                  invokedVerbInstance = subOptions;
              }))
            {
                Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
            }

            switch (invokedVerb)
            {
                case "agent-service":
                    AgentCLI.StartAgentService(options, (AgentServiceSubOptions)invokedVerbInstance);
                    break;
                case "add-zone":
                    AgentCLI.AddZone(options, (AddZoneSubOptions)invokedVerbInstance);
                    break;
                case "get-zone":
                    AgentCLI.GetZone(options, (GetZoneSubOptions)invokedVerbInstance);
                    break;
                case "delete-zone":
                    AgentCLI.DeleteZone(options, (DeleteZoneSubOptions)invokedVerbInstance);
                    break;
                case "get-config":
                    AgentCLI.GetConfig(options);
                    break;
                default:
                    Environment.Exit(CommandLine.Parser.DefaultExitCodeFail);
                    break;
            }
        }

        private static void StartAgentService(Options options, AgentServiceSubOptions subOptions)
        {
            ConfigManager _Config = new ConfigManager();
            Logging LOG = Logging.GetLogger(LoggingContext.Console, _Config);
            AgentConsumer agentConsumer = new AgentConsumer(_Config);

            agentConsumer.Start();
            
            Thread ServiceThread = new Thread(agentConsumer.Consume);
            ServiceThread.Start();
            LOG.Log(LogLevel.Information, "Started Service Thread");
            LOG.Log(LogLevel.Verbose, "Thread ID: " + ServiceThread.ManagedThreadId);
            System.Console.CancelKeyPress += delegate(object sender, ConsoleCancelEventArgs e)
            {
                e.Cancel = true;

                LOG.Log(LogLevel.Information, "Caught Ctrl+C - Stopping Service");

                agentConsumer.Stop();
                LOG.Log(LogLevel.Verbose, "Blocking on Service Thread Completion");
                ServiceThread.Join();
            };
		}

        private static void AddZone(Options options, AddZoneSubOptions subOptions)
        {
            ConfigManager _Config = new ConfigManager();
            Logging LOG = Logging.GetLogger(LoggingContext.Console, _Config);



            DNSServer dns = new DNSServer(_Config);
            LOG.Log(LogLevel.Information, "Connecting to WMI");
            dns.Connect();
            LOG.Log(LogLevel.Information, "Adding Zone");
            dns.CreateNewZone(subOptions.ZoneName, _Config.DNSMasters);
        }

        private static void GetZone(Options options, GetZoneSubOptions subOptions)
        {
            ConfigManager _Config = new ConfigManager();
            Logging LOG = Logging.GetLogger(LoggingContext.Console, _Config);

            DNSServer dns = new DNSServer(_Config);
            LOG.Log(LogLevel.Information, "Connecting to WMI");
            dns.Connect();
            LOG.Log(LogLevel.Verbose, "Getting Zone");
            DNSServer.DNSZone zone = dns.FetchZone(subOptions.ZoneName);
            LOG.Log(LogLevel.Information, "Zone Retrieved: "+ zone.Name);
            LOG.Log(LogLevel.Information, "Zone Masters: ");
            foreach (string master in zone.MasterServers)
            {
                LOG.Log(LogLevel.Information, master);
            }
        }

        private static void DeleteZone(Options options, DeleteZoneSubOptions subOptions)
        {
            ConfigManager _Config = new ConfigManager();
            Logging LOG = Logging.GetLogger(LoggingContext.Console, _Config);

            DNSServer dns = new DNSServer(_Config);
            LOG.Log(LogLevel.Information, "Connecting to WMI");
            dns.Connect();
            LOG.Log(LogLevel.Verbose, "Fetching Zone to Delete");
            DNSServer.DNSZone zone = dns.FetchZone(subOptions.ZoneName);
            LOG.Log(LogLevel.Verbose, "Deleting Zone");
            zone.DeleteZone();
            LOG.Log(LogLevel.Information, "Zone Deleted");
        }

        private static void GetConfig(Options options)
        {
            ConfigManager _Config = new ConfigManager();
            Logging LOG = Logging.GetLogger(LoggingContext.Console, _Config);

            LOG.Log(LogLevel.Information, "::Config Details:: ");
            LOG.Log(LogLevel.Information, "Rabbit Hosts:");
            foreach (string host in _Config.RMQHosts)
            {
                LOG.Log(LogLevel.Information, host);
            }
            LOG.Log(LogLevel.Information, "Rabbit MQ User: " + _Config.RMQUser);
            LOG.Log(LogLevel.Information, "Rabbit MQ Port: " + _Config.RMQPort);
            LOG.Log(LogLevel.Information, "Master DNS Servers:");
            foreach (string host in _Config.DNSMasters)
            {
                LOG.Log(LogLevel.Information, host);
            }
            LOG.Log(LogLevel.Information, "Agent Name: " + _Config.AgentName);
            LOG.Log(LogLevel.Information, "Logging Level: " + _Config.LogLevel);
            LOG.Log(LogLevel.Information, "WMI Host: " + _Config.WMIHost);
            LOG.Log(LogLevel.Information, "WMI User: " + _Config.WMIUser);
        }
    }
}
