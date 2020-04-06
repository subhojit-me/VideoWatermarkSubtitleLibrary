

namespace VideowatermarkLibrary.Model
{
    public class OperationStatus
    {
        public bool IsSuccess { get; private set; }
        public object Data { get; private set; }
        public string ErrorMessage { get; private set; }

        public OperationStatus(bool success, object data = null, string error = null)
        {
            this.IsSuccess = success;
            Data = data;
            ErrorMessage = error;
        }
    }
}