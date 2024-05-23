@echo off
cd /d %~dp0

rd /s /q x64

dotnet restore SimpleVhd.ControlPanel
dotnet restore SimpleVhd.PE

dotnet publish SimpleVhd.ControlPanel --no-restore -c Debug -p=Platform=x64
dotnet publish SimpleVhd.PE --no-restore -c Debug -p=Platform=x64
