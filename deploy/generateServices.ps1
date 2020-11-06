$from=$args[0]
$to=$args[1]

Get-ChildItem -Path kubernetes -Directory -Exclude Templa* | ForEach-Object {
$Name = $_.Name
$FileName = "gke"+$Name.toLower()
Remove-Item $FileName -Force
Copy-Item -Path gke-template -Destination $FileName -Force
& .\changeMapping.ps1 $_.Name
}

