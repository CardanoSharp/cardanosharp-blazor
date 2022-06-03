using CardanoSharp.Blazor.Components.Models.Errors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Exceptions
{
    public class TxSendException : ErrorCodeException
    {
        public TxSendException()
        {
        }

        public TxSendException(string message)
            : base(message)
        {
        }

        public TxSendException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public TxSendException(InfoCodeError error, string message, Exception inner)
            : base(error, message, inner)
        {
        }
    }
}
