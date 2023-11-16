#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# TO BUILD 
# docker build -t imagename/tag -f ProjectFolder/Dockerfile .
# TO PUSH
# docker push accountname/projectname:1.0.0

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["API/API.csproj", "API/"]
COPY ["BLL/BLL.csproj", "BLL/"]
COPY ["CORE/CORE.csproj", "CORE/"]
COPY ["DTO/DTO.csproj", "DTO/"]
COPY ["ENTITIES/ENTITIES.csproj", "ENTITIES/"]
COPY ["DAL/DAL.csproj", "DAL/"]
COPY ["MEDIATRS/MEDIATRS.csproj", "MEDIATRS/"]
COPY ["REFITS/REFITS.csproj", "REFITS/"]
RUN dotnet restore "API/API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]