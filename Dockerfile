FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine

EXPOSE 80
WORKDIR /app

COPY ./publish .
RUN chmod +x Flux.NewRelic.DeploymentReporter

ENTRYPOINT [ "./Flux.NewRelic.DeploymentReporter" ]