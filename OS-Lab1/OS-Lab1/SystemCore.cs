using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Lab1
{
    public class SystemCore
    {
        private List<Process> processes = new List<Process>();

        FormMain formMain;

        public SystemCore(FormMain formMain) {
            this.formMain = formMain;
        }

        public void CreateProcesses()
        {
            processes.Add(new Process(processes.Count(), formMain));
        }

        public void StartPlanning()
        {
            int temp = 0;
            while (true)
            {
                for (int i = 0; i < processes.Count(); i++)
                {
                    processes[i].StartPlanning();
                    temp += processes[i].IterationTime;
                    processes[i].SubtractProcessIterationTime();

                    if (processes[i].ProcessTime < 0)
                    {
                        processes.Remove(processes[i]);
                        i--;
                    }

                    int maxProcessTime = 300;

                    if (temp > maxProcessTime)
                    {
                        return;
                    }
                }

                if (processes.Count() == 0)
                {
                    return;
                }
            }
        }
    }
}
