using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Supra.LittleLogger
{
    public static class Logger
    {
        private static string LogDBConnectionString;
        private static string LogFile;
        private static string App;
        private static string SessionID;
        private static int MaxLogFileSize;
        private static bool Mock;
        public static Severity MaxSeverityThreshold { get; private set; }
        private static readonly string MachineName = Environment.MachineName;
        private static readonly object Lock = new Object(); //Text File Lock

        #region setup
        /// <summary>
        /// Initialize Logger, call before attempting any Logging
        /// </summary>
        public static void Init(string logDBConnString, string logFile, string app, Severity maxSeverityThreshold = Severity.Error, object sessionID = null, int maxLogFileSizeInKB = 5000, bool mock = false)
        {
            LogDBConnectionString = logDBConnString;
            LogFile = logFile;
            App = app;
            MaxSeverityThreshold = maxSeverityThreshold;
            SessionID = sessionID != null ? sessionID.ToString() : app;
            MaxLogFileSize = maxLogFileSizeInKB * 1000;
            if (MaxLogFileSize < 50000) MaxLogFileSize = 50000;
            Mock = mock;
        }
        public static void SetSessionID(object sessionID)
        {
            SessionID = sessionID.ToString();
        }
        #endregion

        #region write method wrappers
        public static void WriteAppStopper(object msg, int? eventId = null, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            Write(Priority.AplicationStopper, Severity.Critical, msg.ToString(), eventId, category, action, sessionID);
        }
        public static void WriteDebug(object msg, Severity? severity = null, int? eventId = null, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            Write(Priority.Debug, severity ?? Severity.Information, msg.ToString(), eventId, category, action, sessionID);
        }
        public static void WriteTrace(object msg, Severity? severity = null, int? eventId = null, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            Write(Priority.Trace, severity ?? Severity.Verbose, msg.ToString(), eventId, category, action, sessionID);
        }
        public static void WriteInformation(object msg, Severity? severity = null, int? eventId = null, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            Write(Priority.Information, severity ?? Severity.Information, msg.ToString(), eventId, category, action, sessionID);
        }
        public static void WriteWarning(object msg, int? eventId = null, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            Write(Priority.Warning, Severity.Warning, msg.ToString(), eventId, category, action, sessionID);
        }
        public static void WriteError(object msg, int? eventId = null, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            Write(Priority.Error, Severity.Error, msg.ToString(), eventId, category, action, sessionID);
        }
        public static void WriteException(Exception e, int? eventId = null, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            WriteError(e.Message, eventId, category, action, sessionID);
        }
        public static void Write(Priority priority, Severity severity, object msg, int? eventId = null, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            LogEntity log = new LogEntity(App, category, action, eventId ?? (int)severity, priority, severity, msg.ToString(), sessionID ?? SessionID);
            Write(new List<LogEntity> { log });
        }
        public static void LogIf(Priority priority, Severity severity, Func<string> f, bool alternativeCondition = false, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            if (alternativeCondition || severity <= MaxSeverityThreshold)
            {
                try
                {
                    LogEntity log = new LogEntity(App, category, action, 0, priority, severity, f(), sessionID ?? SessionID);
                    Write(new List<LogEntity> { log });
                }
                catch (Exception e)
                {
                    WriteException(e);
                }
            }
        }
        public static void Error(Func<string> f, bool alternativeCondition = false, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            LogIf(Priority.Error, Severity.Error, f, alternativeCondition, category, action, sessionID);
        }
        public static void Warning(Func<string> f, bool alternativeCondition = false, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            LogIf(Priority.Warning, Severity.Warning, f, alternativeCondition, category, action, sessionID);
        }
        public static void Info(Func<string> f, bool alternativeCondition = false, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            LogIf(Priority.Information, Severity.Information, f, alternativeCondition, category, action, sessionID);
        }
        public static void Debug(Func<string> f, bool alternativeCondition = false, [CallerFilePath] string category = null, [CallerMemberName] string action = null, string sessionID = null)
        {
            LogIf(Priority.Debug, Severity.Information, f, alternativeCondition, category, action, sessionID);
        }
        #endregion

        #region private methods
        private static void Write(List<LogEntity> logs)
        {
            if (Mock)
            {
                return;
            }

            if (String.IsNullOrEmpty(LogDBConnectionString))
            {
                throw new Exception("Cannot Write to LogDB as Logger.Init method hasn't been called with a valid Connection String");
            }

            //Try to use the database, otherwise fall back to a file
            try
            {
                using (var sql = new SqlConnection(LogDBConnectionString))
                {
                    sql.Open();
                    var cmd = new SqlCommand("WriteLog", sql);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    foreach (LogEntity log in logs)
                    {
                        cmd.Parameters.AddRange(log.GetType().GetProperties().Select(p => new SqlParameter() { ParameterName = "@" + p.Name, Value = p.GetValue(log) }).ToArray());
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
           { 
                logs.Add(new LogEntity(App, "Logger", "Write", 0, Priority.Error, Severity.Error, "Could not log to DB:  " + e, SessionID) { AppDomainName = App });
                WriteLogsToTextFile(logs);
            }
        }
        private static void WriteLogsToTextFile(IEnumerable<LogEntity> logs)
        {
            if(Mock)
            {
                return;
            }

            var logTxt = string.Join("\r\n", logs.Select(l => JsonConvert.SerializeObject(l)));
            try
            {
                lock (Lock)
                {
                    File.AppendAllText(LogFile, logTxt);
                    if (new FileInfo(LogFile).Length>MaxLogFileSize)
                    {
                        var lines = File.ReadAllLines(LogFile);
                        var halfLines = lines.Skip(lines.Length / 2);
                        File.WriteAllLines(LogFile, halfLines);
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Logging has failed for " + App + ".  Could not log to file '" + LogFile + "' :\nError:" + e + "\nLog Data: " + logTxt);
            }
        }
        #endregion
    }
}