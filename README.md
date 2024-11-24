# Pokedex
Welcome to Pokedex! This is a toy project, used to showcase how to implement a Web API by using ASP.NET core 8 with Controllers.
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
  - port 3000 of the local machine is mapped to port 8080 of the Docker container. If port 3000 of the local machine is not available, you can change the port mapping from the project [compose.yaml file](./compose.yaml)
  - by default the ASP.NET core environment is set to `Production`. If you want to run a different environment (e.g.: `Development`) you can do that by changing the `environment` section of the project [compose.yaml file](./compose.yaml)

After the project is started, you can access the Pokemon endpoint by issuing the following HTTP request on your machine: 

 `GET http://localhost:3000/pokemon/{pokemon-name}`

To access the translated Pokemon endpoint, you can issue the following HTTP request on your machine:

 `GET http://localhost:3000/pokemon/translated/{pokemon-name}`

## Possible improvements to make this project production ready