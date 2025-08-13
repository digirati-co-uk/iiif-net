using System;
using System.Collections.Generic;
using System.Linq;
using AngleSharp.Dom;
using Ganss.Xss;

namespace IIIF.Presentation;

/// <summary>
/// Class to help in sanitising HTML markup for use in IIIF property values 
/// </summary>
/// <remarks>See https://iiif.io/api/presentation/3.0/#45-html-markup-in-property-values</remarks>
public static class HtmlSanitiser
{
    // Placeholder that is used to represent removed child tags, replaced with space later.
    // Intentionally not a space to retain any spaces included in source string
    private const string TagReplacement = "~||~";
    
    private static readonly HtmlSanitizerOptions HtmlSanitizerOptions = new()
    {
        AllowedTags = new HashSet<string> { "a", "b", "br", "i", "img", "p", "small", "span", "sub", "sup" },
        AllowedAttributes = new HashSet<string>(0),
        AllowedSchemes = new HashSet<string> { "http", "https", "mailto" },
        UriAttributes = new HashSet<string> { "href" },
    };

    private static readonly HtmlSanitizer Sanitizer = new(HtmlSanitizerOptions);
    
    private static readonly Dictionary<string, ISet<string>> ValidAttributesPerTag
        = new()
        {
            ["a"] = new HashSet<string> { "href" },
            ["img"] = new HashSet<string> { "src", "alt" },
        };

    // These are inline elements and should not have a space added
    // from https://www.w3.org/TR/2011/WD-html5-20110405/text-level-semantics.html
    private static readonly HashSet<string> InlineElements = new()
    {
        "a", "em", "strong", "small", "s", "cite", "q", "dfn", "abbr", "time", "code", "var", "samp", "kbd", "sub", "i",
        "b", "mark", "ruby", "rt", "rp", "bdi", "bdo", "span", "br", "wbr"
    };

    static HtmlSanitiser()
    {
        // NOTE - used HTML sanitiser lib doesn't allow tag-specific attributes, so subscribe to RemovingAttribute
        // events and cancel those that should be allowed
        Sanitizer.RemovingAttribute += (_, args) =>
        {
            // Attribute can also be removed if scheme isn't allowed 
            if (args.Reason != RemoveReason.NotAllowedAttribute) return;
            args.Cancel = ValidAttributesPerTag.TryGetValue(args.Tag.TagName.ToLower(), out var allowedAttributes)
                          && allowedAttributes.Contains(args.Attribute.Name.ToLower());
        };
        Sanitizer.RemovingTag += (_, args) =>
        {
            if (args.Tag.HasChildNodes)
            {
                /*
                 * This mimics native KeepChildNodes=true handling but that just removes tags, which can result in
                 * content being "grouped". This similar handling adds a temporary placeholder either side
                 * of replaced text nodes, these are then replaced with spaces later (see Sanitise()).
                 * 
                 * e.g. for "<ul><li>One</li><li>Two</li><li>Three Four</li><ul>"
                 * Native KeepChildNodes => "OneTwoThree Four"
                 * This custom handling (after further handling from caller) => "One Two Three Four"
                 */
                var childNodes = args.Tag.ChildNodes;
                if (childNodes.Length == 1 &&
                    childNodes[0].NodeType == NodeType.Text &&
                    !InlineElements.Contains(args.Tag.TagName.ToLower()))
                {
                    // If the child is text and current element is block-level, append placeholder to become space 
                    childNodes[0].TextContent = $"{TagReplacement}{childNodes[0].TextContent}{TagReplacement}";
                }

                args.Tag.Replace(childNodes.ToArray());
            }
            else
            {
                args.Tag.Remove();
            }
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
    /// <param name="ignoreNonHtml">
    /// If true, any strings that don't start/end with &lt;/&gt; are returned as-is. If false non-html strings will be
    /// wrapped
    /// </param>
    /// <param name="nonHtmlWrappingTag">
    /// Tag to wrap value in if it is not currently an HTML string (starts with &lt; and ends with &gt;). Only used if
    /// <see cref="ignoreNonHtml"/> is true
    /// </param>
    /// <returns>Sanitised markup value</returns>
    /// <remarks>
    /// To maintain appropriate spacing when replacing internal tags the string "~||~" is used as a temporary
    /// placeholder. If this string legitimately appears in the string it will be replaced.
    /// </remarks>
    public static string SanitiseHtml(this string propertyValue, bool ignoreNonHtml = true,
        string nonHtmlWrappingTag = "span")
    {
        if (string.IsNullOrEmpty(propertyValue) || ignoreNonHtml && !IsHtmlString(propertyValue)) return propertyValue;

        var workingString = Sanitise(propertyValue.Trim());

        if (string.IsNullOrEmpty(workingString) || IsHtmlString(workingString)) return workingString;
        
        if (!HtmlSanitizerOptions.AllowedTags.Contains(nonHtmlWrappingTag))
        {
            throw new ArgumentException(
                $"Tag provided is not allowed. Must be one of: {string.Join(",", HtmlSanitizerOptions.AllowedTags)}",
                nameof(nonHtmlWrappingTag));
        }
        workingString = $"<{nonHtmlWrappingTag}>{workingString}</{nonHtmlWrappingTag}>";

        return Sanitise(workingString);
    }

    /// <summary>
    /// Sanitise provided text, removing any invalid tags/attributes etc
    /// </summary>
    private static string Sanitise(string toSanitise)
    {
        var workingString = Sanitizer.Sanitize(toSanitise);
        
        // At this point "<span><ul><li>One</li><li>Two</li><li>Three Four</li><ul></span>" would
        // be "<span>~||~One~||~~||~Two~||~~||~Three Four~||~</span>", clean that up to
        // "<span>One Two Three Four</span>"
        return workingString
            .Replace($"{TagReplacement}<", "<")
            .Replace($">{TagReplacement}", ">")
            .Replace($"{TagReplacement}{TagReplacement}", TagReplacement)
            .Replace(TagReplacement, " ")
            .Trim();
    }

    private static bool IsHtmlString(string candidate) 
        => candidate[0] == '<' && candidate[^1] == '>';
}