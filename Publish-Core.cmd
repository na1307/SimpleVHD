dotnet restore %1

dotnet publish %1 --no-restore -c %2 -p=Platform=x64
dotnet publish %1 --no-restore -c %2 -p=Platform=ARM64
