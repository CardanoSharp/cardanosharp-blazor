using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Models
{
    public class WalletExtension
    {
        public string Key { get; set; } = "";
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public string Url { get; set; } = "";

        public WalletExtension()
        {
        }

        public WalletExtension(WalletExtension copy)
        {
            Key = copy.Key;
            Name = copy.Name;
            Icon = copy.Icon;
            Url = copy.Url;
        }
    }
}
