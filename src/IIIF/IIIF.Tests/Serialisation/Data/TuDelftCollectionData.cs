namespace IIIF.Tests.Serialisation.Data;

/// <summary>
/// Real-world IIIF Collection from https://heritage.tudelft.nl/iiif/collections/exhibitions/collection.json
/// Used to test deserialisation of a IIIF Presentation 3 Collection including:
/// - Bilingual labels (en + nl)
/// - Items that are Manifest references (id/type/label only, no full body)
/// - Thumbnail on the collection root
/// - Extension property hss:slug (should land in AdditionalProperties)
/// - Some items with thumbnails, some without
/// </summary>
public static class TuDelftCollectionData
{
    public const string CollectionId =
        "https://heritage.tudelft.nl/iiif/collections/exhibitions/collection.json";

    public const int ExpectedItemCount = 11;

    public const string Json = """
        {
          "@context": "http://iiif.io/api/presentation/3/context.json",
          "id": "https://heritage.tudelft.nl/iiif/collections/exhibitions/collection.json",
          "type": "Collection",
          "label": {
            "en": ["Exhibitions"],
            "nl": ["Tentoonstellingen"]
          },
          "thumbnail": [
            {
              "id": "https://dlc.services/iiif-img/7/24/16dde997eaab5528494b156c196700bd/full/1024,1024/0/default.jpg",
              "type": "Image",
              "width": 9986,
              "height": 9988
            }
          ],
          "hss:slug": "collections/exhibitions",
          "items": [
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/bandoeng-bandung/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Bandoeng \u2013 Bandung"] },
              "hss:slug": "manifests/bandoeng-bandung"
            },
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/collection-wall/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Collection wall"], "nl": ["Collectiewand"] },
              "hss:slug": "manifests/collection-wall"
            },
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/corona-chronicles/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Corona Chronicles"] },
              "hss:slug": "manifests/corona-chronicles"
            },
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/gen-ai/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Gen AI on the Fly"] },
              "thumbnail": [
                {
                  "id": "https://dlc.services/iiif-img/7/24/16dde997eaab5528494b156c196700bd/full/1024,1024/0/default.jpg",
                  "type": "Image",
                  "width": 9986,
                  "height": 9988
                }
              ],
              "hss:slug": "manifests/gen-ai"
            },
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/inventing-creativity/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Inventing Creativity"] },
              "thumbnail": [
                {
                  "id": "https://dlc.services/iiif-img/7/21/76c17db9-e1cd-479d-8726-e995c478f2ad/1000,0,3876,3876/1600,1600/0/default.jpg",
                  "type": "Image"
                }
              ],
              "hss:slug": "manifests/inventing-creativity"
            },
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/irrigation-knowledge/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Irrigation knowledge"] },
              "thumbnail": [
                {
                  "id": "https://dlc.services/thumbs/7/21/17da5645-e7b1-8870-1de4-ac34fa58420a/full/1024,/0/default.jpg",
                  "type": "Image",
                  "width": 1024,
                  "height": 794
                }
              ],
              "hss:slug": "manifests/irrigation-knowledge"
            },
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/novieten/manifest.json",
              "type": "Manifest",
              "label": { "nl": ["Novieten"], "en": ["Novices"] },
              "thumbnail": [
                {
                  "id": "https://dlc.services/thumbs/7/21/e16c7a6b-9672-d551-70d7-08c88609efb1/full/1024,/0/default.jpg",
                  "type": "Image",
                  "width": 1024,
                  "height": 774
                }
              ],
              "hss:slug": "manifests/novieten"
            },
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/places-of-commemoration/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Places of Commemoration"] },
              "thumbnail": [
                {
                  "id": "https://dlc.services/thumbs/7/21/20afde89-6154-4843-974c-388f972b76f4/full/826,/0/default.jpg",
                  "type": "Image",
                  "width": 826,
                  "height": 1024
                }
              ],
              "hss:slug": "manifests/places-of-commemoration"
            },
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/rise-of-a-campus/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Rise of a Campus"] },
              "thumbnail": [
                {
                  "id": "https://dlc.services/thumbs/7/17/b9a7d3c2-35a3-447c-9191-bef328ee312d/full/1024,/0/default.jpg",
                  "type": "Image",
                  "width": 1024,
                  "height": 1006
                }
              ],
              "hss:slug": "manifests/rise-of-a-campus"
            },
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/through-the-lens-of/manifest.json",
              "type": "Manifest",
              "label": {
                "nl": ["Door de lens van professor Van Heel"],
                "en": ["Through the lens of Professor Van Heel"]
              },
              "thumbnail": [
                {
                  "id": "https://dlc.services/iiif-img/7/37/c4d51a9ae4e0d50e553b123b44e5f8b9/60,83,3815,3814/max/0/default.jpg",
                  "type": "Image",
                  "width": 3944,
                  "height": 3944
                }
              ],
              "hss:slug": "manifests/through-the-lens-of"
            },
            {
              "id": "https://heritage.tudelft.nl/iiif/manifests/voices-of-wis/manifest.json",
              "type": "Manifest",
              "label": { "en": ["Voices of Women in Science "] },
              "thumbnail": [
                {
                  "id": "https://dlc.services/thumbs/7/24/14204410-74cc-5796-04f2-a59eeaa8ab44/full/1024,/0/default.jpg",
                  "type": "Image",
                  "width": 1024,
                  "height": 563
                }
              ],
              "hss:slug": "manifests/voices-of-wis"
            }
          ]
        }
        """;
}