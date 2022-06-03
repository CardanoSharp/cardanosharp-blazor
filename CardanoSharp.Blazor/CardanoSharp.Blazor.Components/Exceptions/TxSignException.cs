using CardanoSharp.Blazor.Components.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Exceptions
{
    public class TxSignException : ErrorCodeException
    {
        public TxSignException()
        {
        }

        public TxSignException(string message)
            : base(message)
        {
        }

        public TxSignException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TxSignException(InfoCodeError error, string message, Exception inner)
            : base(error, message, inner)
        {
        }
    }
}
