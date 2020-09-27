#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/TicketFlow.Web/TicketFlow.Web.csproj", "src/TicketFlow.Web/"]
COPY ["src/TicketFlow.Application/TicketFlow.Application.csproj", "src/TicketFlow.Application/"]
COPY ["src/TicketFlow.Infrastructure/TicketFlow.Infrastructure.csproj", "src/TicketFlow.Infrastructure/"]
COPY ["src/TicketFlow.Core/TicketFlow.Core.csproj", "src/TicketFlow.Core/"]
RUN dotnet restore "src/TicketFlow.Web/TicketFlow.Web.csproj"
COPY . .
WORKDIR "/src/src/TicketFlow.Web"
RUN dotnet build "TicketFlow.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TicketFlow.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TicketFlow.Web.dll"]