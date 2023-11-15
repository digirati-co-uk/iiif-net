using System.Collections.Generic;
using Ganss.Xss;

namespace IIIF.Presentation;

/// <summary>
/// Class to help in sanitising HTML markup for use in IIIF property values 
/// </summary>
/// <remarks>See https://iiif.io/api/presentation/3.0/#45-html-markup-in-property-values</remarks>
public static class HtmlSanitiser
{
    private static readonly HtmlSanitizerOptions HtmlSanitizerOptions = new()
    {
        AllowedTags = new HashSet<string> { "a", "b", "br", "i", "img", "p", "small", "span", "sub", "sup" },
        AllowedAttributes = new HashSet<string>(0),
        AllowedSchemes = new HashSet<string> { "http", "https", "mailto" },
        UriAttributes = new HashSet<string> { "href" }
    };
    
    private static readonly HtmlSanitizer Sanitizer = new(HtmlSanitizerOptions);
    
    private static readonly Dictionary<string, ISet<string>> ValidAttributesPerTag
        = new()
        {
            ["a"] = new HashSet<string> { "href" },
            ["img"] = new HashSet<string> { "src", "alt" },
        };

    static HtmlSanitiser()
    {
        // NOTE - used HTML sanitiser lib doesn't allow tag-specific attributes so subscribe to RemovingAttribute
        // events and cancel those that should be allowed
        Sanitizer.RemovingAttribute += (sender, args) =>
        {
            // Attribute can also be removed if scheme isn't allowed 
            if (args.Reason != RemoveReason.NotAllowedAttribute) return;
            args.Cancel = ValidAttributesPerTag.TryGetValue(args.Tag.TagName.ToLower(), out var allowedAttributes)
                          && allowedAttributes.Contains(args.Attribute.Name.ToLower());
        };
    }
    
    
    /// <summary>
    /// Sanitise markup to meet requirements in IIIF spec. This will
    ///
    ///  * Remove all tags except: a, b, br, i, img, p, small, span, sub and sup
    ///  * Remove all attributes other than href on the a tag, src and alt on the img tag
    ///  * Remove all href attributes that start with the strings other than “http:”, “https:”, and “mailto:”
    ///  * CData sections
    ///  * XML comments
    ///  * Processing instructions
    ///  * Strip whitespace from either side of HTML string
    ///
    /// see https://iiif.io/api/presentation/3.0/#45-html-markup-in-property-values
    /// </summary>
    /// <param name="propertyValue">Value to be sanitised</param>
    /// <param name="nonHtmlWrappingTag">
    /// Tag to wrap value in if it is not currently an HTML string (starts with &lt; and ends with &gt;)
    /// </param>
    /// <returns>Sanitised markup value</returns>
    public static string SanitiseHtml(this string propertyValue, string nonHtmlWrappingTag = "span") 
    {
        if (string.IsNullOrEmpty(propertyValue)) return propertyValue;

        var workingString = Sanitizer.Sanitize(propertyValue.Trim());
        
        if (IsHtmlString(workingString))
        {
            workingString = $"<{nonHtmlWrappingTag}>{workingString}</{nonHtmlWrappingTag}>";
        }

        return workingString;
    }

    private static bool IsHtmlString(string workingString) => workingString[0] != '<' || workingString[^1] != '>';
}