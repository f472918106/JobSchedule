using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduling
{
    public class Event
    {
        public int time;
        public int type;
        public int jobid;
        public Event prev_Event, next_Event;
    }
}
