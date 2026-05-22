namespace TailwindVariants.NET.Export;

/// <summary>
/// Controls deterministic ordering and token comparison behavior used by <see cref="TvSafelistExporter"/>.
/// </summary>
/// <remarks>
/// Exported tokens are always deduplicated. The configured comparer is used for uniqueness checks
/// and, when enabled, deterministic sorting.
/// </remarks>
public sealed class TvSafelistExportOptions
{
	/// <summary>
	/// Gets the standard exporter behavior: ordinal comparison with sorted unique output.
	/// </summary>
	public static TvSafelistExportOptions Default { get; } = new();

	/// <summary>
	/// Gets a value indicating whether unique tokens are sorted for deterministic output and cleaner generated files.
	/// </summary>
	public bool SortTokens { get; init; } = true;

	/// <summary>
	/// Gets the comparer used for both deduplication and optional sorting.
	/// </summary>
	/// <remarks>
	/// The default is <see cref="StringComparer.Ordinal"/> because Tailwind class tokens are treated
	/// as case-sensitive literal strings.
	/// </remarks>
	public StringComparer Comparer { get; init; } = StringComparer.Ordinal;
}
