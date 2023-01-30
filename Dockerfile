FROM mcr.microsoft.com/dotnet/sdk:7.0 as builder

WORKDIR /.
COPY . /.


WORKDIR /.
RUN dotnet restore

WORKDIR /.
RUN dotnet publish -c Release -o /out FunBooksAndVideos.Users.csproj

FROM mcr.microsoft.com/dotnet/aspnet:7.0

EXPOSE 82
WORKDIR /app
ENTRYPOINT [ "dotnet", "/app/FunBooksAndVideos.Users.dll" ]

COPY --from=builder /out/ .
