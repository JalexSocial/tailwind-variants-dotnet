using TailwindVariants.NET.Export;

namespace TailwindVariants.NET.Tests.Export;

public sealed class TvSafelistExporterTests
{
	[Fact]
	public void GetTokens_ExportsBaseVariantAndCompoundVariantTokens()
	{
		var tokens = TvSafelistExporter.GetTokens(SafelistTestButton.Descriptor);
		var expected = new[] { "font-semibold", "border", "rounded", "bg-blue-500", "text-white", "border-transparent", "bg-white", "text-gray-800", "border-gray-400", "text-sm", "py-1", "px-2", "text-base", "py-2", "px-4", "hover:bg-blue-600" }
			.Distinct(StringComparer.Ordinal).OrderBy(x => x, StringComparer.Ordinal).ToArray();
		Assert.Equal(expected, tokens);
	}

	[Fact] public void GetTokens_AlwaysDeduplicatesTokens(){ var tokens = TvSafelistExporter.GetTokens(SafelistDuplicateTokenComponent.Descriptor); Assert.Single(tokens.Where(t=>t=="px-4")); Assert.Single(tokens.Where(t=>t=="text-sm")); }
	[Fact] public void GetTokens_AcceptsMultipleDescriptors(){ var tokens=TvSafelistExporter.GetTokens(SafelistTestButton.Descriptor,SafelistTestAlert.Descriptor); Assert.Contains("bg-blue-500",tokens); Assert.Contains("rounded-md",tokens); Assert.Contains("border-red-500",tokens); Assert.Contains("text-red-700",tokens); Assert.Equal(tokens.Distinct(StringComparer.Ordinal).Count(),tokens.Length); }
	[Fact] public void GetString_ReturnsSpaceSeparatedTokens(){ var tokens=TvSafelistExporter.GetTokens(SafelistTestAlert.Descriptor); var s=TvSafelistExporter.GetString(SafelistTestAlert.Descriptor); Assert.Equal(string.Join(" ",tokens),s); Assert.DoesNotContain("  ",s); Assert.False(s.EndsWith(" ", StringComparison.Ordinal)); }
	[Fact] public void GetTokens_PreservesTailwindModifiersAndArbitraryValueTokens(){ var tokens=TvSafelistExporter.GetTokens(SafelistArbitraryValueComponent.Descriptor); Assert.Contains("hover:bg-blue-600",tokens); Assert.Contains("md:hover:bg-blue-700",tokens); Assert.Contains("dark:focus:ring-2",tokens); Assert.Contains("data-[state=open]:animate-in",tokens); Assert.Contains("[&>svg]:size-4",tokens); Assert.Contains("grid-cols-[1fr_500px_2fr]",tokens); Assert.Contains("bg-[oklch(0.7_0.15_240)]",tokens); }
	[Fact] public void GetTokens_ThrowsForNullSourcesCollection(){ IEnumerable<ITvSafelistSource>? src=null; Assert.Throws<ArgumentNullException>(()=>TvSafelistExporter.GetTokens(src!)); }
	[Fact] public void GetTokens_ThrowsForNullSourceInsideCollection(){ ITvSafelistSource?[] src=[SafelistTestButton.Descriptor,null]; Assert.Throws<ArgumentNullException>(()=>TvSafelistExporter.GetTokens(src!)); }
	[Fact] public void GetTokens_CanPreserveFirstSeenOrderWhenSortingIsDisabled(){ var tokens=TvSafelistExporter.GetTokens([SafelistOrderedTokenComponent.Descriptor],new TvSafelistExportOptions{SortTokens=false}); Assert.Equal(["z-token","a-token","m-token"],tokens); }
	[Fact] public void GetTokens_UsesConfiguredComparerForDeduplicationAndSorting(){ var defaultTokens=TvSafelistExporter.GetTokens(SafelistCaseSensitiveComponent.Descriptor); Assert.Contains("Token",defaultTokens); Assert.Contains("token",defaultTokens); Assert.Contains("TOKEN",defaultTokens); var ignoreCase=TvSafelistExporter.GetTokens([SafelistCaseSensitiveComponent.Descriptor],new TvSafelistExportOptions{SortTokens=false,Comparer=StringComparer.OrdinalIgnoreCase}); Assert.Equal(["Token"],ignoreCase); }
}

