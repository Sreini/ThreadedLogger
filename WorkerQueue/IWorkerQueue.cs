using System;

namespace WorkerQueue
{
    /// <summary>
    /// Class that instantiates a queue in a different thread and performs any jobs pushed to the queue in that thread.
    /// </summary>
    public interface IWorkerQueue: IDisposable
    {
        /// <summary>
        /// Flushes the queue. Useful for making sure the queue is empty, for example during error handling
        /// </summary>
        void Flush();

        /// <summary>
        /// Disposes the queue
        /// </summary>
        new void Dispose();

        /// <summary>
        /// Adds a job to the queue. The job should be performed in a parallel thread
        /// </summary>
        /// <param name="action">The job added to the thread</param>
        /// <exception cref="ObjectDisposedException">Will be thrown if this method is called after the Dispose method has been called</exception>
        void AddJob(Action action);


    }
}
