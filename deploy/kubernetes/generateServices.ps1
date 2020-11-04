$from=$args[0]
$to=$args[1]

Get-ChildItem -Path .\..\..\src\Services -Directory | ForEach-Object {
$_
#& .\copy.ps1 $from $to
#& .\changeMapping.ps1 $from $to
}

