@echo off

setlocal

REM ========================================
REM Arguments
REM ========================================
REM %1 = Base URL
REM %2 = App Pool Name
REM %3 = Site Path

set BASE_URL=%~1
set APP_POOL_NAME=%~2
set SITE_PATH=%~3

echo ========================================
echo Configuring IIS
echo ========================================

call "%~dp0ensure-app-pool.bat" ^
    "%APP_POOL_NAME%"

if errorlevel 1 exit /b 1

call "%~dp0ensure-site.bat" ^
	"%BASE_URL%" ^
    "%APP_POOL_NAME%" ^
    "%SITE_PATH%"

if errorlevel 1 exit /b 1

echo IIS configuration complete.

endlocal