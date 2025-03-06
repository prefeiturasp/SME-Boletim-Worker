FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/SME.SERAp.Boletim.Worker/SME.SERAp.Boletim.Worker.csproj", "SME.SERAp.Boletim.Worker/"]
COPY ["src/SME.SERAp.Boletim.IoC/SME.SERAp.Boletim.IoC.csproj", "SME.SERAp.Boletim.IoC/"]
COPY ["src/SME.SERAp.Boletim.Aplicacao/SME.SERAp.Boletim.Aplicacao.csproj", "SME.SERAp.Boletim.Aplicacao/"]
COPY ["src/SME.SERAp.Boletim.Infra/SME.SERAp.Boletim.Infra.csproj", "SME.SERAp.Boletim.Infra/"]
COPY ["src/SME.SERAp.Boletim.Dados/SME.SERAp.Boletim.Dados.csproj", "SME.SERAp.Boletim.Dados/"]
COPY ["src/SME.SERAp.Boletim.Dominio/SME.SERAp.Boletim.Dominio.csproj", "SME.SERAp.Boletim.Dominio/"]
RUN dotnet restore "SME.SERAp.Boletim.Worker/SME.SERAp.Boletim.Worker.csproj"
COPY . .
WORKDIR "/src/src/SME.SERAp.Boletim.Worker"
RUN dotnet build "SME.SERAp.Boletim.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SME.SERAp.Boletim.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV TZ=America/Sao_Paulo

RUN apt-get update \
  && apt-get install -yq tzdata locales -y \
  && dpkg-reconfigure --frontend noninteractive tzdata \ 
  && locale-gen en_US.UTF-8 \
  && apt-get install -y --allow-unauthenticated \
  libc6-dev \
  libgdiplus \
  libx11-dev \
  && rm -rf /var/lib/apt/lists/*

ENTRYPOINT ["dotnet", "SME.SERAp.Boletim.Worker.dll"]