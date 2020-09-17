$from=$args[0]
$to=$args[1]

Get-ChildItem -Path $to -Recurse -Include $from*.cs, $from*.csproj | Rename-Item -NewName { $_.Name.replace($from,$to)} #Files
Get-ChildItem -Path $to -Recurse -Include $from* | Rename-Item -NewName { $_.Name.replace($from,$to)} #Directories and rest..

$files = Get-ChildItem -Path $to -Recurse -Include *.cs, *.csproj, *.json

Foreach($file in $files){
	(Get-Content $file.FullName).replace($from, $to) | Set-Content $file.FullName
}
