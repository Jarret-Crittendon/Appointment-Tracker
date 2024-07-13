using Appointment_Tracker;
using Appointment_Tracker.Database;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Appointment_Tracker
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
