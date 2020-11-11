$from=$args[0]
$to=$args[1]
Remove-Item $to -Recurse -Force
Copy-Item -Path $from -Destination $to -Recurse -Force
