[![.NET](https://github.com/Nordes/Flux.NewRelic/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Nordes/Flux.NewRelic/actions/workflows/dotnet.yml)
![badge](https://img.shields.io/endpoint?url=https://gist.githubusercontent.com/Nordes/2d25e1c74703ebd1cf0dbef7217e9d18/raw/799c8e8213a11c3ac487b6f1eca8b62618da3219/code-coverage.json)
         
# Flux.NewRelic
The goal of this project is to manage the release done by Flux in the Cluster by receiving WebHook post from the notification controller. 

**Why is the project in C# or even Dotnet and not in GO?** Just because... it could even be in F# if I wanted to. I just went with something quick to build. Probably in the future I will look forward to change the language and even the structure of the project. 

## Data sent
- New deployment
- TBD: Logs `Info` and `error` in general?

# Compatibility
- [X] >= 0.9.0, 0.9.1 <=

# Maturity
This project is not yet mature enough to use. So please wait... ;)

## Available Hooks
Those marked with an X are implementd. Those in progress will be also marked with ⌛.
- [ ] Bucket
- [ ] ⌛ GitRepository
- [ ] Kustomization
- [ ] HelmRelease
- [ ] HelmChart
- [ ] HelmRepository
- [ ] ImagePolicy
- [ ] ImageRepository
- [ ] ImageUpdateAutomation

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

## Implementation choices (for you)
Item with ⌛ are still in progress.

- Expected Store
  - [X] ⌛ InMemory: Lifetime of the pod (if there's one it's nice, two, might do something weird, but still works). It will be using the `appsettings.json` as main configuration. If it's a ConfigMap overriding it, it will automagically set the expected settings within your application.
  - [ ] Redis: Lifetime of redis. It will use 24h for the keys by default, so if you need to change a key, it's your bad.
  - [ ] Postgres: Manage your API Keys from there (no application API's for that)

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

- [X] Need to add API Key implementation (ref.: https://josef.codes/asp-net-core-protect-your-api-with-api-keys/)

# License
MIT
