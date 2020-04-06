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

        public void OnStart()
        {
            timer.Elapsed += new System.Timers.ElapsedEventHandler(OnElapsedTime);
            var durationInMinutes = int.Parse(ConfigurationManager.AppSettings["serviceRunDurationMinutes"]);
            timer.Interval = durationInMinutes * 60000; // 1000 ms => 1 second
            timer.Enabled = true;
        }

        private void OnElapsedTime(object sender, System.Timers.ElapsedEventArgs e)
        {
            OperationInitilizerService.InitilizeOperation();
        }

        public void OnStop()
        {
            timer.Enabled = false;
        }
    }
}
