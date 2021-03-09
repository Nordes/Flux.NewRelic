namespace Flux.NewRelic.DeploymentReporter.Models.Flux
{
    /// <summary>
    /// Relates to the FLUX - Custom Resource Definition (CRD) alerts.notification.toolkit.fluxcd.io
    /// </summary>
    public enum Kind
    {
        Unknown = 0,
        Bucket,
        GitRepository,
        Kustomization,
        HelmRelease,
        HelmChart,
        HelmRepository,
        ImagePolicy,
        ImageRepository,
        ImageUpdateAutomation,
    }
}
