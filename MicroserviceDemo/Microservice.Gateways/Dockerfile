FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 网关没有引用 SharedKernel，所以比较简单，但保持上下文一致性
COPY ["Gateways/WebApiGateway/WebApiGateway.csproj", "Gateways/WebApiGateway/"]

RUN dotnet restore "Gateways/WebApiGateway/WebApiGateway.csproj"

COPY . .

WORKDIR "/src/Gateways/WebApiGateway"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "WebApiGateway.dll"]