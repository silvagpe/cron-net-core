current_dir := $(shell pwd)
user := $(shell whoami)

nuget:			
	dotnet build -c Release 
	dotnet pack -c Release 
	#dotnet nuget push "bin/Release/*.nupkg" --source https://api.nuget.org/v3/index.json


