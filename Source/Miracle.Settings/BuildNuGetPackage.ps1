param (
	[switch]$Pack = $true,
	
	[switch]$Push,
	
	[string]$PackagePrefix = "Miracle.Settings."
)

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"


if($pack) {
	& msbuild Miracle.Settings.csproj /p:Configuration=Release
	if(!$?){throw "msbuild returned exit code $LASTEXITCODE"}

	& ..\.NuGet\NuGet.exe pack Miracle.Settings.csproj -IncludeReferencedProjects -Prop Configuration=Release
	if(!$?){throw "NuGet pack returned exit code $LASTEXITCODE"}
}

if($push) {
	$filename = Get-ChildItem "$packageprefix*" | Sort-Object LastWriteTime -Descending | Select -First 1
	& ..\.NuGet\NuGet.exe push "$filename" -s http://nugetserver.miraclecloudcontrol.com/ 1FDB0C3A-392F-44D7-A516-95AFD0F42D6A
}
