@echo off
cd /d %~dp0

rd /s /q x64
rd /s /q ARM64

dotnet restore SimpleVhd.ControlPanel
dotnet restore SimpleVhd.PE

dotnet publish SimpleVhd.ControlPanel --no-restore -c Release -p=Platform=x64
dotnet publish SimpleVhd.PE --no-restore -c Release -p=Platform=x64

dotnet publish SimpleVhd.ControlPanel --no-restore -c Release -p=Platform=ARM64
dotnet publish SimpleVhd.PE --no-restore -c Release -p=Platform=ARM64
