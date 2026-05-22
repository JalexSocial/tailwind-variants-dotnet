namespace TailwindVariants.NET.Export;

/// <summary>
/// Exposes raw descriptor-declared class strings that can be tokenized for Tailwind safelist generation.
/// </summary>
/// <remarks>
/// Implementations should return statically declared class strings from descriptor configuration,
/// not runtime-resolved, merged, or state-dependent output. Null, empty, and whitespace-only values
/// are allowed and are ignored by <see cref="TvSafelistExporter"/>.
/// </remarks>
public interface ITvSafelistSource
{
	/// <summary>
	/// Gets the raw class strings declared by this source for safelist token extraction.
	/// </summary>
	/// <returns>
	/// A sequence of raw class strings declared by the source. Items may be null or whitespace-only.
	/// </returns>
	IEnumerable<string?> GetSafelistClassStrings();
}
