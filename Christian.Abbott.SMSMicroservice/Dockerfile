#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Christian.Abbott.SMSMicroservice/Christian.Abbott.SMSMicroservice.csproj", "Christian.Abbott.SMSMicroservice/"]
RUN dotnet restore "Christian.Abbott.SMSMicroservice/Christian.Abbott.SMSMicroservice.csproj"
COPY . .
WORKDIR "/src/Christian.Abbott.SMSMicroservice"
RUN dotnet build "Christian.Abbott.SMSMicroservice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Christian.Abbott.SMSMicroservice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Christian.Abbott.SMSMicroservice.dll"]