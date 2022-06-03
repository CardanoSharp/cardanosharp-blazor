using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Enums
{
    public enum TxSendErrorCode
    {
        /// <summary>
        /// Wallet refuses to send the tx (could be rate limiting)
        /// </summary>
        Refused = 1,
        /// <summary>
        /// Wallet could not send the tx
        /// </summary>
        Failure = 2,
    }
}
