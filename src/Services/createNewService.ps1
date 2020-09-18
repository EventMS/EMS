$from=$args[0]
$to=$args[1]

& .\copy.ps1 $from $to
& .\changeMapping.ps1 $from $to
& .\changeDockerFiles.ps1 $from $to