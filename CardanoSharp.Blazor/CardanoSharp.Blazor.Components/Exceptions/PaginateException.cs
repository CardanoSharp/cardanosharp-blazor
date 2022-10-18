using CardanoSharp.Wallet.CIPs.CIP30.Models.Errors;

namespace CardanoSharp.Blazor.Components.Exceptions
{
	public class PaginateException : WebWalletException
	{
		public int MaxSize { get; set; }

		public PaginateException()
		{
		}

		public PaginateException(string message)
			: base(message)
		{
		}

		public PaginateException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public PaginateException(PaginateError error, string message, Exception inner)
			: base(message, inner)
		{
			MaxSize = error.MaxSize;
		}
	}
}