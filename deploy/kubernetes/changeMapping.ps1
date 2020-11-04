$serviceName=$args[0]
$folderName=$serviceName + "Service"
$serviceName = $serviceName.substring(0,1).toLower() + $serviceName.substring(1)

Get-ChildItem -Path $folderName -Recurse -Include *.yaml | Rename-Item -NewName { $_.Name.replace("template1",$serviceName)} #Files

$files = Get-ChildItem -Path $folderName -Recurse -Include *.yaml
Foreach($file in $files){
	(Get-Content $file.FullName).replace("template1", $serviceName) | Set-Content $file.FullName
}
