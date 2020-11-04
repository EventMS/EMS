# Run this to recreate the database.
# ALL DATA WILL BE LOST
# This takes a while which is why there is a seperate script for it

#Delete existing database server
kubectl delete -f .\Database\.;
kubectl delete -f .\MessageQueue\.;

#Create a clean database server that contains no databases
kubectl apply -f .\Database\.;
kubectl apply -f .\MessageQueue\.;