# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 复制 csproj 文件 (注意路径，这里假设构建上下文是 src 根目录)
COPY ["Services/Identity/Identity.csproj", "Services/Identity/"]
COPY ["Common/SharedKernel/SharedKernel.csproj", "Common/SharedKernel/"]

# 还原依赖
RUN dotnet restore "Services/Identity/Identity.csproj"

# 复制所有源代码
COPY . .

# 编译并发布
WORKDIR "/src/Services/Identity"
RUN dotnet publish -c Release -o /app/publish

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Identity.dll"]