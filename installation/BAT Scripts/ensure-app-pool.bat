@echo off

setlocal

set "APPCMD=%windir%\System32\inetsrv\appcmd.exe"

REM ========================================
REM Arguments
REM ========================================
REM %1 = App Pool Name

set "APP_POOL_NAME=%~1"

if "%APP_POOL_NAME%"=="" (
    echo ERROR: App pool name is empty.
    exit /b 1
)

echo.
echo ========================================
echo Ensuring App Pool
echo ========================================
echo.

"%APPCMD%" list apppool "%APP_POOL_NAME%" >nul 2>&1

if errorlevel 1 (
    echo Creating app pool "%APP_POOL_NAME%"...

    "%APPCMD%" add apppool /name:"%APP_POOL_NAME%"

    if errorlevel 1 (
        echo ERROR: Failed to create app pool.
        exit /b 1
    )
) else (
    echo App pool already exists.
)

echo Configuring app pool...

"%APPCMD%" set apppool "%APP_POOL_NAME%" /managedRuntimeVersion: /managedPipelineMode:Integrated

if errorlevel 1 exit /b 1

"%APPCMD%" set config -section:system.applicationHost/applicationPools "/[name='%APP_POOL_NAME%'].processModel.identityType:ApplicationPoolIdentity" /commit:apphost

if errorlevel 1 exit /b 1

"%APPCMD%" set config -section:system.applicationHost/applicationPools "/[name='%APP_POOL_NAME%'].startMode:AlwaysRunning" /commit:apphost

if errorlevel 1 exit /b 1

"%APPCMD%" set config -section:system.applicationHost/applicationPools "/[name='%APP_POOL_NAME%'].processModel.idleTimeout:00:00:00" /commit:apphost

if errorlevel 1 exit /b 1

echo.
echo Checking app pool state...

set "APP_POOL_STATE="

for /f %%S in ('%windir%\System32\inetsrv\appcmd.exe list apppool "%APP_POOL_NAME%" /text:state') do (
    set "APP_POOL_STATE=%%S"
)

echo Current state: %APP_POOL_STATE%

if /i "%APP_POOL_STATE%"=="Started" (
    echo App pool already started.
    goto :success
)

echo Starting app pool...

"%APPCMD%" start apppool /apppool.name:"%APP_POOL_NAME%"

if errorlevel 1 (
    echo ERROR: Failed to start app pool.
    exit /b 1
)

set "APP_POOL_STATE="

for /f %%S in ('%windir%\System32\inetsrv\appcmd.exe list apppool "%APP_POOL_NAME%" /text:state') do (
    set "APP_POOL_STATE=%%S"
)

echo Current state: %APP_POOL_STATE%

if /i not "%APP_POOL_STATE%"=="Started" (
    echo ERROR: Application pool failed to start.
    exit /b 1
)

:success

echo.
echo App pool configured successfully.
echo.

endlocal
exit /b 0