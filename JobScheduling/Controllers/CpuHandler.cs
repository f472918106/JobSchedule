using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobScheduling
{
    public class CpuHandler
    {
        public void InitCpu(CPU cpu,int policy)
        {
            cpu.state = Program.FREE;
            cpu.policy = policy;
            cpu.runningjob = null;

            cpu.busytime = cpu.freetime = 0;
            cpu.jobnumber = 0;
            cpu.turnaroundtime = 0;
            cpu.weightturnaroundtime = 0.0;
        }
        public void PrintCpu(CPU cpu)
        {
            switch (cpu.policy)
            {
                case Program.FCFS:
                    Console.Write("\nFCFS, ");
                    break;
                case Program.SJF:
                    Console.Write("\nSJF, ");
                    break;
                case Program.HRN:
                    Console.Write("\nHRN, ");
                    break;
                default:
                    Console.Write("\nUnknown, ");
                    break;
            }
            Console.Write("busy={0}, free={1},jobnumber={2}, turnaround={3}, wturnaround={4}", cpu.busytime, cpu.freetime, cpu.jobnumber,cpu.turnaroundtime,cpu.weightturnaroundtime);
        }
    }
}
