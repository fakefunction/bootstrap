@echo off
cls

set curr_dir=%cd%

chdir /D src\app

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

".paket/paket.exe" install 

dotnet build

chdir /D CalculatorLib.Function

dotnet restore
dotnet build
dotnet publish

chdir /D %curr_dir%

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

".paket/paket.exe" install

build.bat
