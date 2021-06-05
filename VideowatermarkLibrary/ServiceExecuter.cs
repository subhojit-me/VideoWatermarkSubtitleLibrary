using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideowatermarkLibrary.Helper;

namespace VideowatermarkLibrary
{
    public class ServiceExecuter
    {
        private System.Timers.Timer timer = new System.Timers.Timer() { AutoReset = true };


        /// <summary>
        /// Starts the service   
        /// </summary>
        /// <exception cref="Exception">
        /// throws Exception If occoures any
        /// </exception>
        public void OnStart()
        {
            try
            {
                timer.Elapsed += new System.Timers.ElapsedEventHandler(OnElapsedTime);
                var durationInMinutes = int.Parse(ConfigurationManager.AppSettings["serviceRunDurationMinutes"]);
                timer.Interval = durationInMinutes * 60000; // 1000 ms => 1 second
                timer.Enabled = true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void OnElapsedTime(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {

                OperationInitilizerService.InitilizeOperation();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in OnElapsedTime= {0}", ex.Message);
            }
        }

        public void OnStop()
        {
            timer.Enabled = false;
        }
    }
}
