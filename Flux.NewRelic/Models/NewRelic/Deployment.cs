using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Flux.NewRelic.DeploymentReporter.Models.NewRelic
{
    public class Deployment
    {
        /// <summary>
        /// Id is provided only on returned object from NewRelic
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Id { get; set; }

        /// <summary>
        /// Revision such as a git SHA
        /// </summary>
        [Required]
        public string Revision { get; set; }

        /// <summary>
        /// Changelog such as commit message or whatever (free string in theory).
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Changelog { get; set; }

        /// <summary>
        /// Description for the deployment
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Description { get; set; }

        /// <summary>
        /// The user (free text) pushing the deployment.
        /// </summary>
        /// <example>
        /// Flux-Bot
        /// </example>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string User { get; set; }

        /// <summary>
        /// The timestamp need to follow the ISO-8601 with the "Z", if Flux data is already ok, we will keep it as is.
        ///
        /// System.Text.Json can possibly do it: https://docs.microsoft.com/en-us/dotnet/standard/datetime/system-text-json-support
        /// </summary>
        /// <example>
        /// 2019-10-08T00:15:36Z
        /// </example>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string Timestamp { get; set; }

        /// <summary>
        /// Links is/are provided only when we receive data from NewRelic
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public Links Links { get; set; }
    }
}