using System;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemCore systemCore = new SystemCore(new Random().Next() % 3 + 1);

            int TimeWithoutInterrapting = systemCore.StartPlannigProcessWithoutInterrupting();
            int TimeWithInterrapting = systemCore.StartPlannigProcessWithInterrupting();

            Console.WriteLine("\n\nЗатраченное время без прерываний: " + TimeWithoutInterrapting + "\n\n");
            Console.WriteLine("\n\nЗатраченное время c прерываниями: " + TimeWithInterrapting + "\n\n");
        }
    }
}
