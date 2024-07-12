using System;
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

    /// <summary>
    /// Full image request path, e.g. /0,0,400,400/100,/0/default.jpg
    /// </summary>
    public string ImageRequestPath => OriginalPath.Replace(Identifier, string.Empty);

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

        if (parts[1] == "info.json")
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
}