FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 修正：使用实际的文件夹名称
COPY ["Microservice.Gateways/Microservice.Gateways.csproj", "Microservice.Gateways/"]
# 注意：网关通常不直接引用 SharedKernel，如果你的项目引用了，请取消下面这行的注释
# COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]

RUN dotnet restore "Microservice.Gateways/Microservice.Gateways.csproj"

COPY . .

WORKDIR "/src/Microservice.Gateways"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Microservice.Gateways.dll"]