# IIIF-net

[![](https://img.shields.io/nuget/v/iiif-net)](https://www.nuget.org/packages/iiif-net/)

This repo contains a collection of POCO's for modelling various IIIF APIs.

The different APIs are split by namespace containing name and version.

E.g. `IIIF.Auth.V1`, `IIIF.Presentation.V2`, `IIIF.Presentation.V3`.

## Model Notes

Strongly typed models generally have public properties that map to possible properties as detailed in the relevant IIIF spec, however there are a few differences to this:

### Canvas

To serialise a Presentation3 `Canvas` object as "id" only when used as `Annotation.Target` there are 2 approaches:

* Have a "simple" canvas object, which has no `height`, `width` or `items`.
* Explicitly set `canvas.SerialiseTargetAsId = true`, this allows reuse of a full `Canvas` object.

E.g. The following would both serialise to `"target": "http://iiif-net.example/canvas1"`

```cs
# "Simple" canvas
myAnno.Target = new Canvas{ Id = "http://iiif-net.example/canvas1" };

# Full canvas with SerialiseTargetAsId set
myAnno.Target = new Canvas
    { 
        Id = "http://iiif-net.example/canvas1",
        Width = 100,
        Height = 200,
        SerialiseTargetAsId = true,
        Items = new List<AnnotationPage>{ new AnnotationPage() } 
    };
```

### ImageService2

To conform with https://iiif.io/api/image/2.1/#technical-properties, `ImageService2` has a `Profile` and `ProfileDescription` properties.

These 2 properties are serialised to (and deserialised from) as single `"profile"` property, e.g.:

```cs
var imageService = new ImageService2
{
    Id = "https://example",
    Profile = "http://iiif.io/api/image/2/level0.json",
    ProfileDescription = new ProfileDescription
    {
        Formats = new[] { "jpg", "png" }, 
        Qualities = new[] { "color" },
        Supports = new[] { "sizeByWhListed" }
    }
};
```

will output

```json
{
  "@id": "https://example",
  "@type": "ImageService2",
  "profile": [
    "http://iiif.io/api/image/2/level0.json",
    {
      "formats": ["jpg", "png"],
      "qualities": ["color"],
      "supports": ["sizeByWhListed"]
    }
  ]
}
```

## Serialisation

[newtonsoft](https://www.newtonsoft.com/json) is used to aid serialisation of objects. 

The `.Serialisation` namespace contains a number of custom `JsonConverter` implementations, some of which can be driven by decorating properties with attributes.

* `[ObjectIfSingle]` - used on `IEnumerable<T>` properties. Will render a single object if `.Count == 1`, else will render an array.
* `[RequiredOutput]` - used on `IEnumerable<T>` properties. Will output `[]` if collection is empty (default is to omit empty lists).
* `[CamelCaseEnumAttribute]` - use on an enum property to output value as camelCase (e.g. "MissingCredentials" -> "missingCredentials")

### Helpers

#### Serialisation

`IIIFSerialiserX` contains 2 extension methods for `JsonLdBase` that help with serialising / deserialising models. 

For string serialisation these are `AsJson` and `FromJson<TTarget>`:

```cs
// Serialisation
string jsonManifest = manifest.AsJson();

// Deserialisation
Manifest deserialisedManifest = jsonManifest.FromJson<Manifest>();
```

For `Stream` serialisation these are `AsJsonStream` and `FromJsonStream<TTarget>`:

```cs
// Serialisation
var memoryStream = new MemoryStream();
Stream jsonManifest = manifest.AsJsonStream(memoryStream);

// Deserialisation
Manifest deserialisedManifest = streamContainingManifest.FromJsonStream<Manifest>();
```

> [!Important]
> Full object deserialisation is incomplete - open an issue or PR if you find something unsupported.

#### HTML Markup Handling

`HtmlSanitiser` contains a `SanitiseHtml()` extension method on `string` to help sanitise HTML.

```cs
string original = "<p>my markup<div>invalid</div><p>";
string safe = original.SanitiseHtml();
```

See [IIIF Presentation 3.0 docs](https://iiif.io/api/presentation/3.0/#45-html-markup-in-property-values) for details on html markup.

> Note: The rules around markup differs between Presentation 2.1 and 3.0. This method uses 3.0 which permits a couple of tags not mentioned in 2.1 (`small`, `sub` and `sup`).
>

#### Manifest Traversal

See helpers in `IIIF.Presentation.V3.Traversal` to aid in traversal of `Manifests`, e.g.:

```cs
// Get all annotations from all annotationPages on all canvases in manifest (manifest.Items[*].Items[*].Items[*])
var annos = manifest.AllAnnotations();

// As above but for a specific type
var paintingAnnos = manifest.AllAnnotations<PaintingAnnotation>();
var generalAnnos = manifest.AllAnnotations<GeneralAnnotation>();

// Get all bodies from all paintingAnnos from all annotationPages on all canvases in manifest (manifest.Items[*].Items[*].Items[*].Body)
var paintingAnnoBodies = manifest.AllPaintingAnnoBodies();

// As above but for a specific type
var images = manifest.AllPaintingAnnoBodies<Image>();
var sounds = manifest.AllPaintingAnnoBodies<Sound>();
```

Note that all of the above are available per-Canvas too.

## Local Build

The `local_build.sh` bash script will build/test/pack for ease of testing.

```bash
# build version 1.0.0
$ bash local_build.sh

# build version 1.2.3
$ bash local_build.sh -v 1.2.3
```

## Deployment

New nuget package is published whenever a new version tag is pushed, using gitversion to derive the version number.

### Extensions

This library supports some extensions to the spec:

- `navPlace`
  - This is defined by earthbound geographic coordinates in the form of GeoJSON-LD.