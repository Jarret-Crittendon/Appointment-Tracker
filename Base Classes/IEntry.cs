using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Tracker.Base_Classes
{
    internal interface IEntry<T>
    {
        void Load(int ID);
        void Update();
    }
}
