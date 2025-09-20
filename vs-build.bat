@echo off
echo Building ShuleLink using MSBuild...
echo.

REM Clean first
echo Step 1: Cleaning project...
dotnet clean --verbosity quiet

REM Build using MSBuild directly
echo Step 2: Building with MSBuild...
msbuild ShuleLink.sln -p:Configuration=Debug -p:Platform="Any CPU" -p:TargetFramework=net8.0-android -verbosity:minimal

echo.
if %ERRORLEVEL% EQU 0 (
    echo ✅ Build completed successfully using MSBuild!
    echo.
    echo Your ShuleLink app is ready!
    echo You can now run it from Visual Studio or deploy to device.
) else (
    echo ❌ MSBuild failed. Trying alternative approach...
    echo.
    echo Please try building directly in Visual Studio:
    echo 1. Open Visual Studio
    echo 2. File → Open → Project/Solution
    echo 3. Select ShuleLink.sln
    echo 4. Build → Rebuild Solution
)
echo.
pause
