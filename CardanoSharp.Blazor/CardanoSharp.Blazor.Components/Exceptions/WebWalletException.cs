using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Exceptions
{
    public class WebWalletException : Exception
    {
        public WebWalletException()
        {
        }

        public WebWalletException(string message)
            : base(message)
        {
        }

        public WebWalletException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
