namespace IIIF.Search;

public interface IHasIgnorableParameters
{
    string[]? Ignored { get; set; }
}