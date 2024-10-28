using IIIF.Presentation.V3;
using IIIF.Tests.Serialisation.Data;

namespace IIIF.Tests.Serialisation;

[Trait("Category", "Cookbook")]
public class CookbookDeserialization
{
    [Theory]
    [ClassData(typeof(CookbookManifestData))]
    public void Can_Deserialize_Cookbook_Manifest(string manifestId, Manifest manifest)
    {
        // perfunctory assertion
        manifest.Should().NotBeNull();
        manifest.Id.Should().Be(manifestId);
    }
}