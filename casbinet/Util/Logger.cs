namespace casbinet.Util
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;

    public static class Logger
    {
        private static bool enableLog = true;

        private static readonly object logLock = new object();
        //todo : Added log levels

        public static void Log(LogType logType, params string[] logs)
        {
            
        }
    }

    [Flags]
    public enum LogType
    {
        None,
        Info = 1,
        Message = 2,
        Debug = 4,
        Trace = 8,
        Warning = 16,
        Exception = 32,
        Error = 64,
        Fatal = 128,
        All = 255
    }
}
