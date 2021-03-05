FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine
WORKDIR /app
COPY ./publish .
EXPOSE 80

RUN chmod +x Flux.NewRelic.DeploymentReporter

ENTRYPOINT [ "./Flux.NewRelic.DeploymentReporter" ]