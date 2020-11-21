namespace PBManager.Services.Exceptions
{
    public class AlreadyExistsException : ModelValidationException
    {
        public AlreadyExistsException(string message)
            : base(message)
        {
        }
    }
}