using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Logger;

public interface ILoggerManager
{
    public void LogTrace(string message);
    public void LogDebug(string message);
    public void LogInfo(string message);
    public void LogWarning(string message);
    public void LogError(string message);
    public void LogFatal(string message);
    public void Log(string message, LogLevelEnum level);
}


