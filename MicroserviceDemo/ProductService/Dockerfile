FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["Services/Product/Product.csproj", "Services/Product/"]
COPY ["Common/SharedKernel/SharedKernel.csproj", "Common/SharedKernel/"]

RUN dotnet restore "Services/Product/Product.csproj"

COPY . .

WORKDIR "/src/Services/Product"
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Product.dll"]