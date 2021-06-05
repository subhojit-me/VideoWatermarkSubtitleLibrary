using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideowatermarkLibrary;

namespace WatermarkServiceExecuter
{
    class Program
    {
        static void Main(string[] args)
        {
            new ServiceExecuter().OnStart();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            Console.WriteLine("Timeout Infinite added");
        }
    }
}
