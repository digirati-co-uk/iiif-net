#! /bin/bash

VERSION=1.0.0
while getopts v: flag
do
    case "${flag}" in
        v) VERSION=${OPTARG};;
    esac
done

cd ./src/IIIF

dotnet build IIIF.sln --configuration Release --no-restore
dotnet test --configuration=Release --no-restore --no-build --verbosity normal
dotnet pack ./IIIF/IIIF.csproj --configuration Release -p:Version=$VERSION --no-restore --no-build