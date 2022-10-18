using CardanoSharp.Blazor.Components.Models.Errors;

namespace CardanoSharp.Blazor.Components.Exceptions
{
	public class TxSendException : ErrorCodeException
	{
		public TxSendException()
		{
		}

		public TxSendException(string message)
			: base(message)
		{
		}

		public TxSendException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public TxSendException(InfoCodeError error, string message, Exception inner)
			: base(error, message, inner)
		{
		}
	}
}