FROM microsoft/dotnet:2.2-aspnetcore-runtime AS Base
WORKDIR /app
EXPOSE 80
EXPOSE 443


# Restore dotnet before build to allow for caching
FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /
RUN ls
COPY StyleCop.json .
COPY Conduit.CodeAnalysis.ruleset .
COPY Conduit.CodeAnalysis.targets .
WORKDIR /src
COPY /src .

# Build and copy the out to the container
RUN dotnet restore /src/Conduit.Api/Conduit.Api.csproj
# Copy source files and build
RUN dotnet build /src/Conduit.Api/Conduit.Api.csproj --no-restore -c Release
RUN dotnet publish /src/Conduit.Api/Conduit.Api.csproj --no-restore -c Release -o /app

# Copy compiled app to runtime container
FROM base AS final
COPY --from=build /app .
CMD ["dotnet", "Conduit.Api.dll"]
