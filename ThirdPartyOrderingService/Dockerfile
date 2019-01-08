#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM microsoft/dotnet:2.1-aspnetcore-runtime-nanoserver-1803 AS base
WORKDIR /app
EXPOSE 59290
EXPOSE 44349

FROM microsoft/dotnet:2.1-sdk-nanoserver-1803 AS build
WORKDIR /src
COPY ["ThirdPartyOrderingService/ThirdPartyOrderingService.csproj", "ThirdPartyOrderingService/"]
RUN dotnet restore "ThirdPartyOrderingService/ThirdPartyOrderingService.csproj"
COPY . .
WORKDIR "/src/ThirdPartyOrderingService"
RUN dotnet build "ThirdPartyOrderingService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "ThirdPartyOrderingService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ThirdPartyOrderingService.dll"]