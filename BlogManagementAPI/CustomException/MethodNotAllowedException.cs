namespace BlogManagementAPI.CustomException
{
    public class MethodNotAllowedException : Exception
    {
        public MethodNotAllowedException(string message) : base(message) { }
    }
}
