using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduling
{
    public class Job
    {
        public int id;
        public int state;
        public int createtime;
        public int runtime;
        public int exittime;

        public int turnaroundtime;
        public double weightturnedaroundtime;

        public Job prev_Job;
        public Job next_Job;
    }
}
