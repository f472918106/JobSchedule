using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduling
{
    public class CPU
    {
        public int state;
        public int policy;
        public Job runningjob;

        public int busytime;
        public int freetime;
        public int jobnumber;
        public int turnaroundtime;
        public double weightturnaroundtime;
    }
}
