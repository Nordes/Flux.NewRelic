using System.Text.Json.Serialization;

namespace Flux.NewRelic.DeploymentReporter.Models
{
	public class Links
	{
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public int Application { get; set; }
	}
}