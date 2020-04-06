using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoWatermarkService.Model
{
    public class VIDEO_WATERMARK_DTO
    {
        public string UDTUNIQUEROW { get; set; }
        public string FILE_NAME { get; set; }
        public DateTime DT_STAMP { get; set; }
        public DateTime SRV_DT_STAMP { get; set; }
        public bool IS_PROCESSED { get; set; }
        public bool IS_PROCESSING { get; set; }
        public DateTime? CONVERTED_DT_STAMP { get; set; }
        public string SUBTITLE_TEXT { get; set; }
        public string ERROR_MESSAGE { get; set; }


        public static List<VIDEO_WATERMARK_DTO> ToList(DataTable dt)
        {
            return dt.AsEnumerable().Select(row =>
            {
                return new VIDEO_WATERMARK_DTO()
                {
                    UDTUNIQUEROW = row.Field<Guid>("UDTUNIQUEROW").ToString(),
                    FILE_NAME = row.Field<string>("FILE_NAME"),
                    DT_STAMP = row.Field<DateTime>("DT_STAMP"),
                    SRV_DT_STAMP = row.Field<DateTime>("SRV_DT_STAMP"),
                    IS_PROCESSED = row.Field<bool>("IS_PROCESSED"),
                    IS_PROCESSING = row.Field<bool>("IS_PROCESSING"),
                    CONVERTED_DT_STAMP = row.Field<DateTime?>("CONVERTED_DT_STAMP"),
                    SUBTITLE_TEXT = row.Field<string>("SUBTITLE_TEXT"),
                    ERROR_MESSAGE = row.Field<string>("ERROR_MESSAGE")
                };
            }).ToList();
        }
    }
}
