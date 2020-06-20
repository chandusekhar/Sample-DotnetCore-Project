using System.Runtime.ConstrainedExecution;

namespace CoreAccessControl.Domain.Configuration
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public string Secret { get; set; }
        public RemoteServer RemoteServer { get; set; }
        public EMailSettings MailSettings { get; set; }
        public string ContentRootPath { get; set; }
        public string Domain { get; set; }
        public int Limit { get; set; }
        public Cert Cert { get; set; }
    }

    public class RemoteServer
    {
        public string BasePath { get; set; } = "https://dkintapi.keytest.net";
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class Cert
    {
        public string Path { get; set; }
        public string Password { get; set; }
    }
}
