@echo off
cd /d %~dp0

rd /s /q x64
rd /s /q ARM64

call Publish-Core.cmd SimpleVhd.ControlPanel Release
