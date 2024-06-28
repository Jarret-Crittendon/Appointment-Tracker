using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Tracker
{
    public class User
    {
        public int? ID {  get; set; }
        public string Username {  get; set; }
        public string Password { get; set; }
        public bool Active {  get; set; }
        public DateTime? CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? LastUpdate { get; set; }
        public string LastUpdatedBy { get; set; }

        public virtual string Role()
        {
            return "Normal user";
        }
    }
}
