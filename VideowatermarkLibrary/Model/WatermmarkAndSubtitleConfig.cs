using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideowatermarkLibrary.Model
{
    public class WatermmarkAndSubtitleConfig : IVideoConfiguration
    {
        public string InputVideoPath { get; set; }
        public string WatermarkfilePath { get; set; }
        public string OutputVideoPath { get; set; }
        public string OverlayPosition { get; set; }
        public string SubtitlePath { get; set; }

        public string toCommandString()
        {
            var ffmpegSubSringPath = SubtitlePath.Replace("\\", "\\\\\\\\").Replace(":", "\\\\:");
            var command = @" -i "+InputVideoPath + " -i " + WatermarkfilePath + " -threads 0 -c:v libx264 -crf 28 -preset veryslow -filter_complex \"[0:v][1:v]overlay=W-w-5:H-h-5,subtitles="+ ffmpegSubSringPath +"[out]\" -map \"[out]\" -map 0:a "+OutputVideoPath;

            return command;
        }
    }
}
