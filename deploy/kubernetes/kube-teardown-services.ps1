#Change namespace
kubectl config set-context --current --namespace=eventms;

#Delete all resources
# kubectl delete -f .\Database\.; # Uncomment if needed
Get-ChildItem -Directory -Filter *Service | ForEach-Object {kubectl delete -f .\$_\.;}
# kubectl delete -f .\Ingress\.; # No need to delete the ingress always as it takes forever to get it up again


