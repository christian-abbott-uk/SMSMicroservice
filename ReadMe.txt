Christian Abbott M-KOPA Senior Backend Software Engineer – Take Home Exercise

Completed On 23/07/2022

How To Build:
Visual Studio
or
msbuild.exe Christian.Abbott.SMSMicroservice/Christian.Abbott.SMSMicroservice.csproj
	/t:rebuild
	/verbosity:quiet 
    /logger:FileLogger,Microsoft.Build.Engine;logfile=<filePath>

How to Run:
Build and mount DockerFile or Christian.Abbott.SMSMicroservice/bin/Debug/net6.0/Christian.Abbott.SMSMicroservice.exe
	- Running the code will run a set of dummy data to show the flow of data, main testing should be done through unit tests

How to Test:
Using Visual studio test editor or 
	dotnet test Christian.Abbott.SMSMicroservice -v n