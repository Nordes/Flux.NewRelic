[![.NET](https://github.com/Nordes/Flux.NewRelic/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Nordes/Flux.NewRelic/actions/workflows/dotnet.yml)

# Flux.NewRelic
The goal of this project is to manage the release done by Flux in the Cluster by receiving WebHook post from the notification controller. 

**Why is the project in C# or even Dotnet and not in GO?** Just because... it could even be in F# if I wanted to. I just went with something quick to build. Probably in the future I will look forward to change the language and even the structure of the project. 

# Maturity
This project is not yet mature enough to use. So please wait... ;)

# Setup
...todo...

## NewRelic: How to get your application list (id)

```
# Basic request, you could also add a JQ to simply extract what you really need.
curl --location \
    --request GET 'https://api.newrelic.com/v2/applications.json' \
    --header 'X-Api-Key: [SOME-API-KEY]' \
    | jq '.applications[] | {name: .name, id: .id}'
```

## How to call our Hook
API Keys are defined in the `appsettings.json`. In Kubernetes, you can override the entire file using a _ConfigMap_. The AzureVault is not defined yet, but it could also be an option. The usage would require extra libraries to the project. Since I want to keep it simple for now, I might think about adding this feature later.

**Choice #1:** Use the `api_key` as part of the query string.
```
curl --location --request POST 'https://localhost:5001/hook?type=ImagePolicy&api_key=8f1e9594-55cc-44dc-b76a-e084cdd57d83' \
    --header 'Content-Type: application/json' \
    --data-raw '{"some":"thing"}'
```

**Choice #2:** Use the `X-Api-key` as part of the header.
```
curl --location --request POST 'https://localhost:5001/hook?type=ImagePolicy' \
    --header 'X-Api-key: 8f1e9594-55cc-44dc-b76a-e084cdd57d83' \
    --header 'Content-Type: application/json' \
    --data-raw '{"some":"thing"}'
```

# How to contribute?
Contact me or simply do a PR, it will be welcome.

- [ ] Need to add API Key implementation (ref.: https://josef.codes/asp-net-core-protect-your-api-with-api-keys/)

# License
MIT
