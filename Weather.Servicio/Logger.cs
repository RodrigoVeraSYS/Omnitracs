using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Weather.Servicio
{
    public static class Logger
    {
        private static EventLog eventLog;
        static Logger()
        {
            eventLog = new EventLog();
            if (!EventLog.SourceExists("Weather"))
            {
                EventLog.CreateEventSource(
                     "Weather", "WeatherLog");
            }
            eventLog.Source = "WeatherSource";
            eventLog.Log = "WeatherLog";
        }
        public static void LogException(Exception ex)
        {
            eventLog.WriteEntry(ex.Message + "\n" + ex.StackTrace);
            if (ex.InnerException != null)
            {
                LogException(ex.InnerException);
            }
        }
        public static void WriteEntry(string Message)
        {
            eventLog.WriteEntry(Message);
        }
    }
}
