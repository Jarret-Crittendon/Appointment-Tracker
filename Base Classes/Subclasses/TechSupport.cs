using Org.BouncyCastle.Bcpg.Sig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Tracker.Base_Classes.Subclasses
{
    internal class TechSupport : User
    {
        public TechSupport(User user)
        {
            this.ID = user.ID;
            this.Username = user.Username;
            this.Password = user.Password;
            this.CreateDate = user.CreateDate;
            this.LastUpdate = user.LastUpdate;
            this.CreatedBy = user.CreatedBy;
            this.LastUpdatedBy = user.LastUpdatedBy;
            this.Active = user.Active;
        }

        public override string Role()
        {
            return "Technician-level user";
        }
    }
}
