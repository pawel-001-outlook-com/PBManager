namespace PBManager.Services.Exceptions
{
    public class IllegalOperationException : ModelValidationException
    {
        public IllegalOperationException(string message)
            : base(message)
        {
        }
    }
}