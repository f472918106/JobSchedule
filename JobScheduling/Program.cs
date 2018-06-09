using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduling
{
    class Program
    {
        public static int systemtime=0;
        public static int jobid = 1;
        public static Random random = new Random();
        public static int rand = random.Next(0, 500);
        public static int NEXTJOBCREATETIMEINTERVAL = (rand % MAXJOBCREATETIMEINTERVAL + 1);
        public static int NEXTJOBRUNTIME = (rand % MAXJOBRUNTIME + 1);
        public const int MAXJOBCREATETIMEINTERVAL=50;
        public const int MAXJOBRUNTIME = 100;
        public const int JOBBACKUPSTATE = 1;
        public const int JOBRUNNINGSTATE = 2;
        public const int JOBEXITSTATE = 3;
        public const int BUSY = 1;
        public const int FREE = 2;
        public const int FCFS = 1;
        public const int SJF = 2;
        public const int HRN = 3;
        public const int JOBCREATEEVENT = 1;
        public const int JOBSCHEDULEEVENT = 2;
        public const int JOBEXITEVENT = 3;
        public const int FALSE = 0;
        public const int TRUE = 1;
        public static Job jobqueue=new Job(), exitjobqueue=new Job();
        public static Event eventqueue=new Event(), exiteventqueue=new Event();
        public static CPU cpu=new CPU();
        public static EventHandler eventhandler = new EventHandler();
        public static JobHandler jobhandler = new JobHandler();
        public static CpuHandler cpuhandler = new CpuHandler();
        public static Handle handle = new Handle();
             
        static void Main(string[] args)
        {
            int i;
            Event e;
            Job job;
            int jobnumber, policy, createtime, turnaroundtime;
            double weightturnaroundtime;
            string[] inputStr = new string[2];
            Console.WriteLine("Please input jobamount!");
            inputStr = (Console.ReadLine()).Split(new char[1] { ' ' });
            jobnumber = Convert.ToInt32(inputStr[0]);
            if (inputStr[1] == "SJF")
            {
                policy = SJF;
            }
            else if (inputStr[1] == "HRN")
            {
                policy = HRN;
            }
            else
            {
                policy = FCFS;
            }
            eventhandler.InitEventQueue(eventqueue);
            eventhandler.InitEventQueue(exiteventqueue);

            jobhandler.InitJobQueue(jobqueue);
            jobhandler.InitJobQueue(exitjobqueue);

            cpuhandler.InitCpu(cpu, policy);

            createtime = 0;
            for (i= 0;i < jobnumber; ++i)
            {
                createtime += NEXTJOBCREATETIMEINTERVAL;
                e = eventhandler.CreateEvent(createtime, JOBCREATEEVENT);
                eventhandler.AddEventToQueue(e, eventqueue);
            }

            while (eventhandler.IsEmptyEventQueue(eventqueue) == FALSE)
            {
                e = eventhandler.FirstEventInQueue(eventqueue);
                eventhandler.RemoveEventFromQueue(e);
                systemtime = e.time;

                switch (e.type)
                {
                    case JOBCREATEEVENT:
                        handle.JobCreateEventHandle(e);
                        break;
                    case JOBSCHEDULEEVENT:
                        handle.JobScheduleEventHandle(e);
                        break;
                    case JOBEXITEVENT:
                        handle.JobExitEventHandle(e);
                        break;
                    default:
                        break;
                }
                eventhandler.AddEventToQueue(e, exiteventqueue);
            }

            Console.Write("\nEVENTQUEUE:");
            eventhandler.PrintEventQueue(exiteventqueue);

            Console.Write("\nJOBQUEUE:");
            jobhandler.PrintJobQueue(exitjobqueue);

            for (turnaroundtime = 0, weightturnaroundtime = 0.0, job = jobhandler.FirstJobInQueue(exitjobqueue); job != exitjobqueue; job = job.next_Job)
            {
                turnaroundtime += job.turnaroundtime;
                weightturnaroundtime += job.weightturnedaroundtime;
            }
            cpu.turnaroundtime = turnaroundtime / cpu.jobnumber;
            cpu.weightturnaroundtime = weightturnaroundtime / cpu.jobnumber;

            Console.Write("\nCPU:");
            cpu.freetime = systemtime - cpu.busytime;
            cpuhandler.PrintCpu(cpu);

            Console.WriteLine();

            eventhandler.ClearEventQueue(eventqueue);
            eventhandler.ClearEventQueue(exiteventqueue);
            jobhandler.ClearJobQueue(jobqueue);
            jobhandler.ClearJobQueue(exitjobqueue);
            Console.Read();
        }
    }
}
