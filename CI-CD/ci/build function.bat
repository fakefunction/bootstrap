chdir /D src\app

.paket\paket.exe restore
if errorlevel 1 (
  exit /b %errorlevel%
)

".paket/paket.exe" install 

chdir /D CalculatorLib.Function

dotnet restore
dotnet build