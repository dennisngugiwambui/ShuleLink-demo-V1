@echo off
echo ========================================
echo ShuleLink Debug Build Script
echo ========================================
echo.

echo Cleaning previous builds...
dotnet clean
echo.

echo Building in Debug mode with verbose logging...
dotnet build -c Debug -v detailed
echo.

if %ERRORLEVEL% EQU 0 (
    echo ✅ Build successful!
    echo.
    echo To run the app and see debug logs:
    echo 1. Open Visual Studio
    echo 2. Set breakpoints if needed
    echo 3. Run in Debug mode
    echo 4. Check Debug Output window for detailed logs
    echo.
    echo Look for these debug markers:
    echo - 🚀 APP CONSTRUCTOR START
    echo - 🏠 MAINPAGE ONAPPEARING START  
    echo - 🚨 MAINTABS NAVIGATION DETECTED
    echo - ❌ Any error messages with stack traces
) else (
    echo ❌ Build failed! Check the errors above.
)

echo.
echo Press any key to exit...
pause > nul
