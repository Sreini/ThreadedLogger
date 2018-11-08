using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ThreadedLogger
{
    public class Logger : ILogger
    {
        //TODO: make fields injectable
        private readonly BlockingCollection<Action> _queue = new BlockingCollection<Action>(new ConcurrentQueue<Action>());
        private readonly Thread _loggerThread;

        /// <summary>
        /// constructor. Initializes a thread
        /// </summary>
        private Logger()
        {
            _loggerThread = new Thread(ProcessQueue)
            {
                IsBackground = true,
            };
            _loggerThread.Start();
        }

        void ProcessQueue()
        {
            throw new NotImplementedException();
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void LogInfo(string message)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void HandleException(string message, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
