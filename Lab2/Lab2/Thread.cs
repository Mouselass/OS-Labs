using System;

namespace Lab2
{
    public class Thread
    {
        public int ThreadId { get; private set; }

        public int ProcessId { get; private set; }

        public int ThreadExecutionTime;

        public int threadExecutionTime
        {
            get { if (!hasInputOutput || IOWatingCount <= 0) { return ThreadExecutionTime; } else { return IOWaitingTime; } }
            set { ThreadExecutionTime = value; }
        }

        public int timeOfOneIteration { get; private set; }

        public bool hasInputOutput { get; set; }

        public int IOWaitingTime { get; set; }

        public int IOWatingCount { get; set; }

        public Thread(int threadId, int processId, bool hasInputOutput, int OneIteration, int ExecutionTime, int IOWating, int IOWatingCount, bool print)
        {
            threadExecutionTime = ExecutionTime;
            timeOfOneIteration = OneIteration;
            this.ThreadId = threadId;
            this.ProcessId = processId;
            this.hasInputOutput = hasInputOutput;
            if (hasInputOutput)
            {
                this.IOWaitingTime = IOWating;
                this.IOWatingCount = IOWatingCount;
            }
            if (print)
            {
                Console.WriteLine("\nСоздаем поток. TID: " + threadId + (hasInputOutput ? " Есть ввод/вывод." : " Нет ввода/вывода.")
                    + (hasInputOutput ? " Количество взаимодействий с утройством ввода/вывода: " + IOWatingCount : "")
                    + ".\nВремя выполнения " + threadExecutionTime
                    + ". Время одной итерации " + timeOfOneIteration);
            }
        }

        public void Start()
        {
            Console.WriteLine("Начинаем поток. PID: " + ProcessId + ", TID: " + ThreadId);
            Console.WriteLine("Нужно времени для выполнения: " + threadExecutionTime);
        }

        public int StartWithoutInterrupting()
        {
            if (hasInputOutput && --IOWatingCount >= 0)
            {
                return IOWaitingTime;
            }
            threadExecutionTime -= timeOfOneIteration;
            return timeOfOneIteration;

        }

        public int StartWithInterrupting()
        {
            if (hasInputOutput && --IOWatingCount >= 0)
            {
                return -1;
            }
            else
            {
                threadExecutionTime -= timeOfOneIteration;
                return timeOfOneIteration;
            }
        }

        public object Clone()
        {
            return new Thread(ThreadId, ProcessId, hasInputOutput, timeOfOneIteration, threadExecutionTime, IOWaitingTime, IOWatingCount, false);
        }
    }
}
