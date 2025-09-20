@echo off
echo Cleaning and rebuilding ShuleLink...
echo.

REM Clean everything thoroughly
echo Step 1: Deep cleaning previous builds...
dotnet clean --verbosity normal
rmdir /s /q bin 2>nul
rmdir /s /q obj 2>nul

REM Clear NuGet cache
echo Step 2: Clearing NuGet cache...
dotnet nuget locals all --clear

REM Restore packages
echo Step 3: Restoring NuGet packages...
dotnet restore --force --no-cache

REM Build the project
echo Step 4: Building project...
dotnet build -c Debug -f net8.0-android --verbosity normal

echo.
if %ERRORLEVEL% EQU 0 (
    echo ✅ Build completed successfully!
    echo.
    echo Next steps:
    echo - Run: dotnet run -f net8.0-android
    echo - Or build APK: dotnet publish -c Release -f net8.0-android
) else (
    echo ❌ Build failed. Check the errors above.
    echo.
    echo Try these troubleshooting steps:
    echo 1. Restart Visual Studio
    echo 2. Clean solution in Visual Studio
    echo 3. Check Android SDK installation
)
echo.
pause
