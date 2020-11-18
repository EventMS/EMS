# Redeploys all microservices
#   1. Changes to the correct namespace
#   2. Deletes all services
#   3. Deletes all images
#   4. Rebuilds all images (very quick if not changes are made)
#   5. Spins up all images + updates the ingress

#Change namespace
kubectl config set-context --current --namespace=eventms;

#Delete all resources
# kubectl delete -f .\Namespaces\.; # No need to delete this namespace every time.
Get-ChildItem -Directory -Filter *Service | ForEach-Object {kubectl delete -f .\$_\lokal.;}
# kubectl apply -f .\Ingress\.; # No need to always delete the ingress as it takes forever to get it up again


#Deploy all resources
kubectl apply -f .\Namespaces\.;
Get-ChildItem -Directory -Filter *Service | ForEach-Object {kubectl apply -f .\$_\lokal\.;}
kubectl apply -f .\Ingress\.;