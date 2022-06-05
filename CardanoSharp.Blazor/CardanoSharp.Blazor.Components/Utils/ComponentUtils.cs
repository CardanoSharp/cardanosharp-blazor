using CardanoSharp.Wallet.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardanoSharp.Blazor.Components.Utils
{
    public static class ComponentUtils
    {

        public static NetworkType GetNetworkType(int networkId)
        {
            switch (networkId)
            {
                case 0: return NetworkType.Testnet;
                case 1: return NetworkType.Mainnet;
                default: return NetworkType.Unknown;
            }
        }

        public static string GetPrefixHeader(AddressType addressType)
        {
            switch (addressType)
            {
                case AddressType.Reward: return "stake";
                case AddressType.Base: return "addr";
                case AddressType.Enterprise: return "addr";
                default: throw new Exception("Unknown address type");
            }
        }

        public static string GetPrefixTail(NetworkType networkType)
        {
            switch (networkType)
            {
                case NetworkType.Testnet: return "_test";
                case NetworkType.Mainnet: return "";
                default: throw new Exception("Unknown address type");
            }
        }

        public static string GetPrefix(AddressType addressType, NetworkType networkType)
        {
            return $"{GetPrefixHeader(addressType)}{GetPrefixTail(networkType)}";
        }
    }
}
