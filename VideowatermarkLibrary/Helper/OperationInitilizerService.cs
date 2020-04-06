using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideowatermarkLibrary.helpers;
using VideowatermarkLibrary.Model;
using VideoWatermarkService.Model;
using WMPLib;


namespace VideowatermarkLibrary.Helper
{
    public class OperationInitilizerService
    {
        private static WaterMarkGeneratorDbHelper watermarkRepository = new WaterMarkGeneratorDbHelper();

        private static string VideoContainerFolderLocation()
        {
            return Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\VideoContainer";
        }

        private static string GetWatermarkFilenameWithLocation(){
            var watermarkFileName = ConfigurationManager.AppSettings["watermarkFileName"];
            var waterMarkFileLocation = VideoContainerFolderLocation() + "\\WatermarkFile\\" + watermarkFileName;
            return waterMarkFileLocation;
        }

        private static string GetWatermarkPosition()
        {
            var watermarkPosition = ConfigurationManager.AppSettings["watermarkPosition"];
            return watermarkPosition == "center" ? "(W-w)/2:(H-h)/2" :
                watermarkPosition == "top-left" ? "5:5" :
                watermarkPosition == "bottom-left" ? "5:H-h-5" :
                watermarkPosition == "top-right" ? "W-w-5:5"
                : "W-w-5:H-h-5";    // default is bottom-right
        }

        public static void InitilizeOperation()
        {
            try
            {
                var nonConvertedDatas = watermarkRepository.GetNonConvertedDatas(true);
                if (!nonConvertedDatas.IsSuccess)
                {
                    // If exception occoured then needed  to log the error Message.
                    if (!string.IsNullOrWhiteSpace(nonConvertedDatas.ErrorMessage))
                        throw new Exception(nonConvertedDatas.ErrorMessage);

                    // Exception already handled above, due to no Data not available no operation is required now.
                    if ((nonConvertedDatas.Data as List<VIDEO_WATERMARK_DTO>).Count == 0)
                        return;
                }
                var watermarkOperationStatus = new List<OperationStatus>();
                (nonConvertedDatas.Data as List<VIDEO_WATERMARK_DTO>).ForEach(async data =>
                {
                    var curInputFieLocation = VideoContainerFolderLocation() + "\\Videos\\" + data.FILE_NAME;
                    var curOutputFieLocation = VideoContainerFolderLocation() + "\\Videos\\converting_" + data.FILE_NAME;

                    #region Watermark 
                    //var videoConfig = new WatermarkConfiguration()
                    //{
                    //    InputVideoPath = curInputFieLocation,
                    //    OutputVideoPath = curOutputFieLocation,
                    //    WatermarkfilePath = GetWatermarkFilenameWithLocation(),
                    //    OverlayPosition = GetWatermarkPosition()
                    //};
                    //var watermakAddStatus = new WatermmarkGenerator(videoConfig).GenerateVideo();
                    #endregion

                    using (var subtitleHelper = new SubtitleHelper(curInputFieLocation, data.SUBTITLE_TEXT, data.FILE_NAME))
                    {
                        var videoConfig = new WatermmarkAndSubtitleConfig()
                        {
                            InputVideoPath = curInputFieLocation,
                            OutputVideoPath = curOutputFieLocation,
                            WatermarkfilePath = GetWatermarkFilenameWithLocation(),
                            OverlayPosition = GetWatermarkPosition(),
                            SubtitlePath = subtitleHelper.CreateSubtitleFile()
                        };

                        //var watermarkAddStatus = new WatermarkAndSubttleGenerator(videoConfig).GenerateVideo();
                        var watermarkAddStatus = await Task.Run(() => new WatermarkAndSubttleGenerator(videoConfig).GenerateVideo());
                        data.IS_PROCESSING = false;
                        subtitleHelper.DeleteCreatedSubtitleFile();
                        if (watermarkAddStatus.IsSuccess)
                        {
                            data.CONVERTED_DT_STAMP = DateTime.Now;
                            data.IS_PROCESSED = true;
                            data.ERROR_MESSAGE = null;
                            File.Delete(curInputFieLocation);
                            File.Move(curOutputFieLocation, curInputFieLocation);
                        }
                        else
                        {
                            data.ERROR_MESSAGE = watermarkAddStatus.ErrorMessage;
                            File.Delete(curOutputFieLocation);
                            data.IS_PROCESSED = false;
                        }
                        watermarkRepository.UpdateWatermarkProcessingDetails(data);
                        watermarkOperationStatus.Add(watermarkAddStatus);
                    }
                });
            }
            catch (Exception ex)
            {
                /// application specific exceptions will be handled here.
                var errorMessages = new string[]
                {
                    $"Error occourence Time = {DateTime.Now.ToString("yyyy-MM-dd:hh-mm")}",
                    ex.Message,
                    "----------------------------------------------------------------------------"
                };
                var errorFileLocation = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName+"\\applicationErrors.txt";
                if (!File.Exists(errorFileLocation))
                {
                    File.Create(errorFileLocation).Close();
                }
                File.AppendAllLines(errorFileLocation, errorMessages);
            }
        }

    }
}
