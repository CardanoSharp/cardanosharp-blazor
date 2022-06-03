using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.TestApp.Shared
{
    public class AddWitnessRequest
    {
        public string? WitnessCbor { get; set; }
        public string? TxCbor { get; set; }
    }

    public class AddWitnessResponse
    {
        public AddWitnessRequest? Request { get; set; }
        public string? TxCbor { get; set; }
    }
}
