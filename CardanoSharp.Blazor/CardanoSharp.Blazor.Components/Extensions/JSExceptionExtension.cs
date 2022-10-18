using CardanoSharp.Blazor.Components.Enums;
using CardanoSharp.Blazor.Components.Exceptions;
using CardanoSharp.Blazor.Components.Models.Errors;
using CardanoSharp.Wallet.CIPs.CIP30.Models.Errors;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace CardanoSharp.Blazor.Components.Extensions
{
	public static class JSExceptionExtension
	{
		public static WebWalletException ToWebWalletException(this JSException jsException, WebWalletErrorType errorTypeHint = WebWalletErrorType.Unknown)
		{
			try
			{
				var errorCode = JsonConvert.DeserializeObject<InfoCodeError>(jsException.Message);
				if (errorCode != null && errorCode.code != 0)
				{
					if (errorCode.code > 0)
					{
						//only the more method specific errors have positive codes
						//either datasign/txsend/txsign
						switch (errorTypeHint)
						{
							case WebWalletErrorType.DataSign:
								return new DataSignException(errorCode, jsException.Message, jsException);

							case WebWalletErrorType.TxSend:
								return new TxSendException(errorCode, jsException.Message, jsException);

							case WebWalletErrorType.TxSign:
								return new TxSignException(errorCode, jsException.Message, jsException);
						}
					}

					//is definitely error code type error (API Error in CIP30 spec)
					return new ErrorCodeException(errorCode, jsException.Message, jsException);
				}
			}
			catch { }

			try
			{
				var paginateError = JsonConvert.DeserializeObject<PaginateError>(jsException.Message);
				if (paginateError != null && paginateError.MaxSize != 0)
				{
					//is definitely paginate type error
					return new PaginateException(paginateError, jsException.Message, jsException);
				}
			}
			catch { }

			//cant determine error type, return general error
			return new WebWalletException(jsException.Message, jsException);
		}
	}
}