FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 修正：使用实际的文件夹名称
COPY ["ProductService/ProductService.csproj", "ProductService/"]
COPY ["SharedKernel/SharedKernel.csproj", "SharedKernel/"]

RUN dotnet restore "ProductService/ProductService.csproj"

COPY . .

WORKDIR "/src/ProductService"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ProductService.dll"]