using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace ThreadedLogger
{
    public class Logger : ILogger
    {
        private ConcurrentQueue<Action> _queue = new ConcurrentQueue<Action>();
        private Thread _loggerThread;
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
    }
}
