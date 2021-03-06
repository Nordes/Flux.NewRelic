using System.Text.Json.Serialization;

namespace Flux.NewRelic.DeploymentReporter.Models
{
	public class NewRelicDeployment
	{
		/// <summary>
		/// Id is provided only on returned object from NewRelic
		/// </summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int Id { get; set; }

		public string Revision { get; set; }

		public string Changelog { get; set; }

		public string Description { get; set; }

		public string User { get; set; }

		/// <summary>
		/// The timestamp need to follow the ISO-8601 with the "Z", if Flux data is already ok, we will keep it as is.
		///
		/// System.Text.Json can possibly do it: https://docs.microsoft.com/en-us/dotnet/standard/datetime/system-text-json-support
		/// </summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public string Timestamp { get; set; }

		/// <summary>
		/// Links is/are provided only when we receive data from NewRelic
		/// </summary>
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public Links Links { get; set; }
	}
}