using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadedLogger
{
    public interface ILogger : IDisposable
    {
        void LogInfo(string message);

        void LogWarning(string message);

        void LogWarning(string message, Exception ex);

        void HandleException(string message, Exception ex);
    }
}
