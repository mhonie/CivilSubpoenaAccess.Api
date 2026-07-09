@echo off

echo Setting environment variables...

setx EFILING_DATABASE_USER_NAME "%~1" /M
setx EFILING_DATABASE_PASSWORD "%~2" /M
setx EFILING_DATABASE_SERVER_NAME "%~3" /M
setx EFILING_DATABASE_PORT "%~4" /M
setx EFILING_DATABASE_NAME "%~5" /M

echo Environment variables set