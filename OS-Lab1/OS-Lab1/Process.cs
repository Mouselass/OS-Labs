using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OS_Lab1
{
    public class Process
    {
        private List<Thread> threads = new List<Thread>();
        public int ProcessId { get; private set; }

        public int IterationTime { get; private set; }

        public int ProcessTime { get; private set; }

        FormMain formMain;

        public Process(int ProcessId, FormMain formMain) {
            this.ProcessId = ProcessId;
            this.formMain = formMain;

            Random random = new Random();
            for (int i = 0; i < random.Next()% 3 + 1; i++)
            {
                this.CreateThread();
                ProcessTime += threads[i].ThreadTime;
            }
        }

        public void SubtractProcessIterationTime()
        {
            ProcessTime -= IterationTime;
        }

        public void CreateThread()
        {
            threads.Add(new Thread(threads.Count(), ProcessId, formMain));
        }

        public void StartPlanning()
        {
            int temp = 0;
            while (true)
            {
                for (int i = 0; i < threads.Count(); i++)
                {
                    threads[i].Start();
                    temp += threads[i].IterationTime;
                    threads[i].SubtractThreadIterationTime();

                    if (threads[i].ThreadTime < 0)
                    {
                        threads.Remove(threads[i]);
                        i--;
                    }

                    int maxThreadTime = 30;

                    if (temp > maxThreadTime)
                    {
                        IterationTime = temp;
                        return;
                    }
                }

                if (threads.Count() == 0)
                {
                    IterationTime = temp;
                    return;
                }
            }
        }

    }
}
