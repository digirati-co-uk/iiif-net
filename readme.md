# IIIF-net

This repo contains a collection of POCO's for modelling various IIIF APIs.

The different APIs are split by namespace containing name and version.

E.g. `IIIF.Auth.V1`, `IIIF.Presentation.V2`, `IIIF.Presentation.V3`.

## Serialisation

[newtonsoft](https://www.newtonsoft.com/json) is used to aid serialisation of objects. 

The `.Serialisation` namespace contains a number of custom `JsonConverter` implementations, some of which can be driven by decorating properties with attributes.

* `[ObjectIfSingle]` - used on `IEnumerable<T>` properties. Will render a single object if `.Count == 1`, else will render an array.
* `[RequiredOutput]` - used on `IEnumerable<T>` properties. Will output `[]` if collection is empty (default is to omit empty lists).
* `[CamelCaseEnumAttribute]` - use on an enum property to output value as camelCase (e.g. "MissingCredentials" -> "missingCredentials")

### Helpers

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

> Note: full object deserialisation is incomplete - open an issue or PR if you find an issue. 

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
