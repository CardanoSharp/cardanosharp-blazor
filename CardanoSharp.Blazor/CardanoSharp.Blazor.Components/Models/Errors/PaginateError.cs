using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Models.Errors
{
    public class PaginateError
    {
        public const string Schema = @"{ 'maxSize': {'type':'int'} }";

        public int maxSize { get; set; }
    }
}
