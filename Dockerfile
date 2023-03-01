FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "DripChip.Main/DripChip.Main.csproj"
WORKDIR "/src/DripChip.Main"
RUN dotnet build "DripChip.Main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DripChip.Main.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DripChip.Main.dll"]
