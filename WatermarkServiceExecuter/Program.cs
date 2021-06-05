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
            try
            {
                new ServiceExecuter().OnStart();
                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
            }
            catch (Exception e)
            {
                Console.Write("Some Error Occoured in main while starting Service= {0}", e.Message);
            }
        }
    }
}
