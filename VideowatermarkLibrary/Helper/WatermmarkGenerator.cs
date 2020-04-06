using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VideowatermarkLibrary.Model;
using VideoWatermarkService.Model;

namespace VideowatermarkLibrary.helpers
{
    public class WatermmarkGenerator : AbstractVideoGenerator
    {
        public WatermmarkGenerator(WatermarkConfiguration ParamConfig)
        {
            this.config = ParamConfig;
        }
    }
}