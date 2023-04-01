# Academic for OS project

The project is multithread project, simulating go-kart racing. Each customer is a separate thread which attempts to pick a go-kart from buffer, and then races on the route.

Application uses Server-side Blazor. Thread run on the server, and orders frontend to refresh page in order to match current state of the threading logic.

#### Running application

##### Prerequisites
- Docker installed on the system 

###### Systems with make installed
Run following commands in command line, being in main directory of the project.

Build docker image, named so2_250147_image and container named so2_250147
```make build```

Run docker container. After successful run visit http://localhost:5224 to see the application.
```make run```

Stop the container.
```make stop```

###### Systems without make installed
Run following commands in command line, being in main directory of the project.

Build docker image, named so2_250147_image and container named so2_250147
```docker build . -t so2_250147_image```
```docker create -p 5224:5224 --name= so2_250147_image```

Run docker container. After successful run visit http://localhost:5224 to see the application.
```docker start so2_250147```

Stop the container.
```docker stop so2_250147```