FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MusicApi.csproj", "./"]
RUN dotnet restore "MusicApi.csproj"
COPY . .
RUN dotnet build "MusicApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MusicApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "MusicApi.dll"]




# docker build --no-cache -t musicapi .
# docker run -d -p 8080:80 --name musicapi_container musicapi
# docker ps  ===> per controllare se il contenitore sta funzionando
# docker exec -it 9d0e5d625d7f /bin/bash ===> aprire una powershell nel container
# apt-get update && apt-get install -y curl
# curl http://localhost:8080/api/songs ===> controlla l'end point
 
 
