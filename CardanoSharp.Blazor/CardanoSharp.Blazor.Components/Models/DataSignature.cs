using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Models
{
    public class DataSignature
    {
        /// <summary>
        /// cbor\<COSE_Sign1>
        /// </summary>
        public string? Signature { get; set; }
        /// <summary>
        /// cbor\<COSE_Key>
        /// </summary>
        public string? Key { get; set; }
    }
}