public class SafelistTestButton : ISlottable<SafelistTestSlots>
{
	public static readonly TvDescriptor<SafelistTestButton, SafelistTestSlots> Descriptor = new(@base: "font-semibold border rounded", variants: new(){ [b=>b.Variant]=new Variant<ButtonVariant,SafelistTestSlots>{{ButtonVariant.Primary,"bg-blue-500 text-white border-transparent"},{ButtonVariant.Secondary,"bg-white text-gray-800 border-gray-400"}}, [b=>b.Size]=new Variant<ButtonSize,SafelistTestSlots>{{ButtonSize.Small,"text-sm py-1 px-2"},{ButtonSize.Medium,"text-base py-2 px-4"}} }, compoundVariants:[new(b=>b.Variant==ButtonVariant.Primary && !b.Disabled){Class="hover:bg-blue-600"}]);
	public string? Class { get; set; }
	public SafelistTestSlots? Classes { get; set; }
	public ButtonVariant Variant { get; set; }
	public ButtonSize Size { get; set; }
	public bool Disabled { get; set; }
	public enum ButtonVariant { Primary, Secondary }
	public enum ButtonSize { Small, Medium }
}

public class SafelistTestAlert : ISlottable<SafelistTestSlots>
{
	public static readonly TvDescriptor<SafelistTestAlert, SafelistTestSlots> Descriptor = new(@base:"rounded-md border-red-500 text-red-700", variants:new(){ [a=>a.Intent]=new Variant<AlertIntent,SafelistTestSlots>{{AlertIntent.Danger,"bg-red-50"},{AlertIntent.Warning,"bg-yellow-50 text-yellow-800"}}});
	public string? Class { get; set; }
	public SafelistTestSlots? Classes { get; set; }
	public AlertIntent Intent { get; set; }
	public enum AlertIntent { Danger, Warning }
}

public class SafelistDuplicateTokenComponent : ISlottable<SafelistTestSlots>{ public static readonly TvDescriptor<SafelistDuplicateTokenComponent,SafelistTestSlots> Descriptor=new(@base:"px-4 text-sm px-4",variants:new(){[c=>c.Size]=new Variant<SizeVariant,SafelistTestSlots>{{SizeVariant.Small,"px-4 text-sm"},{SizeVariant.Medium,"px-4 text-sm"}}}); public string? Class{get;set;} public SafelistTestSlots? Classes{get;set;} public SizeVariant Size{get;set;} public enum SizeVariant{Small,Medium}}
public class SafelistArbitraryValueComponent : ISlottable<SafelistTestSlots>{ public static readonly TvDescriptor<SafelistArbitraryValueComponent,SafelistTestSlots> Descriptor=new(@base:"hover:bg-blue-600 md:hover:bg-blue-700 dark:focus:ring-2 data-[state=open]:animate-in [&>svg]:size-4 grid-cols-[1fr_500px_2fr] bg-[oklch(0.7_0.15_240)]"); public string? Class{get;set;} public SafelistTestSlots? Classes{get;set;} }
public class SafelistOrderedTokenComponent : ISlottable<SafelistTestSlots>{ public static readonly TvDescriptor<SafelistOrderedTokenComponent,SafelistTestSlots> Descriptor=new(@base:"z-token a-token m-token z-token"); public string? Class{get;set;} public SafelistTestSlots? Classes{get;set;} }
public class SafelistCaseSensitiveComponent : ISlottable<SafelistTestSlots>{ public static readonly TvDescriptor<SafelistCaseSensitiveComponent,SafelistTestSlots> Descriptor=new(@base:"Token token TOKEN"); public string? Class{get;set;} public SafelistTestSlots? Classes{get;set;} }
public sealed partial class SafelistTestSlots : ISlots { public string? Base { get; set; } }
