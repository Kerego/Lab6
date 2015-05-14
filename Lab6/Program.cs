using System;
using System.Collections.Generic;

namespace Lab6
{
    class Program
    {
        static void Main(string[] args)
        {
            Astar();
        }

        static void Astar()
        {

            dalbaioji design = new dalbaioji();
            design.jora(() => Console.Write("jora"));
            design.jora(() => Console.WriteLine(" cardan"));
            Console.ReadKey();
        }
    }

    internal class dalbaioji
    {
        public void jora(Action action)  
        {
            action.Invoke();
        }
        public string dibil;
    }
}
