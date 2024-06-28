using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Tracker.Base_Classes
{
    internal class TechSupport : User
    {
        int EmployeeId { get; set; }
        string role = "Tech Support";
    }
}
