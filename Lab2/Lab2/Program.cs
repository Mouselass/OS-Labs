using System;

namespace Lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemCore systemCore = new SystemCore(new Random().Next(2, 4));

            int TimeWithoutInterrapting = systemCore.StartPlanProcessWithoutInterrupting();
            int TimeWithInterrapting = systemCore.StartPlanProcessWithInterrupting();

            Console.WriteLine("\n\nЗатраченное время выполнения планировщика без прерываний: " + TimeWithoutInterrapting);
            Console.WriteLine("\n\nЗатраченное время выполнения планировщика c прерываниями: " + TimeWithInterrapting);
        }
    }
}
