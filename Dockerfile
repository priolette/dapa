FROM mcr.microsoft.com/dotnet/aspnet:7.0
COPY DAPA.Api/bin/Release/net.7.0/publish/ App/
WORKDIR /App
EXPOSE 80
ENTRYPOINT ["dotnet", "DAPA.Api.dll"]
