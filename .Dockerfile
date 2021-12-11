FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# RUN dotnet tool install -g dotnet-aspnet-codegenerator --version 6.0.0-rc.1.21464.1
# RUN dotnet tool install --global dotnet-ef --version 6.0.0-rc.1.21452.10
# RUN export PATH=$HOME/.dotnet/tools:$PATH

# copy csproj and restore as distinct layers
COPY *.csproj .
RUN dotnet restore

# copy and publish app and libraries
COPY . .
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app .
COPY SQLite ./SQLite
ENTRYPOINT ["dotnet", "miniproject.dll"]