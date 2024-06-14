using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Appointment_Tracker
{
    public class Pair
    {
        public Pair() { }

        public Pair(int id, string name)
        {
            ID = id;
            Name = name;
        }

        public string Name { get; set; }
        public int ID { get; set; }
    };
}
