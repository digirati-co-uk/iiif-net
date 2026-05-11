namespace IIIF.Tests.Serialisation.Data;

/// <summary>
/// Sample IIIF Collection used to test deserialisation of
/// a IIIF Presentation 3 Collection including:
/// - Bilingual labels (en + nl)
/// - Items that are Manifest references (id/type/label only, no full body)
/// - Thumbnail on the collection root
/// - Extension property hss:slug (should land in AdditionalProperties)
/// - Some items with thumbnails, some without
/// </summary>
public static class ManifestDataFileTwo
{
    public const string CollectionId =
        "https://example.org/iiif/collections/sample-exhibitions/collection.json";

    public const int ExpectedItemCount = 11;

    public const string Json = """
        {
          "@context": "http://iiif.io/api/presentation/3/context.json",
          "id": "https://example.org/iiif/collections/sample-exhibitions/collection.json",
          "type": "Collection",
          "label": {
            "en": ["Sample Exhibitions"],
            "nl": ["Voorbeeldtentoonstellingen"]
          },
          "thumbnail": [
            {
              "id": "https://assets.example.org/iiif-img/sample-root/full/1024,1024/0/default.jpg",
              "type": "Image",
              "width": 9986,
              "height": 9988
            }
          ],
          "hss:slug": "collections/sample-exhibitions",
          "items": [
            {
              "id": "https://example.org/iiif/manifests/exhibit-01/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Exhibit One"] },
              "hss:slug": "manifests/exhibit-01"
            },
            {
              "id": "https://example.org/iiif/manifests/exhibit-02/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Exhibit Two"], "nl": ["Tentoonstelling Twee"] },
              "hss:slug": "manifests/exhibit-02"
            },
            {
              "id": "https://example.org/iiif/manifests/exhibit-03/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Exhibit Three"] },
              "hss:slug": "manifests/exhibit-03"
            },
            {
              "id": "https://example.org/iiif/manifests/exhibit-04/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Exhibit Four"] },
              "thumbnail": [
                {
                  "id": "https://assets.example.org/iiif-img/exhibit-04/full/1024,1024/0/default.jpg",
                  "type": "Image",
                  "width": 9986,
                  "height": 9988
                }
              ],
              "hss:slug": "manifests/exhibit-04"
            },
            {
              "id": "https://example.org/iiif/manifests/exhibit-05/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Exhibit Five"] },
              "thumbnail": [
                {
                  "id": "https://assets.example.org/iiif-img/exhibit-05/1000,0,3876,3876/1600,1600/0/default.jpg",
                  "type": "Image"
                }
              ],
              "hss:slug": "manifests/exhibit-05"
            },
            {
              "id": "https://example.org/iiif/manifests/exhibit-06/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Exhibit Six"] },
              "thumbnail": [
                {
                  "id": "https://assets.example.org/thumbs/exhibit-06/full/1024,/0/default.jpg",
                  "type": "Image",
                  "width": 1024,
                  "height": 794
                }
              ],
              "hss:slug": "manifests/exhibit-06"
            },
            {
              "id": "https://example.org/iiif/manifests/exhibit-07/manifest.json",
              "type": "Manifest",
              "label": { "nl": ["Tentoonstelling Zeven"], "en": ["Exhibit Seven"] },
              "thumbnail": [
                {
                  "id": "https://assets.example.org/thumbs/exhibit-07/full/1024,/0/default.jpg",
                  "type": "Image",
                  "width": 1024,
                  "height": 774
                }
              ],
              "hss:slug": "manifests/exhibit-07"
            },
            {
              "id": "https://example.org/iiif/manifests/exhibit-08/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Exhibit Eight"] },
              "thumbnail": [
                {
                  "id": "https://assets.example.org/thumbs/exhibit-08/full/826,/0/default.jpg",
                  "type": "Image",
                  "width": 826,
                  "height": 1024
                }
              ],
              "hss:slug": "manifests/exhibit-08"
            },
            {
              "id": "https://example.org/iiif/manifests/exhibit-09/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Exhibit Nine"] },
              "thumbnail": [
                {
                  "id": "https://assets.example.org/thumbs/exhibit-09/full/1024,/0/default.jpg",
                  "type": "Image",
                  "width": 1024,
                  "height": 1006
                }
              ],
              "hss:slug": "manifests/exhibit-09"
            },
            {
              "id": "https://example.org/iiif/manifests/exhibit-10/manifest.json",
              "type": "Manifest",
              "label": {
                "nl": ["Door de lens van een professor"],
                "en": ["Through the Lens of a Professor"]
              },
              "thumbnail": [
                {
                  "id": "https://assets.example.org/iiif-img/exhibit-10/60,83,3815,3814/max/0/default.jpg",
                  "type": "Image",
                  "width": 3944,
                  "height": 3944
                }
              ],
              "hss:slug": "manifests/exhibit-10"
            },
            {
              "id": "https://example.org/iiif/manifests/exhibit-11/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Exhibit Eleven"] },
              "thumbnail": [
                {
                  "id": "https://assets.example.org/thumbs/exhibit-11/full/1024,/0/default.jpg",
                  "type": "Image",
                  "width": 1024,
                  "height": 563
                }
              ],
              "hss:slug": "manifests/exhibit-11"
            }
          ]
        }
        """;
}