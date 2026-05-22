namespace TailwindVariants.NET.Export;

internal static class TvClassTokenParser
{
	public static IEnumerable<string> Parse(string? classString)
	{
		if (string.IsNullOrWhiteSpace(classString))
		{
			yield break;
		}

		foreach (var token in classString.Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
		{
			if (!string.IsNullOrWhiteSpace(token))
			{
				yield return token;
			}
		}
	}
}
