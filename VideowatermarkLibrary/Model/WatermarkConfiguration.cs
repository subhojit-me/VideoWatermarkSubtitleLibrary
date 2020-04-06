using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideowatermarkLibrary.Model;

namespace VideoWatermarkService.Model
{
    public class WatermarkConfiguration : IVideoConfiguration
    {
        public string InputVideoPath { get; set; }
        public string WatermarkfilePath { get; set; }
        public string OutputVideoPath { get; set; }
        public string OverlayPosition { get; set; }

        public string toCommandString()
        {
            var cmd = " -i " + InputVideoPath + " -i " + WatermarkfilePath + " -filter_complex \"overlay=" + OverlayPosition + "\" " + OutputVideoPath;
            return cmd;
        }
    }
}
