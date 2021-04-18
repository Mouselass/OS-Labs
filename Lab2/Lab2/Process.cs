using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{
    public class Process
    {
        public int ProcessId { get; private set; }

        public List<Thread> Threads { get; private set; }

        public Process(int processId, int n, int timeOfOneIteration)
        {
            ProcessId = processId;
            Threads = new List<Thread>();
            Random rand = new Random();
            Console.WriteLine("\nСоздаем процесс. PID: " + processId + " Количество потоков: " + n);

            for (int i = 0; i < n; i++)
            {
                bool hasIO = rand.Next(0, 2) == 1 ? true : false;
                int threadExecutionTime = rand.Next(10, 30);
                int IOWaitingTime = rand.Next(10, 21);
                int IOWatingCount = rand.Next(1, 3);
                Threads.Add(new Thread(i, processId, hasIO, timeOfOneIteration, threadExecutionTime, IOWaitingTime, IOWatingCount, true));
            }
        }

        public void Start()
        {
            Console.WriteLine("Начинаем процесс. PID: " + ProcessId);
        }

    }
}
