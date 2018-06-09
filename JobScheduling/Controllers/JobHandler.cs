using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduling
{
    public class JobHandler
    {
        public Job CreateJob(int id, int createtime, int runtime)
        {
            Job job = new Job();
            if (job == null)
            {
                return null;
            }
            job.id = id;
            job.createtime = createtime;
            job.runtime = runtime;
            job.state = Program.JOBBACKUPSTATE;
            job.turnaroundtime = 0;
            job.weightturnedaroundtime = 0.0;
            job.prev_Job = job.next_Job = job;
            return job;
        }
        public void DestroyJob(Job job)
        {
            IDisposable disposable = job as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }
        public void InitJobQueue(Job queue)
        {
            queue.prev_Job = queue.next_Job = queue;
        }
        public void ClearJobQueue(Job queue)
        {
            Job job;
            while (IsEmptyJobQueue(queue) == Program.FALSE)
            {
                job = FirstJobInQueue(queue);
                RemoveJobFromQueue(job);
                DestroyJob(job);
            }
        }
        public int IsEmptyJobQueue(Job queue)
        {
            if (queue.next_Job == queue)
            {
                return Program.TRUE;
            }
            else
            {
                return Program.FALSE;
            }
        }
        public int SizeofJobQueue(Job queue)
        {
            int n;
            Job job;
            for (n = 0, job = queue.next_Job; job != queue; ++n, job = job.next_Job) ;
            return n;
        }
        public void PrintJob(Job job)
        {
            Console.WriteLine("id={0}, create={1}, run={2}, exit={3}, turnad={4}, weightedturnad={5} ", job.id, job.createtime, job.runtime, job.exittime, job.turnaroundtime, job.weightturnedaroundtime);
        }
        public void PrintJobQueue(Job queue)
        {
            Job job;
            for (job = queue.next_Job; job != queue; job = job.next_Job)
            {
                PrintJob(job);
            }
        }
        public Job FirstJobInQueue(Job queue)
        {
            if (IsEmptyJobQueue(queue) == Program.TRUE)
            {
                return null;
            }
            else
            {
                return queue.next_Job;
            }
        }
        public Job ShortJobInQueue(Job queue)
        {
            Job job, shortjob;
            if (IsEmptyJobQueue(queue) == Program.TRUE)
            {
                return null;
            }
            else
            {
                for (shortjob = queue.next_Job, job = shortjob.next_Job; job != queue; job = job.next_Job)
                {
                    if (job.runtime < shortjob.runtime)
                    {
                        shortjob = job;
                    }
                }
                return shortjob;
            }
        }
        public Job HrnJobInQueue(Job queue)
        {
            Job hrnjob, job;
            double hrn, rn;
            if (IsEmptyJobQueue(queue) == Program.TRUE)
            {
                return null;
            }
            else
            {
                for (hrnjob = queue.next_Job, job = hrnjob.next_Job; job != queue; job = job.next_Job)
                {
                    hrn = (double)(Program.systemtime - hrnjob.createtime) / hrnjob.runtime;
                    rn = (double)(Program.systemtime - job.createtime) / job.runtime;
                    if (rn > hrn)
                    {
                        hrnjob=job;
                    }
                }
                return hrnjob;
            }
        }
        public void RemoveJobFromQueue(Job job)
        {
            job.prev_Job.next_Job = job.next_Job;
            job.next_Job.prev_Job = job.prev_Job;
            job.prev_Job = job.next_Job = job;
        }

        public void AddJobToQueue(Job job,Job queue)
        {
            job.prev_Job = queue.prev_Job;
            job.next_Job = queue;
            queue.prev_Job.next_Job = job;
            queue.prev_Job = job;
        }
    }
}
