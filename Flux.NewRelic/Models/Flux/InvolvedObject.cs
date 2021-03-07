namespace Flux.NewRelic.DeploymentReporter.Models.Flux
{
    public class InvolvedObject
    {
        public Kind Kind { get; set; } // TODO <== Put as enum, we know the kind (GitRepository, ImagePolicy, ...)
        public string Namespace { get; set; }
        public string Name { get; set; }
        public string Uid { get; set; }
        public string ApiVersion { get; set; }
        public string ResourceVersion { get; set; }
    }
}
