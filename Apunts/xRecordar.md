# DotNet

### Crear nova app amd dotnet

```bash
    dotnet new console -n [NOMAPP]
```

### Ejecutar app

(Desde la ruta del projecte)
```bash
    dotnet run
```

### Afegir paquet a l'aplicació

```bash
    dotnet add package [NOMPAQUET]
```

# Docker

### Crear contenidor MSSQL

```bash
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=Patata1234" --name mssql -d mcr.microsoft.com/mssql/server:2025-latest
    docker ps # Per verificar que s'està executant el contenidor mssql
```

### Parar contenidor

```bash
    docker container stop [NOMCONTENIDOR]
```

### Engegar contenidor

```bash
    docker container start [NOMCONTENIDOR]
```


### Borrar contenidor

```bash
    docker container rm [NOMCONTENIDOR]
```