using System;
using System.Runtime.CompilerServices;

namespace NgSchoolsBusinessLayer.Services.Contracts
{
    public interface ILoggerService
    {
        void LogErrorToEventLog(Exception ex, [CallerMemberName] string callerMember = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
        void LogErrorToEventLog<T>(Exception ex, T filter, [CallerMemberName] string callerMember = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int callerLineNumber = 0);
    }
}