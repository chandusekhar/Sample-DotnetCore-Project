using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Supra.LittleLogger
{
    internal class LogEntity
    {
        public LogEntity()
        {
            System.Threading.Thread currentThread = System.Threading.Thread.CurrentThread;
            System.Diagnostics.Process currentProcess = System.Diagnostics.Process.GetCurrentProcess();

            Timestamp = DateTime.UtcNow;
            MachineName = Environment.MachineName;
            ProcessID = currentProcess.Id;
            ProcessName = currentProcess.ProcessName;
            Win32ThreadId = currentThread.ManagedThreadId;
        }

        public LogEntity(string app, string category, string action, int eventId, Priority priority, Severity severity, string message, string sessionID) : this()
        {          
            EventID = eventId;
            Priority = priority;
            SeverityEnum = severity;
            Severity = SeverityEnum.ToString();
            Title = Priority.ToString();
            AppDomainName = app;
            ThreadName = category + " " + action;
            Message = message;
            SessionID = sessionID;
            FormattedMessage = message; 
        }

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //The database generates a value when a row is inserted.
        public Int64 LogID { get; }
        public int EventID { get; set; }
        [MaxLength(256)]
        public string Title { get; set; }
        public Priority Priority { get; set; }
        internal Severity SeverityEnum { get; set; }
        [MaxLength(32)]
        public string Severity { get; set; }
        [MaxLength(256)]
        public DateTime Timestamp { get; set; }
        [MaxLength(32)]
        public string MachineName { get; set; }
        [MaxLength(512)]
        public string AppDomainName { get; set; }
        public int ProcessID { get; }
        [MaxLength(512)]
        public string ProcessName { get; }
        [MaxLength(512)]
        public string ThreadName { get; }
        public int Win32ThreadId { get; }
        [MaxLength(1500)]
        public string Message { get; set; }
        [MaxLength(50)]
        public string SessionID { get; }
        public string FormattedMessage { get; set; }
    }
}
