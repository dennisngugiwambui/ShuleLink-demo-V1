@echo off
echo Building ShuleLink in offline mode...
echo.

REM Stop any running processes that might lock files
echo Step 1: Stopping Visual Studio processes...
taskkill /f /im devenv.exe 2>nul
taskkill /f /im MSBuild.exe 2>nul
taskkill /f /im dotnet.exe 2>nul
timeout /t 2 >nul

REM Clean everything thoroughly
echo Step 2: Deep cleaning...
rmdir /s /q bin 2>nul
rmdir /s /q obj 2>nul

REM Try to build with existing packages
echo Step 3: Building with existing packages...
dotnet build -c Debug -f net8.0-android --no-restore --verbosity minimal

echo.
if %ERRORLEVEL% EQU 0 (
    echo ✅ Build completed successfully!
    echo.
    echo Your app is ready to run!
) else (
    echo ❌ Build failed.
    echo.
    echo Troubleshooting options:
    echo 1. Check your internet connection
    echo 2. Try building from Visual Studio instead
    echo 3. Use mobile hotspot if WiFi has issues
    echo 4. Contact your network administrator about NuGet access
)
echo.
pause
