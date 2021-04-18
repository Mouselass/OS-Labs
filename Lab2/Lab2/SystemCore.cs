using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab2
{
    public class SystemCore
    {
        List<Process> Processes;
        Dictionary<int, List<Thread>> Threads;

        private int maxTimeOfThreads = 30;

        private bool ProcessEnd = false;

        public SystemCore(int n)
        {
            Processes = new List<Process>();
            Threads = new Dictionary<int, List<Thread>>();
            Random rand = new Random();
            int timeOfOneIteration = rand.Next(5, 11);
            for (int i = 0; i < n; i++)
            {
                Process process = new Process(i, rand.Next(1, 4), timeOfOneIteration);
                Processes.Add(process);
                Threads.Add(i, process.Threads);
            }
        }

        public int StartPlanProcessWithoutInterrupting()
        {
            var processes = Processes.Select(x => x).ToList();
            var threads = Threads.ToDictionary(x => x.Key, x => x.Value.Select(x => (Thread)x.Clone()).ToList()); 

            Console.WriteLine("\n\nПланирование процессов без прерываний\n\n");

            int fullExecutionTime = 0;

            while (true)
            {
                for (int i = 0; i < processes.Count; i++)
                {

                    processes[i].Start(); 

                    int temp = StartPlanThreadWithoutInterrupting(processes[i].ProcessId, threads); //затраченное на процесс время

                    fullExecutionTime += temp;

                    if (temp == 0)
                    {
                        Console.WriteLine("Удаляем процесс: " + processes[i].ProcessId);
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

        public int StartPlanThreadWithoutInterrupting(int pid, Dictionary<int, List<Thread>> threads)
        {
            int temp = 0;//затраченное на процессы время
            while (true)
            {
                for (int i = 0; i < threads[pid].Count; i++)
                {

                    threads[pid][i].Start();

                    int time = threads[pid][i].StartWithoutInterrupting();
                    temp += time;

                    if (threads[pid][i].hasInputOutput && threads[pid][i].IOWatingCount >= 0)
                    {
                        Console.WriteLine($"Время ожидания потока: {time}, Осталось взаимодейсвий с устройством ввода/вывода: {threads[pid][i].IOWatingCount}");
                        Console.WriteLine($"Затраченное на поток время: {time}");
                        Console.WriteLine($"Затраченное на процесс время: {temp}. (Максимальное время итерации процесса {maxTimeOfThreads})\n");
                        if (temp >= maxTimeOfThreads)
                        {
                            Console.WriteLine("Выхожу из планировщика потоков\n");
                            return temp;
                        }
                        break;
                    }

                    Console.WriteLine($"Затраченное на поток время: {time}");
                    Console.WriteLine($"Затраченное на процесс время: {temp}. (Максимальное время итерации процесса {maxTimeOfThreads})\n");

                    if (threads[pid][i].threadExecutionTime <= 0)
                    {
                        Console.WriteLine("Удаляем поток: " + threads[pid][i].ThreadId);
                        threads[pid].RemoveAt(i);
                        i--;
                    }

                    if (temp >= maxTimeOfThreads)
                    {
                        Console.WriteLine("Выхожу из планировщика потоков\n");
                        return temp;
                    }
                }

                if (threads[pid].Count == 0)
                {
                    return temp;
                }
            }
        }

        public int StartPlanProcessWithInterrupting()
        {
            var processes = Processes.Select(x => x).ToList();
            var threads = Threads.ToDictionary(x => x.Key, x => x.Value.Select(x => (Thread)x.Clone()).ToList()); 

            Console.WriteLine("\n\nПланирование процессов с прерываниями\n\n");

            int fullExecutionTime = 0;

            while (true)
            {
                for (int i = 0; i < processes.Count; i++)
                {
                    processes[i].Start();

                    int temp = StartPlanThreadWithInterrupting(processes[i].ProcessId, threads);//затраченное на процесс время

                    fullExecutionTime += temp;

                    if (ProcessEnd)
                    {
                        Console.WriteLine("Удаляем процесс: " + processes[i].ProcessId);
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


        public int StartPlanThreadWithInterrupting(int pid, Dictionary<int, List<Thread>> threads)
        {
            ProcessEnd = false;
            int temp = 0;//затраченное на процессы время
            while (true)
            {
                for (int i = 0; i < threads[pid].Count; i++)
                {

                    threads[pid][i].Start();

                    int resultOfRunning = threads[pid][i].StartWithInterrupting();

                    if (resultOfRunning == -1)
                    {
                        Console.WriteLine($"Осталось взаимодейсвий с устройством ввода/вывода: {threads[pid][i].IOWatingCount}");
                        Console.WriteLine($"Затраченное на процесс время: {temp}. (Максимальное время итерации процесса {maxTimeOfThreads})\n");
                        return temp;
                    }
                    else if (resultOfRunning >= 0)
                    {
                        Console.WriteLine($"Затраченное на поток время: {resultOfRunning}");
                        temp += resultOfRunning;
                    }

                    if (threads[pid][i].threadExecutionTime <= 0)
                    {
                        Console.WriteLine("Удаляем поток: " + threads[pid][i].ThreadId);
                        threads[pid].RemoveAt(i);
                        i--;
                    }

                    Console.WriteLine($"Затраченное на процесс время: {temp}. (Максимальное время итерации процесса {maxTimeOfThreads})\n");

                    if (temp >= maxTimeOfThreads)
                    {
                        Console.WriteLine("Выхожу из планировщика потоков\n");
                        return temp;
                    }
                }

                if (threads[pid].Count == 0)
                {
                    ProcessEnd = true;
                    return temp;
                }
            }
        }
    }
}
