@echo off
echo Building ShuleLink APK for sharing...
echo.

REM Clean previous builds
echo Cleaning previous builds...
dotnet clean

REM Build the APK in Release mode
echo Building APK in Release mode...
dotnet build -c Release -f net8.0-android

REM Publish the APK
echo Publishing APK...
dotnet publish -c Release -f net8.0-android -p:AndroidPackageFormat=apk

echo.
echo Build completed!
echo.
echo APK Location:
echo bin\Release\net8.0-android\publish\
echo.
echo You can now share the APK file with others!
echo The APK will be named: com.shulelink.education-Signed.apk
echo.
pause
