@echo off

setlocal

REM ========================================
REM Arguments
REM ========================================
REM %1 = Config Directory

set "CONFIG_DIR=%~1"

if "%CONFIG_DIR%"=="" (
    echo ERROR: Config directory not specified.
    exit /b 1
)

REM ========================================
REM Delete .env
REM ========================================

if exist "%CONFIG_DIR%\.env" (
    echo Deleting .env...

    del /q "%CONFIG_DIR%\.env"
)

REM ========================================
REM Delete environment-specific appsettings
REM ========================================

echo Deleting environment-specific appsettings files...

for %%f in (%CONFIG_DIR%\appsettings.*.json) do (
    if exist "%%~f" (
        echo Deleting %%~nxf...

        del /q "%%~f"
    )
)

echo Configuration cleanup complete.

endlocal

exit /b 0