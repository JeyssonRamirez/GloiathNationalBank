namespace Core.DataTransferObject
{
    public class BaseApiResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public object Data { set; get; }
    }

}
