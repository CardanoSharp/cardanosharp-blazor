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
    public class PaginateException : WebWalletException
    {
        public int MaxSize { get; set; }

        public PaginateException()
        {
        }

        public PaginateException(string message)
            : base(message)
        {
        }

        public PaginateException(string message, Exception inner)
            : base(message, inner)
        {
        }

        public PaginateException(PaginateError error, string message, Exception inner)
            : base(message, inner)
        {
            MaxSize = error.maxSize;
        }
    }
}
