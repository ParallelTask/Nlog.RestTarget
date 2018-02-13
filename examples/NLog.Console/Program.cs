using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace NLog.Console
{
    class Program
    {
        private static LogEventInfo SetEventPropeties(ILogger logger, string message, Dictionary<string, string> dictionay, LogLevel loglevel)
        {
            LogEventInfo theEvent = new LogEventInfo(loglevel, logger.Name, message);

            foreach (KeyValuePair<string, string> item in dictionay)
            {
                theEvent.Properties[item.Key] = item.Value;
            }

            return theEvent;
        }

        static void Main(string[] args)
        {
            Logger wslogger = LogManager.GetLogger("rs");

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("customeventproperty", "CUSTOM_EVENT_PROPERTY");
            MappedDiagnosticsLogicalContext.Set("customheader", "CUSTOM_HEADER");

            wslogger.Log(SetEventPropeties(wslogger, "This is Trace message", dictionary, LogLevel.Trace));

            System.Console.ReadLine();
        }
    }
}
