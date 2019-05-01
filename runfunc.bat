@echo off
cls

set curr_dir=%cd%

chdir /D src\app

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

".paket/paket.exe" install 

chdir /D CalculatorLib.Function

dotnet restore
dotnet build
dotnet publish

chdir /D bin\Debug\net462\publish

func host start

chdir /D %curr_dir%
