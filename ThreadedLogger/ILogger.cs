using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadedLogger
{
    public interface ILogger : IDisposable
    {
        void Info(string message);

        void Warning(string message);

        void Warning(string message, Exception ex);

        void HandleException(string message, Exception ex);
    }
}
