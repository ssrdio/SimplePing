using SimpleLsSample.DAL.DAO;
using SimpleLsSample.Interfaces;
using System.Runtime.InteropServices;

namespace SimpleLsSample.Services
{
    public class ResultBl : IResultBl
    {
        private PingerDBContext _dbContext;
        public ResultBl(PingerDBContext pingerDBContext) 
        {
            _dbContext = pingerDBContext;
        }
        public string PingDomain(string domain)
        {
            if (domain == null)
            {
                return "";
            }
            else
            {
                _dbContext.Add(new Ping
                {
                    Domain = domain,
                    DateTime = DateTime.Now
                });
                _dbContext.SaveChanges();
                string shell = "/bin/bash";
                string defaultArgs = $"-c \"ping -c 4 {domain}";
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    shell = "cmd.exe";
                    defaultArgs = $"/c ping {domain}";
                }

                System.Diagnostics.Process process = new System.Diagnostics.Process();
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.StartInfo.FileName = shell;
                process.StartInfo.Arguments = $"{defaultArgs}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();
                string q = "";
                while (!process.HasExited)
                {
                    q += process.StandardOutput.ReadToEnd();
                }
                
                return q;
                
            }
        }

        public virtual string GetIndexViewName()
        {
            return "Index";
        }
    }
}
