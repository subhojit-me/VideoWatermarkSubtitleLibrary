using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VideowatermarkLibrary.Helper
{
    public class SubtitleHelper : IDisposable
    {
        public string subtitleFileLocation { get; private set; }
        public string subtitleText { get; private set; }

        public SubtitleHelper(string mediaFileLocation, string SubtitleMessage, string inputFileName)
        {
            SubtitleMessage = string.IsNullOrWhiteSpace(SubtitleMessage) ? "                   " : SubtitleMessage;
            subtitleText = GetSubtitleText(mediaFileLocation, SubtitleMessage);
            subtitleFileLocation = VideoContainerFolderLocation() + "\\subtitle_" + inputFileName + ".srt";
        }

        /// <summary>
        /// Creates the subtitle File with the message that given in the constructor
        /// and returns the location where subtitle is cretaed
        /// </summary>
        public string CreateSubtitleFile()
        {
            System.IO.File.WriteAllText(subtitleFileLocation, subtitleText);
            return subtitleFileLocation;
        }

        public void DeleteCreatedSubtitleFile()
        {
            System.IO.File.Delete(subtitleFileLocation);
        }

        private string VideoContainerFolderLocation()
        {
            return System.IO.Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName + "\\VideoContainer\\Videos";
        }

        private string GetSubtitleText(string mediaFileLocation, string SubtitleMessage)
        {
            var videoEndTime = this.GetVideoEndTime(mediaFileLocation);
            var totalSubtitleText = $@"1
00:00:00,000 --> {videoEndTime}
{SubtitleMessage}
";
            return totalSubtitleText;
        }

        private string GetVideoEndTime(string mediaFileLocation)
        {
            var mediainfo = new WMPLib.WindowsMediaPlayer().newMedia(mediaFileLocation);
            var durationString = mediainfo.durationString;
            var duration = mediainfo.duration;
            var videoEndTime = TimeSpan.FromSeconds(duration).ToString(@"hh\:mm\:ss\,fff");
            return videoEndTime;
        }

        #region Dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;
        SafeHandle handle = new Microsoft.Win32.SafeHandles.SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
            }

            disposed = true;
        }
        ~SubtitleHelper()
        {
            Dispose(false);
        }
        #endregion
    }
}
