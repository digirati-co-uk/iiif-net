using System;
using System.Collections.Generic;
using System.Net.Http;
using IIIF.Presentation.V3;
using IIIF.Serialisation;

namespace IIIF.Tests.Serialisation;

public class CookbookDeserialization
{
    private Collection theseusCollection;
    private HttpClient httpClient;
    private List<string> skip;
    
    public CookbookDeserialization()
    {
        httpClient = new HttpClient();
        var s = httpClient.GetStringAsync("https://theseus-viewer.netlify.app/cookbook-collection.json").Result;
        theseusCollection = s.FromJson<Collection>();
        
        // these have bugs in the cookbook, see https://github.com/IIIF/cookbook-recipes/pull/546
        skip = new List<string>
        {
            "https://iiif.io/api/cookbook/recipe/0219-using-caption-file/manifest.json",
            "https://iiif.io/api/cookbook/recipe/0040-image-rotation-service/manifest-service.json"
        };
    }

    [Fact]
    public void Can_Deserialize_Cookbook_Collection()
    {
        foreach (var item in theseusCollection.Items!)
        {
            if (item is Manifest manifestRef)
            {
                if (!skip.Contains(manifestRef.Id))
                {
                    var s = httpClient.GetStringAsync(manifestRef.Id).Result;
                    var manifest = s.FromJson<Manifest>();
                    // perfunctory assertion
                    manifest.Id.Should().Be(manifestRef.Id);
                }
            }
            // Do collections too...
        }
        
    }
    
}