# Pokedex
Welcome to Pokedex! This is a toy project, used to showcase how to implement a Web API by using ASP.NET core 8 with controllers.
This project contains the following API endpoints: 
 - `GET /pokemon/{pokemon-name}`: this endpoint returns basic information on the searched Pokemon. 
 - `GET /pokemon/translated/{pokemon-name}`: this endpoint is similar to the previous one, but in this case the Pokemon description is translated using the [FunTranslations API](https://funtranslations.com/)

If you want to learn more about the available endpoints, you can use the included Swagger UI which is available at the following endpoint: 

 `GET /swagger`

 **IMPORTANT**: Swagger UI is only included when the ASP.NET core environment is set to `Development`. Read the following section for more information on this. 

 ## How to run
 To run the project you need to install [Docker](https://www.docker.com/) and [Docker Compose](https://docs.docker.com/compose/). These tools are available for all the major operating systems. If you need help to install them on your machine, please refer to the [official documentation](https://docs.docker.com/get-started/get-docker/).

 To run the project, open a terminal in the project root folder and run the following command: 

 `docker compose up`

 By doing so, the following happens:
  - a Docker image is created, by using the project [Dockerfile](./Dockerfile)
  - a Docker container is created from the Docker image and then is started
  - port `3000` of the local machine is mapped to port `8080` of the Docker container. If port `3000` of the local machine is not available, you can change the port mapping from the project [compose.yaml file](./compose.yaml)
  - by default the ASP.NET core environment is set to `Production`. If you want to run a different environment (e.g.: `Development`) you can do that by changing the `environment` section of the project [compose.yaml file](./compose.yaml)

After the project is started, you can access the Pokemon endpoint by issuing the following HTTP request on your machine: 

 `GET http://localhost:3000/pokemon/{pokemon-name}`

To access the translated Pokemon endpoint, you can issue the following HTTP request on your machine:

 `GET http://localhost:3000/pokemon/translated/{pokemon-name}`

## Possible improvements
As explained above, this project is **not** production ready. Please, consider it a simple proof of concept. Several possible improvements are described in the following paragraphs. 

### TLS, authetication and authorization
This project does not include any support to TLS, authentication and authorization. 

Usually, in micro service architectures the internal communication between services is implemented by using plain HTTP requests and the TLS support is offloaded to the infrastructure and pushed at the edge of the system (e.g.: API gateway). This way, external callers can interact with the endpoints by using HTTPS at the edge of the system and there is no need to have TLS certificates for each and every micro service. 

A common approach to implement authentication in Web API services is using `OAuth2` access tokens issued by an identity server (e.g.: `Microsoft Entra ID`). Access tokens are usually in the form of JWT tokens. The API client authenticate itself witht he identity server and gets an access token in return, then the access token is included in each and every HTTP request to the Web API service inside the [Authorization HTTP request header](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Authorization).

Authorization policies are [fully supported in ASP.NET core](https://learn.microsoft.com/en-us/aspnet/core/security/authorization/policies?view=aspnetcore-9.0) and are based on the claims associated with the authenticated principal. 

### Caching the result of outbound HTTP requests
This project gets Pokemon data from the [PokéAPI](https://pokeapi.co/) and translations from the [FunTranslations API](https://funtranslations.com/).

The results obtained by calling these endpoints can safely be cache, since we don't expect Pokemon information and text translations to change often. By doing so, performance can be improved a lot, since a lot of HTTP requests can be saved and responses are read directly from a cache. The simplest caching solution is using the [ASP.NET core memory cache](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-9.0), a common solution to implement a distributed cache is using [Redis](https://redis.io/). 
A very nice library to implement advanced caching strategies in ASP.NET core applications is [Fusion Cache](https://github.com/ZiggyCreatures/FusionCache). 

Another benefit obtained by caching the responses from PokeAPI and FunTranslations API is to avoid hitting the rate-limit threshold, which is quite aggressive for the free version of these API services.

### Resiliency of the outbound HTTP requests
This project offers a very simple support to retry policies, which are applied in case of transient HTTP errors when calling the PokéAPI and the FunTranslations API. See [here](https://github.com/EnricoMassone/Pokedex/blob/c9a9b7aef95b82e8a01c151bd7d2811dc4d6c358/src/Pokedex.Infrastructure/DependencyInjectionConfiguration.cs#L36) and [here](https://github.com/EnricoMassone/Pokedex/blob/c9a9b7aef95b82e8a01c151bd7d2811dc4d6c358/src/Pokedex.Infrastructure/DependencyInjectionConfiguration.cs#L58) for more details on the implementation. 

A common approach is to combine retry policies with a circuit breaker, which is used to avoid overloading a server affected by temporary issues with incoming HTTP requests. By doing so, we can avoid sending HTTP requests to the PokéAPI and the FunTranslations API for a while, to let these services recover from transient errors and become available again.

### Logging and observability
This service offers only a very basic logging implementation, which is done using the [Serilog console sink](https://github.com/serilog/serilog-sinks-console).

In a real production environment it is better to store logs in a centralized log store, which is easily queryable. Common services used to do that are SQL Server, MongoDB or Elasticsearch. 

A further step to improve the observability of the system is using a dedicated APM (Application Performance Monitoring) service, to monitor the Web API service. Common services used to do that are Azure Application Insights and Datadog, among the others. APM services can act as centralized log stores, but they offer many other capabilities: 
 - a lot of useful metrics are collected (e.g.: CPU usage, memory usage, response time of incoming HTTP requests)
 - dashboards to visualize the collected metrics can be defined and modified over time
 - alerts can be set up, based on collected metrics and application performances
 - a detailed tracing of the incoming HTTP requests is available. This tracing highlights all the dependencies used to serve incoming requests (e.g.: database queries and requests to third-party Web API services) and their respective response time. This can be useful to detect bottlenecks and to improve application performance.

 ### Other improvements
 There are many other possible improvements: 
  - error handling and logging of the outbound HTTP requests can be improved. For instance, we can log the HTTP status code returned by each request and implement error handling strategies in case of unexpected response content types (e.g.: `text/plain` instead of `application/json`) and JSON content deserialization errors.
  - adding liveness and readiness endpoints to the service, which can be useful if the service will be hosted in Kubernetes. 
