FROM microsoft/dotnet:2.2-aspnetcore-runtime AS Base
WORKDIR /app
EXPOSE 80/tcp
EXPOSE 443/tcp


# Copy source files to the daemon, run from parent directory so we have access to the required CA build files
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /
COPY StyleCop.json .
COPY Conduit.CodeAnalysis.ruleset .
COPY Conduit.CodeAnalysis.targets .
WORKDIR /src
COPY /src .

# Build and publish
RUN dotnet restore /src/Conduit.Api/Conduit.Api.csproj
RUN dotnet build /src/Conduit.Api/Conduit.Api.csproj --no-restore -c Release
RUN dotnet publish /src/Conduit.Api/Conduit.Api.csproj --no-restore -c Release -o /app

# Copy compiled app to runtime container
FROM base AS final
COPY --from=build /app .
CMD ["dotnet", "Conduit.Api.dll"]
