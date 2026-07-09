@echo off

setlocal

set "APPCMD=%windir%\System32\inetsrv\appcmd.exe"

REM ========================================
REM Arguments
REM ========================================
REM %1 = Base Url
REM %2 = App Pool Name
REM %3 = Physical Path

set "BASE_URL=%~1"
set "APP_POOL_NAME=%~2"
set "SITE_PATH=%~3"

echo.
echo ========================================
echo Ensuring IIS Application
echo ========================================
echo.

if "%BASE_URL%"=="" (
    echo ERROR: Base url not supplied.
    exit /b 1
)

if "%APP_POOL_NAME%"=="" (
    echo ERROR: App pool name not supplied.
    exit /b 1
)

if "%SITE_PATH%"=="" (
    echo ERROR: Physical path not supplied.
    exit /b 1
)

if not exist "%SITE_PATH%" (
    echo Creating application directory...
    mkdir "%SITE_PATH%"
)

echo Removing existing application...

"%APPCMD%" delete app "Default Web Site%BASE_URL%" >nul 2>&1

echo Creating application...

"%APPCMD%" add app /site.name:"Default Web Site" /path:%BASE_URL% /physicalPath:"%SITE_PATH%"

if errorlevel 1 (
    echo ERROR: Failed to create application.
    exit /b 1
)

echo Assigning application pool...

"%APPCMD%" set app "Default Web Site%BASE_URL%" /applicationPool:"%APP_POOL_NAME%"

if errorlevel 1 (
    echo ERROR: Failed to assign application pool.
    exit /b 1
)

echo Starting application pool...

"%APPCMD%" start apppool /apppool.name:"%APP_POOL_NAME%"

if errorlevel 1 (
    echo ERROR: Failed to start application pool.
    exit /b 1
)

echo.
echo Base Url         : %BASE_URL%
echo App Pool         : %APP_POOL_NAME%
echo Physical Path    : %SITE_PATH%

echo.
echo ========================================
echo IIS application configured successfully
echo ========================================

endlocal
exit /b 0