using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace IIIF.ImageApi;

/// <summary>
/// Represents a IIIF image request in format:
/// {scheme}://{server}{/prefix}/{identifier}/{region}/{size}/{rotation}/{quality}.{format}
/// </summary>
/// <remarks>See https://iiif.io/api/image/3.0/#21-image-request-uri-syntax </remarks>
public class ImageRequest
{
    public string Prefix { get; set; }
    public string Identifier { get; set; }
    public bool IsBase { get; set; }
    public bool IsInformationRequest { get; set; }
    public RegionParameter Region { get; set; }
    public SizeParameter Size { get; set; }
    public RotationParameter Rotation { get; set; }
    public string Quality { get; set; }
    public string Format { get; set; }
    public string OriginalPath { get; set; }
    public string Scheme { get; set; }
    public string Server { get; set; }

    /// <summary>
    /// Full image request path, e.g. /0,0,400,400/100,/0/default.jpg
    /// </summary>
    public string? ImageRequestPath => OriginalPath?.Replace(Identifier, string.Empty);
    
    const string InfoJson = "info.json";

    /// <summary>
    /// Parses an image request path as a IIIF ImageRequest object
    /// </summary>
    /// <returns>A ImageRequest object</returns>
    /// <param name="path">The image request path</param>
    /// <param name="prefix">The image request prefix</param>
    /// <param name="validateSegments">If true, throws an ArgumentException if the image request contains empty values,
    /// or an invalid number of segments</param>
    public static ImageRequest Parse(string path, string prefix, bool validateSegments = false)
    {
        if (path[0] == '/') path = path[1..];

        if (prefix.Length > 0)
        {
            if (prefix[0] == '/') prefix = prefix[1..];
            if (prefix != path[..prefix.Length])
                throw new ArgumentException("Path does not start with prefix", nameof(prefix));
            path = path[prefix.Length..];
            if (path[0] == '/') path = path[1..];
        }

        var request = new ImageRequest { Prefix = prefix };
        
        var parts = path.Split('/');
        
        request.Identifier = parts[0];
        
        if (parts.Length == 1 || (parts.Length == 2 && parts[1] == string.Empty))
        {
            // likely the server will want to redirect this
            request.IsBase = true;
            return request;
        }

        if (parts[1] == InfoJson)
        {
            request.IsInformationRequest = true;
            return request;
        }
        
        if (validateSegments && (parts.Length != 5 || parts.Any(string.IsNullOrEmpty)))
        {
            throw new ArgumentException("Path contains empty or an invalid number of segments");
        }

        request.OriginalPath = path;
        request.Region = RegionParameter.Parse(parts[1]);
        request.Size = SizeParameter.Parse(parts[2]);
        request.Rotation = RotationParameter.Parse(parts[3]);
        var filenameParts = parts[4].Split('.');
        request.Quality = filenameParts[0];
        request.Format = filenameParts[1];

        return request;
    }
    
    /// <summary>
    /// Parses an image request path as a IIIF ImageRequest object
    /// </summary>
    /// <returns>A ImageRequest object</returns>
    /// <param name="path">The image request path</param>
    /// <param name="imageRequest"></param>
    /// <returns>true if able to parse path to <see cref="ImageRequest"/>, else false</returns>
    public static bool TryParse(string path, [NotNullWhen(true)] out ImageRequest? imageRequest)
    {
        try
        {
            if (path[0] == '/') path = path[1..];

            imageRequest = new ImageRequest();

            var parts = path.Split('/');

            var last = parts[^1];

            // If the last part is "info.json", this is info.json 
            if (last == InfoJson)
            {
                imageRequest.IsInformationRequest = true;
                SetCommonIdentifiers(parts.Length - 1, path, parts, imageRequest);
                return true;
            }

            // Check if the last part is {quality}.{format}
            var qfCandidate = last.Split('.');
            if (qfCandidate.Length == 2 && Qualities.All.Contains(qfCandidate[0]))
            {
                imageRequest.Quality = qfCandidate[0];
                imageRequest.Format = qfCandidate[1];

                // work back from here to build it
                imageRequest.Rotation = RotationParameter.Parse(parts[^2]);
                imageRequest.Size = SizeParameter.Parse(parts[^3]);
                imageRequest.Region = RegionParameter.Parse(parts[^4]);

                SetCommonIdentifiers(parts.Length - 4, path, parts, imageRequest, true);
                return true;
            }

            // If fall through, treat as base request
            imageRequest.IsBase = true;
            SetCommonIdentifiers(parts.Length, path, parts, imageRequest);
            return true;
        }
        catch (Exception)
        {
            imageRequest = null;
            return false;
        }
    }

    private static void SetCommonIdentifiers(int lastIndex, string path, string[] parts, ImageRequest request,
        bool setOriginal = false)
    {
        // Working back from last accessed index, is the {identifier}
        var candidate = parts[..lastIndex];
        request.Identifier = candidate[^1];
        var schemeDelimiter = path.IndexOf("://", StringComparison.Ordinal);
        
        // If there are elements before {identifier} then set {prefix}
        if (candidate.Length > 1)
        {
            var startIndex = schemeDelimiter > 0 ? 3 : 0;
            request.Prefix = candidate.Length == startIndex + 1
                ? null!
                : $"{string.Join("/", candidate[startIndex..^1])}/";
        }

        // Set {scheme}://{server}, if provided
        if (schemeDelimiter > 0)
        {
            request.Scheme = parts[0][..^1];
            request.Server = parts[2];
        }

        if (setOriginal)
        {
            var startIndex = path.IndexOf(request.Identifier, StringComparison.Ordinal);
            request.OriginalPath = path[startIndex..];
        }
    }

    private class Qualities
    {
        public const string Color = "color";
        public const string Gray = "gray";
        public const string Bitonal = "bitonal";
        public const string Default = "default";

        public static readonly string[] All = { Color, Gray, Bitonal, Default };
    }

    public override string ToString()
    {
        var basePath = $"{Prefix}{Identifier}";
        if (IsBase) return basePath;
        if (IsInformationRequest) return $"{basePath}/info.json";
        if (!string.IsNullOrEmpty(Scheme) && !string.IsNullOrEmpty(Server))
        {
            basePath = $"{Scheme}://{Server}/{basePath}";
        }
            
        return $"{basePath}/{Region}/{Size}/{Rotation}/{Quality}.{Format}";
    }
}