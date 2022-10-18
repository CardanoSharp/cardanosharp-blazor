using CardanoSharp.Blazor.Components.Models.Errors;

namespace CardanoSharp.Blazor.Components.Exceptions
{
	public class DataSignException : ErrorCodeException
	{
		public DataSignException()
		{
		}

		public DataSignException(string message)
			: base(message)
		{
		}

		public DataSignException(string message, Exception inner)
			: base(message, inner)
		{
		}

		public DataSignException(InfoCodeError error, string message, Exception inner)
			: base(error, message, inner)
		{
		}
	}
}