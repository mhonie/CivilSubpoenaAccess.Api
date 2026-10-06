@echo off

setlocal

REM ========================================
REM Arguments
REM ========================================
REM %1 = Website Directory
REM %2 = Output Directory
REM %3 = Vite Base URL
REM %4 = API Base URL
REM %5 = Subpoena PDF Base URL

set "WEBSITE_DIR=%~1"
set "OUTPUT_DIRECTORY=%~2"
set "VITE_BASE_URL=%~3"
set "VITE_API_BASE_URL=%~4"
set "VITE_SUBPOENA_PDF_BASE_URL=%~5"

if "%WEBSITE_DIR%"=="" (
    echo ERROR: Website directory not specified.
    exit /b 1
)

if not exist "%WEBSITE_DIR%\package.json" (
    echo ERROR: package.json was not found in "%WEBSITE_DIR%".
    exit /b 1
)

if "%OUTPUT_DIRECTORY%"=="" (
    echo ERROR: Output directory not specified.
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

if not exist "%WEBSITE_DIR%\dist\index.html" (
    echo ERROR: Website build output was not created.
    exit /b 1
)

if not exist "%WEBSITE_DIR%\dist\web.config" (
    echo ERROR: web.config was not copied into the build output.
    exit /b 1
)

if exist "%OUTPUT_DIRECTORY%" (
    rmdir /s /q "%OUTPUT_DIRECTORY%"

    if errorlevel 1 (
        echo ERROR: Failed to clean output directory.
        exit /b 1
    )
)

mkdir "%OUTPUT_DIRECTORY%"

if errorlevel 1 (
    echo ERROR: Failed to create output directory.
    exit /b 1
)

robocopy ^
    "%WEBSITE_DIR%\dist" ^
    "%OUTPUT_DIRECTORY%" ^
    /MIR ^
    /R:2 ^
    /W:2

set "ROBOCOPY_EXIT_CODE=%ERRORLEVEL%"

REM Robocopy:
REM 0-7 = Success
REM 8+  = Failure

if %ROBOCOPY_EXIT_CODE% GEQ 8 (
    echo ERROR: Failed to copy the website into the artifact directory.
    exit /b %ROBOCOPY_EXIT_CODE%
)

if not exist "%OUTPUT_DIRECTORY%\index.html" (
    echo ERROR: Published index.html was not created.
    exit /b 1
)

if not exist "%OUTPUT_DIRECTORY%\web.config" (
    echo ERROR: Published web.config was not created.
    exit /b 1
)

echo.
echo Published website:
echo %OUTPUT_DIRECTORY%
echo.

dir "%OUTPUT_DIRECTORY%"

endlocal
exit /b 0