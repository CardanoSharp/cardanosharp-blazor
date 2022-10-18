using CardanoSharp.Blazor.TestApp.Shared;
using CardanoSharp.Wallet.CIPs.CIP30.Extensions.Models;
using CardanoSharp.Wallet.Extensions;
using CardanoSharp.Wallet.Extensions.Models.Transactions;
using CardanoSharp.Wallet.Extensions.Models.Transactions.TransactionWitnesses;
using CardanoSharp.Wallet.Models.Transactions;
using Microsoft.AspNetCore.Mvc;

namespace CardanoSharp.Blazor.TestApp.Server.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WalletConnectorController : ControllerBase
	{
		private readonly ILogger<WalletConnectorController> _logger;

		public WalletConnectorController(ILogger<WalletConnectorController> logger)
		{
			_logger = logger;
		}

		[HttpPost]
		[Route("/walletconnector/addwitness")]
		public AddWitnessResponse AddWitness(AddWitnessRequest request)
		{
			var transaction = request.TxCbor.HexToByteArray().DeserializeTransaction();
			if (transaction == null)
			{
				throw new InvalidOperationException("Could not deserialize txCbor");
			}

			var witnessSet = request.WitnessCbor.HexToByteArray().DeserializeTransactionWitnessSet();

			foreach (var vkeyWitness in witnessSet.VKeyWitnesses)
			{
				transaction.TransactionWitnessSet.VKeyWitnesses.Add(vkeyWitness);
			}
			foreach (var nativeScript in witnessSet.NativeScripts)
			{
				transaction.TransactionWitnessSet.NativeScripts.Add(nativeScript);
			}
			foreach (var bootstrap in witnessSet.BootStrapWitnesses)
			{
				transaction.TransactionWitnessSet.BootStrapWitnesses.Add(bootstrap);
			}

			var response = new AddWitnessResponse();
			response.Request = request;
			response.TxCbor = transaction.Serialize().ToStringHex();
			return response;
		}

		[HttpPost]
		[Route("/walletconnector/getamountcbor")]
		public GetAmountCborResponse GetAmountCbor(GetAmountCborRequest request)
		{
			var outputValue = new TransactionOutputValue();
			outputValue.Coin = request.Coin;

			var response = new GetAmountCborResponse()
			{
				Request = request,
			};
			response.Cbor = outputValue.Serialize().ToStringHex();
			return response;
		}

		[HttpPost]
		[Route("/walletconnector/getamountcbor")]
		public GetTxFromUtxosResponse GetTxFromUtxos(GetTxFromUtxosRequest request)
		{
			var response = new GetTxFromUtxosResponse()
			{
				Request = request
			};
			if (request.Utxos == null)
			{
				return response;
			}

			foreach (var utxoCbor in request.Utxos)
			{
				var utxo = utxoCbor.HexToByteArray().DeserializeTransactionUnspentOutput();
			}

			return response;
		}
	}
}