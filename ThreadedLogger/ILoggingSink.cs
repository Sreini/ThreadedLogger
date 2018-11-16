using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadedLogger
{
    public interface ILoggingSink
    {
        void LogInformative(string message);
    }
}
