using System;
using System.Collections.Generic;
using System.Linq;

namespace IIIF;

public static class ContextHelper
{
    // This is a list of known IIIF contexts that must be last in Contexts list
    // Only 1 can be present in any given Context list
    private static List<string> knownContexts = new()
    {
        Presentation.Context.Presentation2Context,
        Presentation.Context.Presentation3Context,
        ImageApi.V2.ImageService2.Image2Context,
        ImageApi.V3.ImageService3.Image3Context,
    };
    
    /// <summary>
    /// Adds specified Context to list.
    /// The IIIF context must be last in the list, to override any that come before it.
    /// </summary>
    public static void EnsureContext(this JsonLdBase resource, string contextToEnsure)
    {
        if (resource.Context == null)
        {
            resource.Context = contextToEnsure;
            return;
        }
        
        var workingContexts = GetWorkingContexts(resource);
        if (workingContexts.Contains(contextToEnsure))
        {
            // Context already added - no-op
            SetContext(resource, workingContexts);
            return;
        }
        workingContexts.Add(contextToEnsure);

        if (workingContexts.Intersect(knownContexts).Count() > 1)
        {
            throw new InvalidOperationException(
                "You cannot have multiple IIIF contexts (Presentation or Image) in the same resource.");
        }

        var iiifContext = workingContexts.FirstOrDefault(wc => knownContexts.Contains(wc));

        // If we have an IIIF context it must be last in list
        if (!string.IsNullOrEmpty(iiifContext) && workingContexts.Count > 1)
        {
            workingContexts.Remove(iiifContext);
            workingContexts.Add(iiifContext);
        }

        // Now JSON-LD rules. The @context is the only Presentation 3 element that has this.
        SetContext(resource, workingContexts);
    }

    private static void SetContext(JsonLdBase resource, IReadOnlyList<string> workingContexts)
        => resource.Context = workingContexts.Count == 1 ? workingContexts[0] : workingContexts;

    private static List<string> GetWorkingContexts(JsonLdBase resource)
    {
        if (resource.Context is List<string> existingContexts) return existingContexts;
        
        if (resource.Context is string singleContext) return new List<string> { singleContext };

        return new List<string>(1);
    }
}