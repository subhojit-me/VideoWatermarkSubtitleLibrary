using System;

namespace VideowatermarkLibrary.Model
{
    public interface IVideoGenerator
    {
        OperationStatus GenerateVideo();
    }

    public abstract class AbstractVideoGenerator : IVideoGenerator
    {
        public IVideoConfiguration config;

        public virtual OperationStatus GenerateVideo()
        {
            try
            {
                var converter = new NReco.VideoConverter.FFMpegConverter();
                converter.LogReceived += Converter_LogReceived;
                var commandString = config.toCommandString();
                converter.Invoke(commandString);
                return new OperationStatus(true, "Successfully Converted");
            }
            catch (Exception ex)
            {
                return new OperationStatus(false, "Exception Occoured", "Exception: " + ex.Message);
            }
        }

        public virtual void Converter_LogReceived(object sender, NReco.VideoConverter.FFMpegLogEventArgs e)
        {
            var data = e.Data;
            Console.WriteLine(data);
        }

    }
}