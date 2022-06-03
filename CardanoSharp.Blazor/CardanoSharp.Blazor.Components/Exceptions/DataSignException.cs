using CardanoSharp.Blazor.Components.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Exceptions
{
    public class DataSignException : ErrorCodeException
    {
        public DataSignException()
        {
        }

        public DataSignException(string message)
            : base(message)
        {
        }

        public DataSignException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public DataSignException(InfoCodeError error, string message, Exception inner)
            : base(error, message, inner)
        {
        }
    }
}
