using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    public class SystemCore
    {
        List<Process> Processes;
        Dictionary<int, List<Thread>> Threads;

        private int MaxTimeOfThreads = 50;

        List<Thread> BlockThreads = new List<Thread>();

        public SystemCore(int x)
        {
            Processes = new List<Process>();
            Threads = new Dictionary<int, List<Thread>>();

            for (int i = 0; i < x; i++)
            {
                Processes.Add(new Process(Processes.Count, true));
                CreateThreads(Processes.Count - 1);
            }
        }

        public void CreateThreads(int pid)
        {
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < new Random().Next() % 2 + 1; i++)
            {
                threads.Add(Processes[Processes.Count - 1].CreateThread(threads.Count));
            }
            Threads.Add(pid, threads);
        }

        public int StartPlannigProcessWithoutInterrupting()
        {
            var processes = Processes.Select(x => x).ToList();
            var threads = Threads.ToDictionary(x => x.Key, x => x.Value.Select(x => (Thread)x.Clone()).ToList());

            Console.WriteLine("\nПланирование процессов без прерываний\n");

            int fullExecutionTime = 0;

            while (true)
            {
                for (int i = 0; i < processes.Count; i++)
                {

                    processes[i].Start();

                    int temp = StartPlannigThreadWithoutInterrupting(processes[i].ProcessId, threads);

                    fullExecutionTime += temp;

                    if (temp == 0)
                    {
                        processes.RemoveAt(i); 
                        break;
                    }
                }

                if (processes.Count == 0)
                {
                    return fullExecutionTime;
                }
            }
        }

        public int StartPlannigThreadWithoutInterrupting(int pid, Dictionary<int, List<Thread>> threads)
        {

            int temp = 0;
            while (true)
            {
                for (int i = 0; i < threads[pid].Count; i++)
                {

                    threads[pid][i].Start();

                    temp += threads[pid][i].RunWithoutInterrupting();

                    if (threads[pid][i]._ThreadExecutionTime <= 0)
                    {
                        threads[pid].RemoveAt(i);
                        i--;
                    }

                    Console.WriteLine($"Затраченное на процесс время:{temp}. (Максимальное время итерации процесса {MaxTimeOfThreads})\n");

                    if (temp >= MaxTimeOfThreads)
                    {
                        return temp;
                    }
                }

                if (threads[pid].Count == 0)
                {
                    return temp;
                }
            }
        }

        public int StartPlannigProcessWithInterrupting()
        {
            var processes = Processes.Select(x => x).ToList();
            var threads = Threads.ToDictionary(x => x.Key, x => x.Value.Select(x => (Thread)x.Clone()).ToList());

            Console.WriteLine("\nПланирование процессов с прерываниями\n");

            int fullExecutionTime = 0;

            while (true)
            {
                for (int i = 0; i < processes.Count; i++)
                {
                    processes[i].Start();

                    int temp = StartPlannigThreadWithInterrupting(processes[i].ProcessId, threads);

                    fullExecutionTime += temp;

                    if (temp == 0)
                    {
                        processes.RemoveAt(i);
                        break;
                    }
                }

                if (processes.Count == 0)
                {
                    return fullExecutionTime;
                }
            }
        }

        public int StartPlannigThreadWithInterrupting(int pid, Dictionary<int, List<Thread>> threads)
        {

            int temp = 0;
            while (true)
            {
                for (int i = 0; i < threads[pid].Count; i++)
                {
                    if (BlockThreads.Contains(threads[pid][i]))
                    {
                        if (threads[pid][i]._ThreadExecutionTime <= 0)
                        {
                            BlockThreads.Remove(threads[pid][i]);
                            threads[pid].RemoveAt(i);
                            i--;
                        }
                        continue;
                    }

                    threads[pid][i].Start(); 

                    int resultOfRunning = threads[pid][i].RunWithInterrupting(); 

                    foreach (Thread blockedThread in BlockThreads)
                    {
                        blockedThread.SubtractIOWaitingTime(resultOfRunning);
                    }

                    if (resultOfRunning == -1)
                    {
                        temp += threads[pid][i].TimeOfOneIteration;
                        BlockThreads.Add(threads[pid][i]); 
                    }
                    else if (resultOfRunning >= 0)
                    {
                        temp += resultOfRunning;
                    }

                    if (threads[pid][i]._ThreadExecutionTime <= 0)
                    {
                        threads[pid].RemoveAt(i);
                        i--;
                    }

                    Console.WriteLine($"Затраченное на процесс время:{temp}. (Максимальное время итерации процесса {MaxTimeOfThreads})\n");

                    if (temp >= MaxTimeOfThreads)
                    {
                        return temp;
                    }
                }

                bool existThreadWithoutBlocking = false;

                for (int i = 0; i < threads[pid].Count; i++)
                {
                    if (!BlockThreads.Contains(threads[pid][i]))
                    {
                        existThreadWithoutBlocking = true;
                        break;
                    }
                }

                if (threads[pid].Count == 0 || !existThreadWithoutBlocking)
                {
                    return temp;
                }
            }

        }
    }
}
