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

        public WalletExtensionState(WalletExtension copy)
            : base(copy)
        {
        }
    }
}
