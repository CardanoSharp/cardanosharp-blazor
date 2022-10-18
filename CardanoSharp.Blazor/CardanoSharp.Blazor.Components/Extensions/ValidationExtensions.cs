namespace CardanoSharp.Blazor.Components.Extensions;

public static class ValidationExtensions
{
	public static T ThrowIfNull<T>(this T reference)
	{
		if (reference == null)
			throw new NullReferenceException(nameof(reference));
		return reference;
	}
}