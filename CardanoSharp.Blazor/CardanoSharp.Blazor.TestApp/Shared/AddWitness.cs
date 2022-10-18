namespace CardanoSharp.Blazor.TestApp.Shared
{
	public class AddWitnessRequest
	{
		public string? WitnessCbor { get; set; }

		public string? TxCbor { get; set; }
	}

	public class AddWitnessResponse
	{
		public AddWitnessRequest? Request { get; set; }

		public string? TxCbor { get; set; }
	}
}