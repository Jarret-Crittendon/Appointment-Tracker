using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969_Task
{
    public class Logger
    {
        public Logger() { }

        string path = "./logfile.txt";

        public void LogSuccess(string user)
        {
            using (StreamWriter log = File.AppendText(path))
            {
                var s = string.Format("{0} (SUCCESS) - [{1}] has logged into the application.", DateTime.Now, user);
                log.WriteLine(s);
            }          
        }

        public void LogError(string user)
        {
            using (StreamWriter log = File.AppendText(path))
            {
                var s = string.Format("{0} (FAIL) - [{1}] failed to log into the application.", DateTime.Now, user);
                log.WriteLine(s);
            }
        }
    }
}
