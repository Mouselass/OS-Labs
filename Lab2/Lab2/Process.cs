using System;
using System.Collections.Generic;
using System.Text;

namespace Lab2
{
    public class Process
    {
        public int ProcessId { get; private set; }

        public Process(int processId, bool displayLabel)
        {
            this.ProcessId = processId;
            if (displayLabel)
            {
                Console.WriteLine("ProcessId: " + processId);
            }
        }

        public Thread CreateThread(int threadsSize)
        {
            return new Thread(threadsSize, ProcessId, new Random().Next() % 2 == 1 ? true : false, true);
        }

        public void Start()
        {
            Console.WriteLine("ProcessId: " + ProcessId);
        }

    }
}
