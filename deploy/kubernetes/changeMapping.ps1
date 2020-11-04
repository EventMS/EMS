$serviceName=$args[0]
$folderName=$serviceName + "Service"
$serviceName = $serviceName.toLower()

Get-ChildItem -Path $folderName -Recurse -Include *.yaml | Rename-Item -NewName { $_.Name.replace("template1",$serviceName)} #Files

$files = Get-ChildItem -Path $folderName -Recurse -Include *.yaml
Foreach($file in $files){
	(Get-Content $file.FullName).replace("template1", $serviceName) | Set-Content $file.FullName
}
