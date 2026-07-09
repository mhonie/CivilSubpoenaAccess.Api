@echo off

setlocal

set HEALTH_CHECK_URL=%~1

echo Verifying deployment...
echo URL: %HEALTH_CHECK_URL%

curl ^
--silent ^
--show-error ^
--fail ^
"%HEALTH_CHECK_URL%"

if errorlevel 1 (
echo.
echo Deployment verification failed.
exit /b 1
)

echo.
echo Deployment verification succeeded.

endlocal
