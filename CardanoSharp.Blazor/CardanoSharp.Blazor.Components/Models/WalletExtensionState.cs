using CardanoSharp.Wallet.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Models
{
    public class WalletExtensionState : WalletExtension
    {
        public bool Installed { get; set; }
        public bool Enabled { get; set; }
        public bool Connected { get; set; }
        public ulong Balance { get; set; }
        public string BalanceAda
        {
            get
            {
                return (Balance / 1000000).ToString();
            }
        }
        public string BalanceRemainder
        {
            get
            {
                return (Balance - (Balance / 1000000)).ToString("X6");
            }
        }
        public NetworkId Network { get; set; }
        public string CoinCurrency
        {
            get
            {
                if (Network == NetworkId.Mainnet)
                {
                    return "₳";
                }
                return "t₳";
            }
        }

        public ulong TokenPreservation { get; set; }
        public int TokenCount { get; set; }

        public WalletExtensionState(WalletExtension copy)
            : base(copy)
        {
        }
    }
}
