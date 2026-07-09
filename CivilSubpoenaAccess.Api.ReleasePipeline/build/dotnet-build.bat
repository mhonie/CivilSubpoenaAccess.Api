@echo off

set BUILD_CONFIGURATION=%~1

dotnet build --configuration %BUILD_CONFIGURATION%