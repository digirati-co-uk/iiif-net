using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using IIIF.Presentation.V3;
using IIIF.Serialisation;

namespace IIIF.Tests.Serialisation.Data;

/// <summary>
/// Used as [ClassData] - contains Manifests from IIIF Cookbook to validate deserialisation
/// </summary>
public class CookbookManifestData : IEnumerable<object[]>
{
    // This will store { manifest-id, deserialized-manifest }
    private readonly List<object[]> data = new();
    
    // these have bugs in the cookbook, see https://github.com/IIIF/cookbook-recipes/pull/546
    private List<string> skip = new()
    {
        "https://iiif.io/api/cookbook/recipe/0219-using-caption-file/manifest.json",
        "https://iiif.io/api/cookbook/recipe/0040-image-rotation-service/manifest-service.json"
    };
    
    public CookbookManifestData()
    {
        using var httpClient = new HttpClient();
        var theseusCollection =
            GetIIIFResource<Collection>("https://theseus-viewer.netlify.app/cookbook-collection.json", true);

        foreach (var item in theseusCollection.Items!)
        {
            if (item is Manifest manifestRef)
            {
                if (skip.Contains(manifestRef.Id)) continue;
                
                var iiif = GetIIIFResource<Manifest>(manifestRef.Id);
                data.Add(new object[] { manifestRef.Id, iiif });
            }
        }
        
        T GetIIIFResource<T>(string url, bool mustSucceed = false) where T : JsonLdBase
        {
            var resource = httpClient.GetAsync(url).Result;
            if (mustSucceed) resource.EnsureSuccessStatusCode();
            if (!resource.IsSuccessStatusCode) return null;

            try
            {
                var iiif = resource.Content.ReadAsStream().FromJsonStream<T>();
                return iiif;
            }
            catch (Exception)
            {
                if (mustSucceed) throw;
                return null;
            }
        }
    }

    public IEnumerator<object[]> GetEnumerator() => data.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}