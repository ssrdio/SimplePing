#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update -y
RUN apt-get install -y iputils-ping
WORKDIR /app
COPY flag.txt /flag.txt


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
COPY . .
RUN dotnet restore "src/SimpleLsSample/SimpleLsSample.csproj"
RUN dotnet build "src/SimpleLsSample/SimpleLsSample.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/SimpleLsSample/SimpleLsSample.csproj" -c Release -o /app/publish 

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN apt update && apt install -y supervisor ssh vim nano python python3-pip sudo && \
	echo 'PubkeyAuthentication yes\nPermitRootLogin yes' >> /etc/ssh/sshd_config && \
	useradd -m -s /bin/bash SERVICE_USER && \
	echo 'root:$y$j9T$a9fWIG9ZUPKJJxiESos.s1$p4/20kcQ/bwcH7HcUxJCYJxWIPVg41SO/ei3KZkmun.' | chpasswd -e && \
	echo 'SERVICE_USER:$y$j9T$mI3H59L.PjbAVC.17HOb90$UOITBAjW8Uh9kKBg46CNa0PTlrDDUUioHqGY/xiAHq2' | chpasswd -e && \
  mkdir -p /run/sshd

COPY ./src/SimpleLsSample/config/supervisord.conf /etc/supervisord.conf
COPY ./src/SimpleLsSample/update.py /app/update.py
COPY ./docker-entrypoint.sh /app/docker-entrypoint.sh
COPY ./requirements.txt /app/requirements.txt
RUN chmod 664 /flag.txt && \
    chmod +x /app/docker-entrypoint.sh && \
    chown :SERVICE_USER /flag.txt && \
    pip install -r requirements.txt

ENTRYPOINT ["/app/docker-entrypoint.sh"]
