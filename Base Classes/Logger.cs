using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Tracker
{
    public class Logger
    {
        public Logger() { }

        string path = "./logfile.txt";

        public void LogSuccess(User user)
        {
            using (StreamWriter log = File.AppendText(path))
            {
                var s = string.Format("{0} (SUCCESS) - [{1}] has logged into the application.", DateTime.Now, user.Username);
                log.WriteLine(s);
                s = string.Format("{0} (ROLE) - [{1}] is a {2}.", DateTime.Now, user.Username, user.Role());
                log.WriteLine(s);

            }          
        }

        public void LogError(string user_name)
        {
            using (StreamWriter log = File.AppendText(path))
            {
                var s = string.Format("{0} (FAIL) - [{1}] failed to log into the application.", DateTime.Now, user_name);
                log.WriteLine(s);
            }
        }
    }
}
