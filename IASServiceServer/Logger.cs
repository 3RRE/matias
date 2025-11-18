using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace IASServiceServer
{
    public static class Logger
    {
        private static readonly TraceSource Trace = new TraceSource("Essential.Diagnostics.Source");

        public static void Write(TraceEventType eventType, params object[] data)
        {
            Trace.TraceData(eventType, 0, data);
            Trace.Flush();
        }

        public static void Verbose(params object[] data)
        {
            Trace.TraceData(TraceEventType.Verbose, 0, data);
            Trace.Flush();
        }

        public static void Information(params object[] data)
        {
            Trace.TraceData(TraceEventType.Information, 0, data);
            Trace.Flush();
        }

        public static void Warning(params object[] data)
        {
            Trace.TraceData(TraceEventType.Warning, 0, data);
            Trace.Flush();
        }

        public static void Error(params object[] data)
        {
            Trace.TraceData(TraceEventType.Error, 0, data);
            Trace.Flush();
        }

        public static void Error(Exception exception)
        {
            var messages = new List<string>();
            string message;
            do
            {
                message = exception.Message;
                if (!string.IsNullOrEmpty(message))
                {
                    messages.Add(message);
                }

                exception = exception.InnerException;
            } while (exception != null);

            message = string.Join(", ", messages);

            Trace.TraceData(TraceEventType.Error, 0, message);
            Trace.Flush();
        }

        public static void Critical(params object[] data)
        {
            Trace.TraceData(TraceEventType.Critical, 0, data);
            Trace.Flush();
        }
    }
}
