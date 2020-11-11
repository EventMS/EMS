$from=$args[0]
$to=$args[1]

Get-ChildItem -Path .\..\..\src\Services -Directory -Exclude Templa* | ForEach-Object {
$ServiceName = $_.Name + "Service"
& .\copy.ps1 Template1Service $ServiceName
& .\changeMapping.ps1 $_.Name
}

