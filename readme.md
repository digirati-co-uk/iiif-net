# IIIF-net

This repo contains a collection of POCO's for modelling various IIIF APIs.

The different APIs are split by namespace containing name and version.

E.g. `IIIF.Auth.V1`, `IIIF.Presentation.V2`, `IIIF.Presentation.V3`.

## Serialisation

[newtonsoft](https://www.newtonsoft.com/json) is used to aid serialisation of objects. 

The `.Serialisation` namespace contains a number of custom `JsonConverter` implementations, some of which can be driven by decorating properties with attributes.

* `[ObjectIfSingle]` - used on `IEnumerable<T>` properties. Will render a single object if `.Count == 1`, else will render an array.
* `[RequiredOutput]` - used on `IEnumerable<T>` properties. Will output `[]` if collection is empty (default is to omit empty lists).

> NOTE: Deserialisation of models is currently not supported.

## Local Build

The `local_build.sh` bash script will build/test/pack for ease of testing.

```bash
# build version 1.0.0
$ bash local_build.sh

# build version 1.2.3
$ bash local_build.sh -v 1.2.3
```