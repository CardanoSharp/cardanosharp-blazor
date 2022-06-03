using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.TestApp.Shared
{
    public class GetAmountCborRequest
    {
        public ulong Coin { get; set; }
    }

    public class GetAmountCborResponse
    {
        public GetAmountCborRequest Request { get; set; }
        public string Cbor { get; set; }
    }
}
