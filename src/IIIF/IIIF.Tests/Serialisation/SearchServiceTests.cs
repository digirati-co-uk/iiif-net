using System.Linq;
using IIIF.Presentation.V3;
using IIIF.Search.V1;
using IIIF.Serialisation;

namespace IIIF.Tests.Serialisation;

public class SearchServiceTests
{
    [Fact]
    public void CanDeserialise_SearchService_WithMultipleServiceArray()
    {
        // Test for https://github.com/digirati-co-uk/iiif-net/issues/102
        var manifest = """
                       {
                         "type": "Manifest",
                         "service": [
                           {
                             "@id": "https://iiif.example.org/search/v1/foo",
                             "@type": "SearchService1",
                             "profile": "http://iiif.io/api/search/1/search",
                             "label": "Search within this manifest",
                             "service": [
                               {
                                 "@id": "https://iiif.example.org/search/autocomplete/v1/foo",
                                 "@type": "AutoCompleteService1",
                                 "profile": "http://iiif.io/api/search/1/autocomplete",
                                 "label": "Autocomplete words in this manifest"
                               }
                             ]
                           }
                         ]
                       }
                       """;

        var result = manifest.FromJson<Manifest>();

        var searchService = result!.Service!.OfType<SearchService>().Single();
        searchService.Service.Should().HaveCount(1);
        searchService.Service!.Single().Should().BeOfType<AutoCompleteService>()
            .Which.Id.Should().Be("https://iiif.example.org/search/autocomplete/v1/foo");
    }
    
    [Fact]
    public void CanDeserialise_SearchService_WithSingleService()
    {
      var manifest = """
                     {
                       "type": "Manifest",
                       "service": [
                         {
                           "@id": "https://iiif.example.org/search/v1/foo",
                           "@type": "SearchService1",
                           "profile": "http://iiif.io/api/search/1/search",
                           "label": "Search within this manifest",
                           "service": {
                             "@id": "https://iiif.example.org/search/autocomplete/v1/foo",
                             "@type": "AutoCompleteService1",
                             "profile": "http://iiif.io/api/search/1/autocomplete",
                             "label": "Autocomplete words in this manifest"
                           }
                         }
                       ]
                     }
                     """;

      var result = manifest.FromJson<Manifest>();

      var searchService = result!.Service!.OfType<SearchService>().Single();
      searchService.Service.Should().HaveCount(1);
      searchService.Service!.Single().Should().BeOfType<AutoCompleteService>()
        .Which.Id.Should().Be("https://iiif.example.org/search/autocomplete/v1/foo");
    }
    
    [Fact]
    public void CanDeserialise_SearchService_WithCustomService()
    {
      // Test for https://github.com/digirati-co-uk/iiif-net/issues/102
      var manifest = """
                     {
                       "type": "Manifest",
                       "service": [
                         {
                           "@id": "https://iiif.example.org/search/v1/foo",
                           "@type": "SearchService1",
                           "profile": "http://iiif.io/api/search/1/search",
                           "label": "Search within this manifest",
                           "service": [
                             {
                               "@id": "https://iiif.example.org",
                               "@type": "AnotherService"
                             },
                             {
                               "id": "https://iiif.example.org/v3",
                               "type": "AnotherServiceV3"
                             }
                           ]
                         }
                       ]
                     }
                     """;

      var result = manifest.FromJson<Manifest>();

      var searchService = result!.Service!.OfType<SearchService>().Single();
      searchService.Service.Should().HaveCount(2);
      searchService.Service!.First().Should().BeOfType<V2ServiceReference>()
        .Which.Id.Should().Be("https://iiif.example.org");
      searchService.Service!.Last().Should().BeOfType<ExternalService>()
        .Which.Id.Should().Be("https://iiif.example.org/v3");
    }
}