@echo off

setlocal

REM ========================================
REM Arguments
REM ========================================
REM %1 = Website Directory
REM %2 = Vite Base URL
REM %3 = API Base URL
REM %4 = Subpoena PDF Base URL

set "WEBSITE_DIR=%~1"
set "VITE_BASE_URL=%~2"
set "VITE_API_BASE_URL=%~3"
set "VITE_SUBPOENA_PDF_BASE_URL=%~4"

if "%WEBSITE_DIR%"=="" (
    echo ERROR: Website directory not specified.
    exit /b 1
)

if not exist "%WEBSITE_DIR%\package.json" (
    echo ERROR: package.json was not found in "%WEBSITE_DIR%".
    exit /b 1
)

if "%VITE_BASE_URL%"=="" (
    echo ERROR: Vite base URL not specified.
    exit /b 1
)

if "%VITE_API_BASE_URL%"=="" (
    echo ERROR: API base URL not specified.
    exit /b 1
)

if "%VITE_SUBPOENA_PDF_BASE_URL%"=="" (
    echo ERROR: Subpoena PDF base URL not specified.
    exit /b 1
)

pushd "%WEBSITE_DIR%"

call npm ci

if errorlevel 1 (
    popd
    exit /b 1
)

call npm run build -- --base "%VITE_BASE_URL%"

if errorlevel 1 (
    popd
    exit /b 1
)

popd

endlocal
exit /b 0