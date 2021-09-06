using IIIF.Presentation.V3.Content;

namespace IIIF.Search.V2
{
    public class SearchService : ExternalResource, IService
    {
        public const string Search2Context = "http://iiif.io/api/search/2/context.json";
        public const string Search2Profile = "http://iiif.io/api/search/2/search";
        
        public SearchService() : base(nameof(SearchService))
        {
            // Allow callers to decide whether to set the @context
            Profile = Search2Profile;
        }
    }
}