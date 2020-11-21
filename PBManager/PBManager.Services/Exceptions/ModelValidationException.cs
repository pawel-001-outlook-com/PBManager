using System;

namespace PBManager.Services.Exceptions
{
    public abstract class ModelValidationException : ApplicationException
    {
        public ModelValidationException(string message)
            : base(message)
        {
        }
    }
}