namespace IIIF.Tests.Serialisation.Data;

/// <summary>
/// Real-world manifest from https://leedsunilibrary.exhibitionviewer.org/iiif/marie-hartley.json
/// Used to test deserialisation of complex real-world IIIF content including:
/// - Canvas behavior arrays
/// - SpecificResource with ImageApiSelector and SvgSelector
/// - ImageService2 with array-format profile (full capability declaration)
/// - TextualBody as a painting annotation body
/// - placeholderCanvas/accompanyingCanvas empty arrays
/// </summary>
public static class MarieHartleyManifestData
{
    public const string ManifestId = "https://leedsunilibrary.exhibitionviewer.org/iiif/marie-hartley.json";

    public const string Json = """
        {
          "@context": "http://iiif.io/api/presentation/3/context.json",
          "id": "https://leedsunilibrary.exhibitionviewer.org/iiif/marie-hartley.json",
          "type": "Manifest",
          "label": { "en": ["Welcome to Yorkshire: The art of Marie Hartley"] },
          "metadata": [
            { "label": { "en": ["Collections"] }, "value": { "en": ["University Art Collection"] } },
            { "label": { "en": ["Attribution"] }, "value": { "en": ["Image Credit : Leeds University Library"] } }
          ],
          "rights": "http://rightsstatements.org/vocab/InC/1.0/",
          "provider": [
            {
              "id": "https://library.leeds.ac.uk/info/1600/about",
              "type": "Agent",
              "label": { "en": ["University of Leeds"] },
              "homepage": [
                {
                  "id": "https://https://library.leeds.ac.uk//",
                  "type": "Text",
                  "format": "text/html",
                  "label": { "en": ["Leeds University Library Homepage"] }
                }
              ],
              "logo": [
                {
                  "id": "https://resources.library.leeds.ac.uk/logo/black.png",
                  "type": "Image",
                  "format": "image/png",
                  "height": 61,
                  "width": 300
                }
              ]
            }
          ],
          "items": [
            {
              "id": "https://iiif.library.leeds.ac.uk/canvases/w5b3cz4c_objects_LEEUA_1999.015.731_01.tif/annotations/0/Canvas/4lcxzhhi2tn-mb8hklvw",
              "type": "Canvas",
              "height": 2423,
              "width": 3399,
              "behavior": ["w-7", "h-6"],
              "label": { "en": ["Marie Hartley and Joan Ingilby"] },
              "thumbnail": [
                {
                  "id": "https://iiif.library.leeds.ac.uk/image/v2/w5b3cz4c_objects_LEEUA_1999.015.731_01.tif/61,57,3426,2433/512,/0/default.jpg",
                  "type": "Image",
                  "format": "image/jpeg",
                  "height": 364,
                  "width": 512
                }
              ],
              "items": [
                {
                  "id": "https://iiif.library.leeds.ac.uk/canvases/w5b3cz4c_objects_LEEUA_1999.015.731_01.tif/annotations/0/Page/wwrldo6c0cr-mb8hklvw",
                  "type": "AnnotationPage",
                  "items": [
                    {
                      "id": "https://iiif.library.leeds.ac.uk/canvases/w5b3cz4c_objects_LEEUA_1999.015.731_01.tif/annotations/0/Page/wwrldo6c0cr-mb8hklvw/Annotation/rpiykukjna-mb8hmdhm",
                      "type": "Annotation",
                      "motivation": "painting",
                      "target": "https://iiif.library.leeds.ac.uk/canvases/w5b3cz4c_objects_LEEUA_1999.015.731_01.tif/annotations/0/Canvas/4lcxzhhi2tn-mb8hklvw",
                      "body": {
                        "id": "https://iiif.library.leeds.ac.uk/canvases/w5b3cz4c_objects_LEEUA_1999.015.731_01.tif/annotations/0/SpecificResource/mpbz1u3lbl-mb8hmdhm",
                        "type": "SpecificResource",
                        "source": {
                          "id": "https://iiif.library.leeds.ac.uk/image/v2/w5b3cz4c_objects_LEEUA_1999.015.731_01.tif/70,48,3399,2423/max/0/default.jpg",
                          "type": "Image",
                          "format": "image/jpeg",
                          "height": 742,
                          "width": 1024,
                          "service": [
                            {
                              "@context": "http://iiif.io/api/image/2/context.json",
                              "@id": "https://iiif.library.leeds.ac.uk/image/v2/w5b3cz4c_objects_LEEUA_1999.015.731_01.tif",
                              "@type": "ImageService2",
                              "profile": "http://iiif.io/api/image/2/level2.json",
                              "width": 3544,
                              "height": 2568
                            },
                            {
                              "@context": "http://iiif.io/api/image/3/context.json",
                              "id": "https://iiif.library.leeds.ac.uk/image/w5b3cz4c_objects_LEEUA_1999.015.731_01.tif",
                              "type": "ImageService3",
                              "profile": "level2",
                              "width": 3544,
                              "height": 2568
                            }
                          ]
                        },
                        "selector": { "type": "ImageApiSelector", "region": "70,48,3399,2423" }
                      }
                    }
                  ]
                }
              ]
            },
            {
              "id": "https://leedsunilibrary.exhibitionviewer.org/iiif/marie-hartely.json/canvas/ooexgyxgavl-mb99kqia",
              "type": "Canvas",
              "height": 1000,
              "width": 1000,
              "behavior": ["info", "w-4", "h-5"],
              "placeholderCanvas": [],
              "accompanyingCanvas": [],
              "items": [
                {
                  "id": "https://leedsunilibrary.exhibitionviewer.org/iiif/marie-hartely.json/canvas/ooexgyxgavl-mb99kqia/annotation-page/27y83xapgch-mb99kqia",
                  "type": "AnnotationPage",
                  "items": [
                    {
                      "id": "https://leedsunilibrary.exhibitionviewer.org/iiif/marie-hartely.json/canvas/ooexgyxgavl-mb99kqia/annotation/09fm149od4qo-mb99kqia",
                      "type": "Annotation",
                      "motivation": "painting",
                      "target": "https://leedsunilibrary.exhibitionviewer.org/iiif/marie-hartely.json/canvas/ooexgyxgavl-mb99kqia",
                      "body": {
                        "id": "https://leedsunilibrary.exhibitionviewer.org/iiif/marie-hartely.json/canvas/ooexgyxgavl-mb99kqia/annotation/09fm149od4qo-mb99kqia/html/en/mweernqpo4-mb99kqia",
                        "type": "TextualBody",
                        "format": "text/html",
                        "height": 1000,
                        "width": 1000,
                        "language": "en",
                        "value": "<h2>Discover Yorkshire</h2>"
                      }
                    }
                  ]
                }
              ]
            },
            {
              "id": "https://iiif.library.leeds.ac.uk/canvases/f69h1881_objects_LEEUA_1999.015.505_01.tif/annotations/0/Canvas/2q8szdaux4y-mb8htpn9",
              "type": "Canvas",
              "height": 2779,
              "width": 3810,
              "behavior": ["w-12", "h-6", "left"],
              "label": { "en": ["Geological map of Yorkshire"] },
              "items": [
                {
                  "id": "https://iiif.library.leeds.ac.uk/canvases/f69h1881_objects_LEEUA_1999.015.505_01.tif/annotations/0/Page/ozqr9gdmyv-mb8htpn9",
                  "type": "AnnotationPage",
                  "items": [
                    {
                      "id": "https://leedsunilibrary.exhibitionviewer.org/iiif/marie-hartely.json/Annotation/2u9e19cdo1k-mb8htpn9",
                      "type": "Annotation",
                      "motivation": "painting",
                      "target": "https://iiif.library.leeds.ac.uk/canvases/f69h1881_objects_LEEUA_1999.015.505_01.tif/annotations/0/Canvas/2q8szdaux4y-mb8htpn9",
                      "body": {
                        "id": "https://iiif.library.leeds.ac.uk/canvases/f69h1881_objects_LEEUA_1999.015.505_01.tif/annotations/0/SpecificResource/bg2lxbe648f-mb8htpn9",
                        "type": "SpecificResource",
                        "source": {
                          "id": "https://iiif.library.leeds.ac.uk/image/v2/f69h1881_objects_LEEUA_1999.015.505_01.tif/337,53,3810,2779/max/0/default.jpg",
                          "type": "Image",
                          "format": "image/jpeg",
                          "height": 680,
                          "width": 1024,
                          "service": [
                            {
                              "@context": "http://iiif.io/api/image/2/context.json",
                              "@id": "https://iiif.library.leeds.ac.uk/image/v2/f69h1881_objects_LEEUA_1999.015.505_01.tif",
                              "@type": "ImageService2",
                              "profile": "http://iiif.io/api/image/2/level2.json",
                              "width": 4288,
                              "height": 2848
                            }
                          ]
                        },
                        "selector": { "type": "ImageApiSelector", "region": "337,53,3810,2779" }
                      }
                    }
                  ]
                }
              ],
              "annotations": [
                {
                  "id": "https://iiif.library.leeds.ac.uk/canvases/f69h1881_objects_LEEUA_1999.015.505_01.tif/annotations/0/Canvas/2q8szdaux4y-mb8htpn9/annotations/01880k9bmot7-mb9a3f6c",
                  "type": "AnnotationPage",
                  "label": { "en": ["Tour steps"] },
                  "items": [
                    {
                      "id": "https://iiif.library.leeds.ac.uk/canvases/f69h1881.../annotation/sk7i08atv8n-mb9a8da1",
                      "type": "Annotation",
                      "motivation": "describing",
                      "target": {
                        "type": "SpecificResource",
                        "source": "https://iiif.library.leeds.ac.uk/canvases/f69h1881_objects_LEEUA_1999.015.505_01.tif/annotations/0/Canvas/2q8szdaux4y-mb8htpn9",
                        "selector": [{ "type": "SvgSelector", "value": "<svg></svg>" }]
                      },
                      "body": {
                        "id": "https://iiif.library.leeds.ac.uk/canvases/f69h1881.../annotation/sk7i08atv8n-mb9a8da1/html",
                        "type": "TextualBody",
                        "format": "text/html",
                        "language": "en",
                        "value": "<p>Pickering</p>"
                      }
                    }
                  ]
                }
              ]
            },
            {
              "id": "https://iiif.library.leeds.ac.uk/canvases/yhms7npn_objects_LEEUA_1999.015.037_01.tif/annotations/0/Canvas/rgzuqo2y3z-mb94plkd",
              "type": "Canvas",
              "height": 2473,
              "width": 3052,
              "behavior": ["w-10", "h-5", "right"],
              "label": { "en": ["The Headrow, Leeds"] },
              "thumbnail": [
                {
                  "id": "https://iiif.library.leeds.ac.uk/image/v2/yhms7npn_objects_LEEUA_1999.015.037_01.tif/full/max/0/default.jpg",
                  "type": "Image",
                  "format": "image/jpeg",
                  "height": 2608,
                  "width": 3292,
                  "service": [
                    {
                      "@context": "http://iiif.io/api/image/2/context.json",
                      "@id": "https://iiif.library.leeds.ac.uk/image/v2/yhms7npn_objects_LEEUA_1999.015.037_01.tif",
                      "@type": "iiif:Image",
                      "profile": [
                        "http://iiif.io/api/image/2/level2.json",
                        {
                          "formats": ["tif", "jpg", "gif", "png"],
                          "qualities": ["bitonal", "default", "gray", "color"],
                          "supports": ["regionByPx", "sizeByW", "cors", "sizeByPct"]
                        }
                      ],
                      "protocol": "http://iiif.io/api/image",
                      "width": 3292,
                      "height": 2608,
                      "sizes": [{ "width": 1024, "height": 811 }],
                      "tiles": [{ "width": 512, "height": 512, "scaleFactors": [1, 2, 4, 8] }]
                    }
                  ]
                }
              ],
              "items": [
                {
                  "id": "https://iiif.library.leeds.ac.uk/canvases/yhms7npn.../annotations/0/Page/a73w66fuiel-mb94plkd",
                  "type": "AnnotationPage",
                  "items": [
                    {
                      "id": "https://iiif.library.leeds.ac.uk/canvases/yhms7npn.../Annotation/d9hv4nygvid-mb98qg8l",
                      "type": "Annotation",
                      "motivation": "painting",
                      "target": "https://iiif.library.leeds.ac.uk/canvases/yhms7npn_objects_LEEUA_1999.015.037_01.tif/annotations/0/Canvas/rgzuqo2y3z-mb94plkd",
                      "body": {
                        "id": "https://iiif.library.leeds.ac.uk/canvases/yhms7npn.../SpecificResource/4tktp91ms8w-mb98qg8l",
                        "type": "SpecificResource",
                        "source": {
                          "id": "https://iiif.library.leeds.ac.uk/image/v2/yhms7npn_objects_LEEUA_1999.015.037_01.tif/92,50,3052,2473/max/0/default.jpg",
                          "type": "Image",
                          "format": "image/jpeg",
                          "height": 811,
                          "width": 1024,
                          "service": [
                            {
                              "@context": "http://iiif.io/api/image/2/context.json",
                              "@id": "https://iiif.library.leeds.ac.uk/image/v2/yhms7npn_objects_LEEUA_1999.015.037_01.tif",
                              "@type": "ImageService2",
                              "profile": "http://iiif.io/api/image/2/level2.json",
                              "width": 3292,
                              "height": 2608
                            }
                          ]
                        },
                        "selector": { "type": "ImageApiSelector", "region": "92,50,3052,2473" }
                      }
                    }
                  ]
                }
              ]
            }
          ]
        }
        """;
}
