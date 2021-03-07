using System.Text.Json.Serialization;

namespace Flux.NewRelic.DeploymentReporter.Models.NewRelic
{
    public class Links
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Application { get; set; }
    }
}