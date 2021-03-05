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

# How to contribute?
Contact me or simply do a PR, it will be welcome.

- [ ] Need to add API Key implementation (ref.: https://josef.codes/asp-net-core-protect-your-api-with-api-keys/)

# License
MIT