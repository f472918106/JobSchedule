using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduling
{
    public class Handle
    {
        public void JobCreateEventHandle(Event _event)
        {
            Job job;
            Event e;
            if((job=Program.jobhandler.CreateJob((Program.jobid++),Program.systemtime,Program.NEXTJOBRUNTIME))== null)
            {
                return;
            }
            Program.jobhandler.AddJobToQueue(job, Program.jobqueue);
            _event.jobid = job.id;
            if (Program.cpu.state == Program.FREE)
            {
                e = Program.eventhandler.CreateEvent(Program.systemtime, Program.JOBSCHEDULEEVENT);
                Program.eventhandler.FrontAddEventToQueue(e, Program.eventqueue);
            }
            return;
        }
        public void JobScheduleEventHandle(Event _event)
        {
            Job job;
            Event e;
            if (Program.jobhandler.IsEmptyJobQueue(Program.jobqueue) == Program.TRUE)
            {
                return;
            }
            switch (Program.cpu.policy)
            {
                case Program.FCFS:
                    job = Program.jobhandler.FirstJobInQueue(Program.jobqueue);
                    break;
                case Program.SJF:
                    job = Program.jobhandler.ShortJobInQueue(Program.jobqueue);
                    break;
                case Program.HRN:
                    job = Program.jobhandler.HrnJobInQueue(Program.jobqueue);
                    break;
                default:
                    job = Program.jobhandler.FirstJobInQueue(Program.jobqueue);
                    break;
            }
            Program.jobhandler.RemoveJobFromQueue(job);
            _event.jobid = job.id;
            job.state = Program.JOBRUNNINGSTATE;
            Program.cpu.runningjob = job;
            Program.cpu.state = Program.BUSY;
            e = Program.eventhandler.CreateEvent(Program.systemtime + job.runtime, Program.JOBEXITEVENT);
            e.jobid = job.id;
            Program.eventhandler.AddEventToQueue(e, Program.eventqueue);
            return;
        }
        public void JobExitEventHandle(Event _event)
        {
            Job job;
            Event e;
            job = Program.cpu.runningjob;
            job.state = Program.JOBEXITSTATE;
            job.exittime = Program.systemtime;
            job.turnaroundtime = job.exittime - job.createtime;
            job.weightturnedaroundtime = (double)job.turnaroundtime / job.runtime;
            Program.cpu.state = Program.FREE;
            Program.cpu.busytime += job.runtime;
            Program.cpu.jobnumber++;
            Program.jobhandler.AddJobToQueue(job, Program.exitjobqueue);
            if (Program.jobhandler.IsEmptyJobQueue(Program.jobqueue) == Program.FALSE)
            {
                e = Program.eventhandler.CreateEvent(Program.systemtime, Program.JOBSCHEDULEEVENT);
                Program.eventhandler.FrontAddEventToQueue(e, Program.eventqueue);
            }
            return;
        }
    }
}
