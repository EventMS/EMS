$from=$args[0]
$to=$args[1]

$files = Get-ChildItem -Path ../ -Recurse -Include Dockerfile

Foreach($file in $files){
	(Get-Content $file.FullName).replace('COPY "Services/'+$from+'/'+$from+'.API/'+$from+'.API.csproj" "Services/'+$from+'/'+$from+'.API/'+$from+'.API.csproj"', 'COPY "Services/'+$from+'/'+$from+'.API/'+$from+'.API.csproj" "Services/'+$from+'/'+$from+'.API/'+$from+'.API.csproj"
	COPY "Services/'+$to+'/'+$to+'.API/'+$to+'.API.csproj" "Services/'+$to+'/'+$to+'.API/'+$to+'.API.csproj"') | Set-Content $file.FullName
}
