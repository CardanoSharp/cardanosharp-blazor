using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Enums
{
    public enum WebWalletErrorType
    {
        Unknown = 0,
        DataSign = 1,
        Paginate = 2,
        TxSend = 3,
        TxSign = 4,
    }
}
