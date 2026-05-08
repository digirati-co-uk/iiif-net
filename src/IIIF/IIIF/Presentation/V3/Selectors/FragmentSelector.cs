namespace IIIF.Presentation.V3.Selectors;

public class FragmentSelector : ISelector
{
    public string Type => nameof(FragmentSelector);
    public string? ConformsTo { get; set; }
    public string? Value { get; set; }
}