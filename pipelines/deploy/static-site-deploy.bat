@echo off

setlocal

REM ========================================
REM Arguments
REM ========================================
REM %1 = Source Path
REM %2 = Target Path
REM %3 = App Pool Name

set "SOURCE_PATH=%~1"
set "TARGET_PATH=%~2"
set "APP_POOL_NAME=%~3"

set "APPCMD=%windir%\System32\inetsrv\appcmd.exe"

if "%SOURCE_PATH%"=="" (
    echo ERROR: Source path not specified.
    exit /b 1
)

if "%TARGET_PATH%"=="" (
    echo ERROR: Target path not specified.
    exit /b 1
)

if "%APP_POOL_NAME%"=="" (
    echo ERROR: App pool name not specified.
    exit /b 1
)

if not exist "%SOURCE_PATH%\index.html" (
    echo ERROR: Published index.html was not found.
    exit /b 1
)

if not exist "%SOURCE_PATH%\web.config" (
    echo ERROR: Published web.config was not found.
    exit /b 1
)

echo.
echo ========================================
echo Deploying Website Artifacts
echo ========================================
echo.
echo Source:
echo %SOURCE_PATH%
echo.
echo Target:
echo %TARGET_PATH%
echo.
echo App Pool:
echo %APP_POOL_NAME%
echo.

echo Stopping application pool...

"%APPCMD%" stop apppool /apppool.name:"%APP_POOL_NAME%" >nul 2>&1

if not exist "%TARGET_PATH%" (
    mkdir "%TARGET_PATH%"

    if errorlevel 1 (
        echo ERROR: Failed to create target directory.
        exit /b 1
    )
)

echo.
echo Copying files...
echo.

robocopy "%SOURCE_PATH%" "%TARGET_PATH%" /MIR /R:2 /W:2

set "ROBOCOPY_EXIT_CODE=%ERRORLEVEL%"

echo.
echo Robocopy Exit Code: %ROBOCOPY_EXIT_CODE%
echo.

REM Robocopy:
REM 0-7 = Success
REM 8+  = Failure

if %ROBOCOPY_EXIT_CODE% GEQ 8 (
    echo ERROR: Deployment failed.
    exit /b %ROBOCOPY_EXIT_CODE%
)

echo Starting application pool...

"%APPCMD%" start apppool /apppool.name:"%APP_POOL_NAME%" >nul 2>&1

if errorlevel 1 (
    echo ERROR: Failed to start app pool.
    exit /b 1
)

echo Verifying application pool state...

"%APPCMD%" list apppool "%APP_POOL_NAME%" | findstr /i "state:Started" >nul

if errorlevel 1 (
    echo ERROR: Application pool failed to start.

    "%APPCMD%" list apppool "%APP_POOL_NAME%"

    exit /b 1
)

echo.
echo ========================================
echo Deployment Complete
echo ========================================
echo.

endlocal
exit /b 0