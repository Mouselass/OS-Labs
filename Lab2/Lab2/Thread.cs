using System;

namespace Lab2
{
    public class Thread
    {
        public int ThreadId { get; private set; }

        public int ProcessId { get; private set; }

        public int ThreadExecutionTime;
        public int _ThreadExecutionTime
        {
            get { 
                if (!HasInputOutput) { return ThreadExecutionTime; }
                else { return IOWaitingTime; } } 
            private set { ThreadExecutionTime = value; }
        }

        public int TimeOfOneIteration { get; private set; }

        private bool HasInputOutput;

        private int IOWaitingTime;

        public Thread(int threadId, int processId, bool hasInputOutput, bool displayLabel)
        {
            _ThreadExecutionTime = (new Random().Next() % 25) + 10;
            TimeOfOneIteration = (new Random().Next() % 8) + 3;
            this.ThreadId = threadId;
            this.ProcessId = processId;
            this.HasInputOutput = hasInputOutput;
            if (hasInputOutput)
            {
                IOWaitingTime = (new Random().Next() % 21) + 10;
            }
            if (displayLabel)
            {
                Console.WriteLine("ThreadId: " + threadId + (hasInputOutput ? " Есть IO" : " Нет IO") + ";ExecutionTime:" + _ThreadExecutionTime + ";IterationTime:" + TimeOfOneIteration);
            }
        }

        public void Start()
        {
            Console.WriteLine("ProcessId: " + ProcessId + ",ThreadId: " + ThreadId);
            Console.WriteLine("ExecutionTime: " + (HasInputOutput ? IOWaitingTime : _ThreadExecutionTime));
        }

        public int RunWithoutInterrupting()
        {
            if (HasInputOutput)
            {
                int spentTime = IOWaitingTime;
                IOWaitingTime = 0;
                return spentTime;
            }

            else
            {
                _ThreadExecutionTime -= TimeOfOneIteration;
                return TimeOfOneIteration;
            }
        }

        public int RunWithInterrupting()
        {
            if (HasInputOutput)
            {

                if (IOWaitingTime > TimeOfOneIteration)
                {
                    IOWaitingTime -= TimeOfOneIteration;
                    return -1;
                }
                else
                {
                    int spentedTime = IOWaitingTime;
                    IOWaitingTime = 0;
                    return spentedTime;
                }

            }

            else
            {
                _ThreadExecutionTime -= TimeOfOneIteration;
                return TimeOfOneIteration;
            }
        }

        public void SubtractIOWaitingTime(int time)
        {
            IOWaitingTime -= time;
        }

        public object Clone()
        {
            return new Thread(ThreadId, ProcessId, HasInputOutput, true);
        }
    }
}
