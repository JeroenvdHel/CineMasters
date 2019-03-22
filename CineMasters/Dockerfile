FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["CineMasters/CineMasters.csproj", "CineMasters/"]
RUN dotnet restore "CineMasters/CineMasters.csproj"
COPY . .
WORKDIR "/src/CineMasters"
RUN dotnet build "CineMasters.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CineMasters.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CineMasters.dll"]