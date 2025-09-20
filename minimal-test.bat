@echo off
echo ========================================
echo ShuleLink Minimal Test Build
echo ========================================
echo.

echo This will create a minimal version for testing...
echo.

echo Backing up current App.xaml.cs...
copy "ShuleLink\App.xaml.cs" "ShuleLink\App.xaml.cs.backup"

echo Creating minimal App.xaml.cs...
echo namespace ShuleLink > ShuleLink\App.xaml.cs.minimal
echo { >> ShuleLink\App.xaml.cs.minimal
echo     public partial class App : Application >> ShuleLink\App.xaml.cs.minimal
echo     { >> ShuleLink\App.xaml.cs.minimal
echo         public App() >> ShuleLink\App.xaml.cs.minimal
echo         { >> ShuleLink\App.xaml.cs.minimal
echo             InitializeComponent(); >> ShuleLink\App.xaml.cs.minimal
echo             MainPage = new AppShell(); >> ShuleLink\App.xaml.cs.minimal
echo         } >> ShuleLink\App.xaml.cs.minimal
echo     } >> ShuleLink\App.xaml.cs.minimal
echo } >> ShuleLink\App.xaml.cs.minimal

echo.
echo To test minimal version:
echo 1. Replace App.xaml.cs with App.xaml.cs.minimal
echo 2. Build and test
echo 3. Restore App.xaml.cs.backup when done
echo.

pause
