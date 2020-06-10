using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
