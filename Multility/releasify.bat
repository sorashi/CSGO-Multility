@echo off

echo "Releasify script started"

set soldir=%~1
set asver=%~2

cd "%soldir%packages"
dir /ad /b > paths
find /i "squirrel.windows" < paths > sqpath
find /i "NuGet.CommandLine" < paths > nupath
set /p sqpath=<sqpath
set /p nupath=<nupath
del paths
del sqpath
del nupath
cd "%soldir%"
if not exist "BuildPackages" mkdir "BuildPackages"
"packages\%nupath%\tools\NuGet.exe" pack "Multility.nuspec" -Version "%asver%" -Properties Configuration=Release -OutputDirectory "BuildPackages" -BasePath "%cd%"
"packages\%sqpath%\tools\Squirrel.exe" --releasify "BuildPackages\Multility.%asver%.nupkg"

echo "Releasify script ended"