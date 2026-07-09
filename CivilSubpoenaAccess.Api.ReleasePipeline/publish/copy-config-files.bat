@echo off

setlocal enabledelayedexpansion

REM ========================================
REM Arguments
REM ========================================
REM %1 = Repository Root
REM %2 = Output Directory
REM %3 = Environment Name

set "PROJECT_DIR=%~1"
set "OUTPUT_DIRECTORY=%~2"
set "ENVIRONMENT_NAME=%~3"

echo Environment:
echo !ENVIRONMENT_NAME!

REM ========================================
REM Copy environment appsettings
REM ========================================

set "ENVIRONMENT_APPSETTINGS_PATH=%PROJECT_DIR%\appsettings.!ENVIRONMENT_NAME!.json"

if exist "!ENVIRONMENT_APPSETTINGS_PATH!" (
echo Copying appsettings.!ENVIRONMENT_NAME!.json...
copy /Y "!ENVIRONMENT_APPSETTINGS_PATH!" "%OUTPUT_DIRECTORY%"
) else (
echo Environment appsettings file not found:
echo !ENVIRONMENT_APPSETTINGS_PATH!
exit /b 1
)

echo Configuration file copy complete.

endlocal
