using C969___Scheduler;
using C969_Task.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace C969_Task
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DBHost.StartConnection();
            //Application.Run(new MainScreen());
            Application.Run(new LoginScreen());
            DBHost.CloseConnection();
        }
    }
}
