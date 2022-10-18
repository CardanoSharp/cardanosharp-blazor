namespace CardanoSharp.Blazor.TestApp.Shared
{
	public class GetTxFromUtxosRequest
	{
		public string[]? Utxos { get; set; }

		public string? ToAddress { get; set; }

		public ulong ToAmount { get; set; }

		public string? ChangeAddress { get; set; }
	}

	public class GetTxFromUtxosResponse
	{
		public GetTxFromUtxosRequest? Request { get; set; }

		public string? TxCbor { get; set; }
	}
}