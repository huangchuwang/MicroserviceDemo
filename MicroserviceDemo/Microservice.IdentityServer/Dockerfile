FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 修正：使用实际的文件夹名称
COPY ["Microservice.IdentityServer/Microservice.IdentityServer.csproj", "Microservice.IdentityServer/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]

RUN dotnet restore "Microservice.IdentityServer/Microservice.IdentityServer.csproj"

COPY . .

WORKDIR "/src/Microservice.IdentityServer"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Microservice.IdentityServer.dll"]