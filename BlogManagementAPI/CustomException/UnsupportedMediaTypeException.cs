namespace BlogManagementAPI.CustomException
{
    public class UnsupportedMediaTypeException : Exception
    {
        public UnsupportedMediaTypeException(string message) : base(message) { }
    }
}
