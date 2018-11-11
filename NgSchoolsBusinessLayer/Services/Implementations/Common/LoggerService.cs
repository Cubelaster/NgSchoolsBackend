using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NgSchoolsBusinessLayer.Models.Common;
using NgSchoolsBusinessLayer.Services.Contracts;
using System;
using System.Runtime.CompilerServices;

namespace NgSchoolsBusinessLayer.Services.Implementations.Common
{
    public class LoggerService : ILoggerService
    {
        private readonly ILogger<LoggerService> logger;
        public LoggerService(ILogger<LoggerService> logger)
        {
            this.logger = logger;
        }

        public void LogErrorToEventLog<T>(Exception ex, T filter, [CallerMemberName] string callerMember = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            EventLogErrorParams parameters = new EventLogErrorParams
            {
                CallerMember = callerMember,
                LineNumber = callerLineNumber,
                Source = sourceFilePath,
                Parameters = JsonConvert.SerializeObject(filter),
                Exception = ex
            };
            LogParams(parameters);
        }

        public void LogErrorToEventLog(Exception ex, [CallerMemberName] string callerMember = "",
            [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
        {
            EventLogErrorParams parameters = new EventLogErrorParams
            {
                CallerMember = callerMember,
                LineNumber = callerLineNumber,
                Source = sourceFilePath,
                Parameters = "Method has no entry parameters",
                Exception = ex
            };
            LogParams(parameters);
        }

        private void LogParams(EventLogErrorParams errorParams)
        {
            logger.LogError(errorParams.Exception,
                    $"Error on: {Environment.NewLine}" +
                    $"Member: {errorParams.CallerMember}{Environment.NewLine}" +
                    $"Source: {errorParams.Source.Substring(errorParams.Source.IndexOf("NgSchools"))}{Environment.NewLine}" +
                    $"Line Number: {errorParams.LineNumber}{Environment.NewLine}" +
                    $"Parameters used:{Environment.NewLine}" +
                    $"{errorParams.Parameters}");
        }
    }
}
