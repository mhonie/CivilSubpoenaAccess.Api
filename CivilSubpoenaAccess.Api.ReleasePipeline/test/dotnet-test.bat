@echo off

set PROJECT_NAME=%~1
set BUILD_CONFIGURATION=%~2

dotnet test %PROJECT_NAME% --configuration %BUILD_CONFIGURATION% /p:SkipCopyConfig=true