using System;
using System.Collections.Generic;
using IIIF.Presentation.V2;
using IIIF.Presentation.V3;
using Newtonsoft.Json;

namespace IIIF.Discovery.V1
{
    /// <summary>
    /// The Activities are the means of describing the changes that have occurred in the content provider’s system.
    /// </summary>
    /// <remarks>See https://iiif.io/api/discovery/1.0/#activities</remarks>
    public class Activity
    {
        [JsonProperty(Order = 2)]
        public string? Id { get; set; }
        
        [JsonProperty(Order = 3)]
        public ActivityType Type { get; set; }

        /// <summary>
        /// The IIIF resource that was affected by the Activity
        /// </summary>
        [JsonProperty(Order = 6)]
        public ActivityObject Object { get; set; }
        
        /// <summary>
        /// The new location of the IIIF resource, after it was affected by a Move activity.
        /// </summary>
        [JsonProperty(Order = 7)]
        public ActivityObject Target { get; set; }
        
        // TODO - formatter to xsd:datetime
        /// <summary>
        /// The time at which the Activity was finished. 
        /// </summary>
        [JsonProperty(Order = 10)]
        public DateTime? EndTime { get; set; }
        
        /// <summary>
        /// The time at which the Activity was started.
        /// </summary>
        [JsonProperty(Order = 11)]
        public DateTime? StartTime { get; set; }
        
        /// <summary>
        /// A short textual description of the Activity
        /// </summary>
        [JsonProperty(Order = 20)]
        public string? Summary { get; set; }

        /// <summary>
        /// The organization, person, or software agent that carried out the Activity.
        /// </summary>
        [JsonProperty(Order = 21)]
        public Actor? Actor { get; set; }
    }
    
    public class ActivityObject : IService
    {
        [JsonProperty(Order = 2)]
        public string? Id { get; set; }
        
        [JsonProperty(Order = 3)]
        public string? Type { get; }

        [JsonProperty(Order = 4)]
        public string? Canonical { get; set; }
        
        [JsonProperty(Order = 10)]
        public List<ExternalResource>? SeeAlso { get; set; }
        
        [JsonProperty(Order = 11)]
        public List<Agent>? Provider { get; set; }
    }

    public class Actor
    {
        [JsonProperty(Order = 2)]
        public string? Id { get; set; }
        
        [JsonProperty(Order = 3)]
        public ActorType Type { get; set; }
    }

    /// <summary>
    /// Valid values for activity Type
    /// </summary>
    /// <remarks>See: https://iiif.io/api/discovery/1.0/#type-activity</remarks>
    public enum ActivityType
    {
        /// <summary>
        /// The initial creation of the resource.
        /// </summary>
        Create,
        
        /// <summary>
        /// Any change to the resource.
        /// </summary>
        Update,
        
        /// <summary>
        /// The deletion of the resource, or its de-publication from the web.
        /// </summary>
        Delete,
        
        /// <summary>
        /// The re-publishing of the resource at a new URI, with the same content
        /// </summary>
        Move,
        
        /// <summary>
        /// The addition of an object to a stream, outside of any of the above types of activity, such as a third
        /// party aggregator adding resources from a newly discovered stream.
        /// </summary>
        Add,
        
        /// <summary>
        /// The removal of an object from a stream, outside of any of the above types of activity, such as a third
        /// party aggregator removing resources from a stream that are no longer considered to be in scope.
        /// </summary>
        Remove,
        
        /// <summary>
        /// The beginning of an activity to refresh the stream with the state of all of the resources.
        /// </summary>
        Refresh
    }

    public enum ActorType
    {
        Application,
        Organization,
        Person
    }
}