@echo off

setlocal

set "WEBSITE_URL=%~1"

if "%WEBSITE_URL%"=="" (
    echo ERROR: Website URL not specified.
    exit /b 1
)

echo Verifying deployment...
echo URL: %WEBSITE_URL%

call :verify "%WEBSITE_URL%"

if errorlevel 1 exit /b 1

call :verify "%WEBSITE_URL%home"

if errorlevel 1 exit /b 1

echo.
echo Deployment verification succeeded.

endlocal
exit /b 0

:verify

set "VERIFY_URL=%~1"

echo Verifying URL: %VERIFY_URL%

curl ^
--silent ^
--show-error ^
--fail ^
--location ^
--output NUL ^
"%VERIFY_URL%"

if errorlevel 1 (
    echo.
    echo Deployment verification failed for %VERIFY_URL%.
    exit /b 1
)

exit /b 0