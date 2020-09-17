$from=$args[0]
$to=$args[1]
Copy-Item -Path $from -Destination $to -Recurse
