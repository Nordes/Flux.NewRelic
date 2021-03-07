using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flux.NewRelic.DeploymentReporter.Models.Flux
{
    public class Event
    {
        /// <summary>
        /// Flux object that this event is about.
        /// </summary>
        /// <example>
        /// ImagePolicy, GitRepository, ...
        /// </example>
        [Required]
        public InvolvedObject InvolvedObject { get; set; }

        /// <summary>
        /// Notification severity type of this event
        /// </summary>
        /// <example>
        /// info, error
        /// </example>
        [Required]
        public Severity Severity { get; set; }

        /// <summary>
        /// The time at which this event was recorded in ISO-8601 Format.
        /// </summary>
        /// <example>
        /// 2021-03-06T00:57:42Z
        /// </example>
        /// <remarks>
        /// We keep it as a string, since this will be reused as is for the job to be done in this application.
        /// </remarks>
        public string Timestamp { get; set; }

        /// <summary>
        /// A human-readable description of this event. Maximum length 39,000 characters
        /// </summary>
        /// <example>
        /// GitRepository: "Fetched revision: main/49fc0e14ad37d5a4918c572280118e2fb12ac6e8"
        /// ImagePolicy: "Latest image tag for \u0027some-acr.azurecr.io/container-name\u0027 resolved to: 1.2.3-develop.1023"
        /// ...
        /// </example>
        [Required]
        [MaxLength(39000)]
        public string Message { get; set; }

        /// <summary>
        /// A machine understandable string that gives the reason for the transition into the object's current status.
        /// </summary>
        /// <example>
        /// info
        /// </example>
        [Required]
        public string Reason { get; set; }

        /// <summary>
        /// [Optional]
        /// Metadata of this event, e.g. apply change set.
        /// 
        /// There's no example about what it could looks like. So far, we keep it to match the design.
        /// </summary>
        public IList<string> Metadata { get; set; }

        /// <summary>
        /// Name of the controller that emitted this event, e.g. `source-controller`.
        /// </summary>
        /// <example>
        /// source-controller
        /// </example>
        [Required]
        public string ReportingController { get; set; }

        /// <summary>
        /// [Optional]
        /// ID of the controller instance, e.g. `source-controller-xyzf`.
        /// </summary>
        public string ReportingInstance { get; set; }
    }
}
