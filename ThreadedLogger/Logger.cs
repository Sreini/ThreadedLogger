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
        private const int CollectionUpperBound = 100000;

        private readonly BlockingCollection<Action> _queue = new BlockingCollection<Action>(new ConcurrentQueue<Action>(), CollectionUpperBound);
        private readonly Thread _loggerThread;

        private readonly ILoggingSink _log;

        private readonly ManualResetEvent hasNewItems = new ManualResetEvent(false);
        private readonly ManualResetEvent terminate = new ManualResetEvent(false);
        private readonly ManualResetEvent waiting = new ManualResetEvent(false);

        /// <summary>
        /// constructor. Initializes a thread
        /// </summary>
        private Logger(ILoggingSink log)
        {
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _loggerThread = new Thread(ProcessQueue)
            {
                IsBackground = true,
            };
            _loggerThread.Start();
        }

        private void ProcessQueue()
        {
            while (true)
            {
                waiting.Set();

                //thread waits here until either hasNewItems or terminate are signaled
                var waitHandler = WaitHandle.WaitAny(new WaitHandle[] { hasNewItems, terminate });

                //terminate was signaled
                if (waitHandler == 1)
                {
                    return;
                }

                hasNewItems.Reset();
                waiting.Reset();
                while (_queue.Count != 0)
                {
                    var log = _queue.Take();
                    log();
                }
            }
        }
        

        public void Flush()
        {
            waiting.WaitOne();
        }

        public void Dispose()
        {
            terminate.Set();
            _loggerThread.Join();
        }

        public void Info(string message)
        {
            if (!_queue.IsAddingCompleted)
            {
                _queue.Add(() => _log.LogInformative(message));
                hasNewItems.Set(); 
            }
        }

        public void Warning(string message)
        {
            throw new NotImplementedException();
        }

        public void Warning(string message, Exception ex)
        {
            throw new NotImplementedException();
        }

        public void HandleException(string message, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
