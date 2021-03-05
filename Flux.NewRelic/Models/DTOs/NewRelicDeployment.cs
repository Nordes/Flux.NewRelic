namespace Flux.NewRelic.DeploymentReporter.Models.DTOs 
{
    public class NewRelicDeployment 
    {
        public string Revision { get; set; }
        public string Changelog { get; set; }
        public string Description { get; set; }
        public string User { get; set; }

        /// <summary>The timestamp need to follow the ISO with the "Z", the best is to simply send the data we receive from the webhook.</summary>
        public string Timestamp { get; set; }
    }
}