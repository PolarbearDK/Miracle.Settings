param (
	[switch]$Pack = $true,
	
	[switch]$Push,
	
	[string]$PackagePrefix = "Miracle.Settings."
)

Set-StrictMode -Version latest
$ErrorActionPreference = "Stop"


if($pack) {
	& msbuild Miracle.Settings.csproj /p:Configuration='Release 4.0'
	if(!$?){throw "msbuild returned exit code $LASTEXITCODE"}
	& msbuild Miracle.Settings.csproj /p:Configuration='Release 4.5.2'
	if(!$?){throw "msbuild returned exit code $LASTEXITCODE"}
	& msbuild Miracle.Settings.csproj /p:Configuration='Release 4.6'
	if(!$?){throw "msbuild returned exit code $LASTEXITCODE"}

	& ..\.NuGet\NuGet.exe pack Miracle.Settings.csproj -IncludeReferencedProjects -Prop Configuration=Release
	if(!$?){throw "NuGet pack returned exit code $LASTEXITCODE"}
}

if($push) {
	$filename = Get-ChildItem "$packageprefix*" | Sort-Object LastWriteTime -Descending | Select -First 1
	& ..\.NuGet\NuGet.exe push "$filename" -Source https://www.nuget.org/api/v2/package
}
