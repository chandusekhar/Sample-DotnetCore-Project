namespace Supra.LittleLogger
{
    public enum Priority
    {
        AplicationStopper = 0,
        Error = 1,
        Warning = 2,
        Information = 3,
        Trace = 4,
        Debug = 5
    }

    public enum Severity
    {
        Critical = 1, // Fatal error or application crash.       
        Error = 2, // Recoverable error.        
        Warning = 4, // Noncritical problem.        
        Information = 8, // Informational message.       
        Verbose = 16, // Debugging trace.        
        Start = 256, // Starting of a logical operation.        
        Stop = 512, // Stopping of a logical operation.       
        Suspend = 1024, // Suspension of a logical operation.        
        Resume = 2048, // Resumption of a logical operation.       
        Transfer = 4096 // Changing of correlation identity.
    }
}
