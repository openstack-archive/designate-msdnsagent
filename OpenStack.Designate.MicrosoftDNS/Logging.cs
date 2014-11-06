using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenStack.Designate.MicrosoftDNS
{

    public enum LoggingContext
    {
        Console, EventLog
    }

    public enum LogLevel
    {
        Critical, Error, Warning, Information, Verbose
    }

    public class Logging
    {
        private static Logging _instance;
        private LoggingContext context = LoggingContext.Console;
        private ConfigManager _Config;
        private EventLog _EventLog;

        private Logging(LoggingContext context, ConfigManager config) {
            this.context = context;

            if (this.context == LoggingContext.EventLog)
            {
                this._EventLog = new System.Diagnostics.EventLog();
                this._EventLog.Source = "Openstack Designate MSDNSAgent";
                this._EventLog.Log = "Application";
                if (!EventLog.SourceExists(this._EventLog.Source))
                {
                    EventLog.CreateEventSource(this._EventLog.Source, this._EventLog.Log);
                }
            }

            this._Config = config;
        }
        
        public static Logging GetLogger(LoggingContext context, ConfigManager config)
        {
            if (_instance == null)
            {
                _instance = new Logging(context, config);
            }
            return _instance;
        }

        public static Logging GetLogger()
        {
            if (_instance == null)
            {
                throw new NotImplementedException();
            }
            return _instance;
        }
        
        public void Log(LogLevel level, string message)
        {
            StackFrame callstack = new StackFrame(1);

            string caller = "[" + callstack.GetMethod().Module + ":" + callstack.GetMethod().Name + "]";
            string time = "[" + DateTime.Now.ToShortTimeString() + "]";
            string level_ = "[" + level + "]";

            if (this.context == LoggingContext.Console)
            {
                if (_Config.LogLevel >= level)
                {
                    System.Console.WriteLine( time + " " + level_.PadLeft(13) + " " + caller.PadRight(65) + ":: " + message);
                }
            }
            else if (this.context == LoggingContext.EventLog)
            {
                EventLogEntryType windowsLevel;
                switch (level)
                {
                    case LogLevel.Critical:
                        windowsLevel = EventLogEntryType.Error;
                        break;
                    case LogLevel.Error:
                        windowsLevel = EventLogEntryType.Error;
                        break;
                    case LogLevel.Warning:
                        windowsLevel = EventLogEntryType.Warning;
                        break;
                    case LogLevel.Information:
                        windowsLevel = EventLogEntryType.Information;
                        break;
                    case LogLevel.Verbose:
                        windowsLevel = EventLogEntryType.Information;
                        break;
                    default:
                        windowsLevel = EventLogEntryType.Information;
                        break;
                }
                this._EventLog.WriteEntry(caller + message, windowsLevel);
            }

        }
    }
}
