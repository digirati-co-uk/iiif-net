using IIIF.Presentation.V3.Content;

namespace IIIF.Search.V2;

public class AutoCompleteService : ExternalResource, IService
{
    public const string AutoComplete2Profile = "http://iiif.io/api/search/2/autocomplete";

    public AutoCompleteService() : base(nameof(AutoCompleteService))
    {
        // Allow callers to decide whether to set the @context
        Profile = AutoComplete2Profile;
    }
}