using System;
using System.Collections.Concurrent;
using System.Threading;

namespace WorkerQueue
{
    /// <inheritdoc/>
    public class WorkerQueue : IWorkerQueue
    {
        private const int DefaultCollectionUpperBound = 1000;
        private const int DefaultTimeout = -1;

        private readonly BlockingCollection<Action> _queue;
        private readonly Thread _workerThread;

        private readonly ManualResetEvent _hasNewItems = new ManualResetEvent(false);
        private readonly ManualResetEvent _endProcess = new ManualResetEvent(false);
        private readonly ManualResetEvent _waiting = new ManualResetEvent(false);

        /// <summary>
        /// constructor that accepts timeout and the upper bound of the collection as optional parameters
        /// </summary>
        /// <param name="timeout">time in milliseconds during which the process will try to take an item from the queue, or -1 for this time being indefinite</param>
        /// <param name="collectionUpperBound">upper bound amount of items in queue</param>
        public WorkerQueue(int timeout = DefaultTimeout, int collectionUpperBound = DefaultCollectionUpperBound)
        {
            _queue = new BlockingCollection<Action>(new ConcurrentQueue<Action>(), collectionUpperBound);

            _workerThread = new Thread(ProcessQueue)
            {
                IsBackground = true,
            };
            object parameters = timeout;
            _workerThread.Start(parameters);
        }

        /// <summary>
        /// processes jobs on the queue
        /// </summary>
        private void ProcessQueue(object parameters)
        {
            var timeout = (int) parameters;
            DoWork(timeout);
        }

        /// <summary>
        /// main loop for processing jobs. 
        /// </summary>
        /// <param name="timeout"></param>
        private void DoWork(int timeout)
        {
            while (true)
            {
                _waiting.Set();

                //thread waits here until either _hasNewItems or _endProcess are signaled
                var waitHandler = WaitHandle.WaitAny(new WaitHandle[] { _hasNewItems, _endProcess });

                //_endProcess was signaled
                if (waitHandler == 1)
                {
                    return;
                }

                _hasNewItems.Reset();
                _waiting.Reset();
                while (_queue.Count != 0)
                {
                    if(_queue.TryTake(out var action, timeout))
                    {
                        action();
                    }
                }
            }
        }

        /// <inheritdoc cref="IWorkerQueue.Flush"/>
        public void Flush()
        {
            _waiting.WaitOne();
        }

        /// <inheritdoc cref="IWorkerQueue.Dispose"/>
        public void Dispose()
        {
            _endProcess.Set();
            _workerThread.Join();
            _queue.Dispose();
        }

        /// <inheritdoc cref="IWorkerQueue.AddJob"/>
        public void AddJob(Action action)
        {
            if (_queue.IsAddingCompleted) return;
            _queue.Add(action);
            _hasNewItems.Set();
        }
    }
}
