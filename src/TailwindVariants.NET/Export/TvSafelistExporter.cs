namespace TailwindVariants.NET.Export;

/// <summary>
/// Exports Tailwind safelist tokens from descriptor sources.
/// </summary>
/// <remarks>
/// The exporter reads statically declared class strings and splits them into whitespace-separated tokens.
/// It does not evaluate predicates, inspect runtime owner state, or run TailwindMerge because safelist
/// generation must preserve every declared token.
/// </remarks>
public static class TvSafelistExporter
{
	/// <summary>
	/// Extracts unique Tailwind tokens from one or more safelist sources passed as a params array.
	/// </summary>
	/// <param name="sources">The descriptor sources to export.</param>
	/// <returns>A deduplicated token array sorted using default options.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is null, or contains a null item.</exception>
	public static string[] GetTokens(params ITvSafelistSource[] sources)
		=> GetTokens((IEnumerable<ITvSafelistSource>)sources);

	/// <summary>
	/// Extracts unique Tailwind tokens from an enumerable of safelist sources.
	/// </summary>
	/// <param name="sources">The descriptor sources to export.</param>
	/// <param name="options">Optional export settings. When null, <see cref="TvSafelistExportOptions.Default"/> is used.</param>
	/// <returns>A deduplicated token array, optionally sorted using <paramref name="options"/>.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is null, or contains a null item.</exception>
	public static string[] GetTokens(IEnumerable<ITvSafelistSource> sources, TvSafelistExportOptions? options = null)
	{
		ArgumentNullException.ThrowIfNull(sources);
		options ??= TvSafelistExportOptions.Default;

		var seen = new HashSet<string>(options.Comparer);
		var tokens = new List<string>();

		foreach (var source in sources)
		{
			ArgumentNullException.ThrowIfNull(source);

			foreach (var classString in source.GetSafelistClassStrings())
			{
				foreach (var token in TvClassTokenParser.Parse(classString))
				{
					if (seen.Add(token))
					{
						tokens.Add(token);
					}
				}
			}
		}

		if (options.SortTokens)
		{
			tokens.Sort(options.Comparer);
		}

		return [.. tokens];
	}

	/// <summary>
	/// Exports unique Tailwind tokens from one or more safelist sources and joins them with single spaces.
	/// </summary>
	/// <param name="sources">The descriptor sources to export.</param>
	/// <returns>A single space-delimited string of unique tokens.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is null, or contains a null item.</exception>
	public static string GetString(params ITvSafelistSource[] sources)
		=> GetString((IEnumerable<ITvSafelistSource>)sources);

	/// <summary>
	/// Exports unique Tailwind tokens from an enumerable of safelist sources and joins them with single spaces.
	/// </summary>
	/// <param name="sources">The descriptor sources to export.</param>
	/// <param name="options">Optional export settings. When null, <see cref="TvSafelistExportOptions.Default"/> is used.</param>
	/// <returns>A single space-delimited string of unique tokens.</returns>
	/// <exception cref="ArgumentNullException"><paramref name="sources"/> is null, or contains a null item.</exception>
	public static string GetString(IEnumerable<ITvSafelistSource> sources, TvSafelistExportOptions? options = null)
		=> string.Join(" ", GetTokens(sources, options));
}
