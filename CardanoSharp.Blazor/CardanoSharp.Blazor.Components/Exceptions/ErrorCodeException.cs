using CardanoSharp.Blazor.Components.Models.Errors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Exceptions
{
    public class ErrorCodeException : WebWalletException
    {
        public int Code { get; set; }
        public string? Info { get; set; }

        public ErrorCodeException()
        {
        }

        public ErrorCodeException(string message)
            : base(message)
        {
        }

        public ErrorCodeException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public ErrorCodeException(InfoCodeError error, string message, Exception inner)
            : base(message, inner)
        {
            Code = error.code;
            Info = error.info;
        }
    }
}
