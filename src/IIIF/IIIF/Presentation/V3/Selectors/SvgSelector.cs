namespace IIIF.Presentation.V3.Selectors;

public class SvgSelector : ISelector
{
    public string? Type => nameof(SvgSelector);
    public string? Value { get; set; }
}