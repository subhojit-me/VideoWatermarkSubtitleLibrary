using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideowatermarkLibrary.Model;

namespace VideowatermarkLibrary.Helper
{
    public class WatermarkAndSubttleGenerator : AbstractVideoGenerator
    {
        public WatermarkAndSubttleGenerator(WatermmarkAndSubtitleConfig paramConfig)
        {
            this.config = paramConfig;
        }
    }
}
