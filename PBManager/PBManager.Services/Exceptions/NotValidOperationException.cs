using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PBManager.Services.Exceptions
{
    public class NotValidOperationException : ModelValidationException
    {
        public NotValidOperationException(string message)
            : base(message)
        {

        }
    }
}
