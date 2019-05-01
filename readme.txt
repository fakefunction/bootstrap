first Download the latest paket.bootstrapper.exe 'https://github.com/fsprojects/Paket/releases/latest' into that directory.
=> run it to update paket exe
=> run ".paket/paket.exe" install 
This will fetch nuget.exe from nuget.org and also download an early version of NUnit that contains 
the NUnit runner. The edit to paket.dependencies does not replace the RestorePackages() step. 
The NUnit.CalculatorLib.Tests test project references the NUnit version 2.6.2 library, 
so we need that version too.


In the root of the project you will find a build.bat file:
If you run this batch file from the command line then the 
latest FAKE version will be downloaded from nuget.org and 
your first FAKE script (build.fsx) will be executed.


to run  function

1. dotnet new --install "Microsoft.Azure.WebJobs.ProjectTemplates"

since https://github.com/MicrosoftDocs/azure-docs/issues/12901

cd tp path
dotnet restore
dotnet build
dotnet publish
cd bin\debug\netstandard2.0\publish
func host start
or just execute run command from Visual Studio Code.


TO BUILD THINGS

===================
run ".paket/paket.exe" install at root
cd into src/app and run run ".paket/paket.exe" 

cd back into root
run build.bat
dc into src/app
dotnet restore then build
cd into .function
dotnet restore then build then publish
cd into bin\debug\netstandard2.0\publish OR \bin\Debug\net462\publish
func host start
or just execute run command from Visual Studio Code.