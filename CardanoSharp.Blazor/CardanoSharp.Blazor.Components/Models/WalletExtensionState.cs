using CardanoSharp.Wallet.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
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
                var temp = (Balance / 1000000).ToString("N", CultureInfo.CreateSpecificCulture("en-US"));
                return temp.Substring(0, temp.IndexOf('.'));
            }
        }
        public string BalanceRemainder
        {
            get
            {
                return (Balance % 1000000).ToString("D6");
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
        public string TokenPreservationAda
        {
            get
            {
                var temp = (TokenPreservation / 1000000).ToString("N", CultureInfo.CreateSpecificCulture("en-US"));
                return temp.Substring(0, temp.IndexOf('.'));
            }
        }
        public string TokenPreservationRemainder
        {
            get
            {
                return (TokenPreservation % 1000000).ToString("D6");
            }
        }
        public int TokenCount { get; set; }

        public WalletExtensionState(WalletExtension copy)
            : base(copy)
        {
        }
    }
}
