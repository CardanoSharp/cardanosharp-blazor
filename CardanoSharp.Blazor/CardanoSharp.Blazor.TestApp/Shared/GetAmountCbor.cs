namespace CardanoSharp.Blazor.TestApp.Shared
{
	public class GetAmountCborRequest
	{
		public ulong Coin { get; set; }
	}

	public class GetAmountCborResponse
	{
		public GetAmountCborRequest Request { get; set; } = null!;

		public string Cbor { get; set; } = null!;
	}
}