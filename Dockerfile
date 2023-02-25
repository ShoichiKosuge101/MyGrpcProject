# 本番用(.NET6 runtime)
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
ENV ASPNETCORE_URLS=http://+:80;https://+:443
EXPOSE 80
EXPOSE 443

# 構築用(sdk)
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG DOTNET_SYSTEM_NET_HTTP_USESOCKETSHTTPHANDLER=0
WORKDIR /src
COPY ["./MyGrpcProject/MyGrpcProject.csproj", "MyGrpcProject/"]
RUN dotnet restore "./MyGrpcProject/MyGrpcProject.csproj"
COPY . .
RUN dotnet build "./MyGrpcProject/MyGrpcProject.csproj" -c Release -o /app/build

FROM build AS publish
WORKDIR /src
RUN dotnet publish "./MyGrpcProject/MyGrpcProject.csproj" -c Releasse -o /app/publish

# 本番用(runtime)
FROM runtime AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyGrpcProject.dll"]