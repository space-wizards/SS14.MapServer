# Quickstart

%project-name% can automatically pull a SS14 git repository, build it and run the map renderer.
It also takes care of managing and hosting map images.

It's mainly used in combination with the map viewer, which lists and displays maps from a specific branch:
[https://github.com/space-wizards/ss14.mapviewer](https://github.com/space-wizards/ss14.mapviewer)

The map viewer for Official SS14 servers can be found here: [https://map.spacestation14.com/](https://map.spacestation14.com/)

The map server can also post map images on PRs that modify map files. That requires setting up a Github app.

> **The map server is not required for running the viewer**
>
> You can run the viewer by serving the map images and the required json files as static files
>
{style="note"}

## Requirements

There are some requirements for running a map server instance.
With the manual rendering setup you can trigger rendering map images by making an api request.
If you want automatic rendering and map images under PRs changing map files you'll need to set up a Github app.

### Manual rendering and hosting maps only:
- A public git repository
  - Private submodules are supported but the content repo needs to be public
- Docker or .net 8 or higher
- A domain (for https)
- A reverse proxy is recommended
- Postgres database

### Automatic rendering, hosting and comments on PRs:
- All of the above requirements
- The repo needs to be on Github
- [A github app](https://docs.github.com/en/apps/creating-github-apps/registering-a-github-app/registering-a-github-app)

## Setup - Docker compose

There is a docker image provided for %project-name%:  
[https://github.com/space-wizards/SS14.MapServer/pkgs/container/ss14.mapserver](https://github.com/space-wizards/SS14.MapServer/pkgs/container/ss14.mapserver)

````yaml
# Example docker compose file
version: '3.3'
services:
  # The ss14 map server container doesn't support https on its own. 
  # Please use a reverse proxy
  ss14mapserver:
    image: ghcr.io/space-wizards/ss14.mapserver:latest
    volumes:
      - ./appsettings.yaml:/app/appsettings.yaml
      - ./private-key.pem:/app/private-key.pem
      - ./files:/app/data
      - ./build:/app/build
    ports:
      - 5218:80 # Replace 5218 with the port you need
    # Disables core dumps to prevent map renderer crashes from filling
    # up your hard drive with over a gigabyte per dump
    ulimits:
      core:
        hard: 0
        soft: 0
  map_database:
    image: postgres:latest
    environment:
      # Replace postgres with a randomly generated password
      - POSTGRES_PASSWORD=postgres
    volumes:
      - ./data:/var/lib/postgresql/data
      - ./init.sql:/docker-entrypoint-initdb.d/init.sql
````
{collapsible="true"}

## Setup - Manual

> Don't set %project-name% up without docker while using local mode and having automated builds enabled.  
> You run the risk of malicious code making its way into the content repo and being executed on the host machine.
{style="warning"}

*Todo: setup guide for running the map server without docker*

## Reverse proxy
It is recommended to run a reverse proxy in front of %project-name%.
The official map renderer instance is using [Caddy](https://caddyserver.com/)

### Example Caddy configuration
````
(cors) {
        @origin{args.0} header Origin {args.0}
        header @origin{args.0} Access-Control-Allow-Origin "{args.0}"
        header @origin{args.0} Access-Control-Allow-Headers "content-type, x-requested-with"
        header @origin{args.0} Vary Origin
}

mapserver.tanukij.dev {
  encode zstd gzip
  reverse_proxy 127.0.0.1:8956

  import cors https://maps14.tanukij.dev
}
````


## Configuration

