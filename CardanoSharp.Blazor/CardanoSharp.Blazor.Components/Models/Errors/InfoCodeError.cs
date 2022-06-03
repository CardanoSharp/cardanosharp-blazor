using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Models.Errors
{
    public class InfoCodeError
    {
        public const string Schema = @"{ 'code': {'type':'int'}, 'info': {'type':'string'} }";

        public int code { get; set; }
        public string? info { get; set; }
    }
}
