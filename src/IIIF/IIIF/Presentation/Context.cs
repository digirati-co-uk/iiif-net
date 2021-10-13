using System;
using System.Collections.Generic;

namespace IIIF.Presentation
{
    /// <summary>
    /// Contains JSON-LD Contexts for IIIF Presentation API.
    /// </summary>
    public static class Context
    {
        /// <summary>
        /// JSON-LD context for IIIF presentation 2.
        /// </summary>
        public const string Presentation2Context = "http://iiif.io/api/presentation/2/context.json";

        /// <summary>
        /// JSON-LD context for IIIF presentation 3. 
        /// </summary>
        public const string Presentation3Context = "http://iiif.io/api/presentation/3/context.json";

        public static void EnsurePresentation3Context(this JsonLdBase resource)
        {
            resource.EnsureContext(Presentation3Context);
        }
        
        public static void EnsurePresentation2Context(this JsonLdBase resource)
        {
            resource.EnsureContext(Presentation2Context);
        }

        // The IIIF context must be last in the list, to override any that come before it.
        public static void EnsureContext(this JsonLdBase resource, string contextToEnsure)
        {
            if (resource.Context == null)
            {
                resource.Context = contextToEnsure;
                return;
            }

            List<string> workingContexts = new();
            if (resource.Context is List<string> existingContexts)
            {
                workingContexts = existingContexts;
            }

            if (resource.Context is string singleContext)
            {
                workingContexts = new List<string> { singleContext };
            }
            
            List<string> newContexts = new();
            bool requiresPresentation3Context = contextToEnsure == Presentation3Context;
            bool requiresPresentation2Context = contextToEnsure == Presentation2Context;
            foreach (string workingContext in workingContexts)
            {
                switch (workingContext)
                {
                    case Presentation3Context:
                        requiresPresentation3Context = true;
                        break;
                    case Presentation2Context:
                        requiresPresentation2Context = true;
                        break;
                    default:
                        newContexts.Add(workingContext);
                        break;
                }
            }

            // Add the new context to the list but not if it supposed to come last
            if (!newContexts.Contains(contextToEnsure) 
                && contextToEnsure != Presentation3Context 
                && contextToEnsure != Presentation2Context)
            {
                newContexts.Add(contextToEnsure);
            }

            if (requiresPresentation2Context && requiresPresentation3Context)
            {
                throw new InvalidOperationException(
                    "You cannot have Presentation 2 and Presentation 3 contexts in the same resource.");
            }
            // These have to come last
            if (requiresPresentation3Context)
            {
                newContexts.Add(Presentation3Context);
            }
            if (requiresPresentation2Context)
            {
                newContexts.Add(Presentation2Context);
            }

            // Now JSON-LD rules. The @context is the only Presentation 3 element that has this.
            if (newContexts.Count == 1)
            {
                resource.Context = newContexts[0];
            }
            else
            {
                resource.Context = newContexts;
            }
        }
    }
}