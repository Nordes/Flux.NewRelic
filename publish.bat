dotnet publish Flux.NewRelic/Flux.NewRelic.DeploymentReporter.csproj -r linux-musl-x64 -o publish --self-contained true /p:Version="0.1.2" /p:InformationalVersion="0.1.2-pre"

docker build . -t newrelic-hook-controller