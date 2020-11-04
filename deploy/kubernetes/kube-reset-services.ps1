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
kubectl delete -f .\StockTraderService\.;
kubectl delete -f .\ShareOwnerService\.;
kubectl delete -f .\StockService\.;
kubectl delete -f .\BrokerService\.;
kubectl delete -f .\FundService\.;
kubectl delete -f .\StockTraderNotificationService\.;
kubectl delete -f .\BuyOrderService\.;
kubectl delete -f .\SellOrderService\.;
kubectl delete -f .\OrderRepository\.;
kubectl delete -f .\TaxationRepository\.;
kubectl delete -f .\AddTaxationService\.;
kubectl delete -f .\DeleteTaxationService\.;
kubectl delete -f .\TaxationApiGateway\.;
kubectl delete -f .\SMSService\.;
kubectl delete -f .\EmailService\.;
#Does the same as the lines above.. Get all folders in directory, filters away those without ending on Service, and then runs the command on those. No need to maintain a list. 
#Get-ChildItem -Directory -Filter *Service | ForEach-Object {kubectl delete -f .\$_\.;}
# kubectl apply -f .\Ingress\.; # No need to always delete the ingress as it takes forever to get it up again

#Remove all images
docker rmi duck2412/itonk-ap2-gr2-stocktraderservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-shareownerservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-stockservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-brokerservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-fundservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-stocktradernotificationservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-buyorderservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-sellorderservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-orderrepository:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-taxationrepository:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-addtaxationservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-deletetaxationservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-taxationapigateway:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-smsservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-emailservice:0.0.1;

#Build all images
docker build -t duck2412/itonk-ap2-gr2-stocktraderservice:0.0.1 ..\StockTraderService\.;
docker build -t duck2412/itonk-ap2-gr2-shareownerservice:0.0.1 ..\ShareOwnerService\.;
docker build -t duck2412/itonk-ap2-gr2-stockservice:0.0.1 ..\StockService\.;
docker build -t duck2412/itonk-ap2-gr2-brokerservice:0.0.1 ..\BrokerService\.;
docker build -t duck2412/itonk-ap2-gr2-fundservice:0.0.1 ..\FundService\.;
docker build -t duck2412/itonk-ap2-gr2-stocktradernotificationservice:0.0.1 ..\StockTraderNotificationService\.;
docker build -t duck2412/itonk-ap2-gr2-buyorderservice:0.0.1 ..\BuyOrderService\.;
docker build -t duck2412/itonk-ap2-gr2-sellorderservice:0.0.1 ..\SellOrderService\.;
docker build -t duck2412/itonk-ap2-gr2-orderrepository:0.0.1 ..\OrderRepository\.;
docker build -t duck2412/itonk-ap2-gr2-taxationrepository:0.0.1 ..\TaxationRepository\.;
docker build -t duck2412/itonk-ap2-gr2-addtaxationservice:0.0.1 ..\AddTaxationService\.;
docker build -t duck2412/itonk-ap2-gr2-deletetaxationservice:0.0.1 ..\DeleteTaxationService\.;
docker build -t duck2412/itonk-ap2-gr2-taxationapigateway:0.0.1 ..\TaxationApiGateway\.;
docker build -t duck2412/itonk-ap2-gr2-smsservice:0.0.1 ..\SMSService\.;
docker build -t duck2412/itonk-ap2-gr2-emailservice:0.0.1 ..\EmailService\.;

#Deploy all resources
kubectl apply -f .\Namespaces\.;
kubectl apply -f .\StockTraderService\.;
kubectl apply -f .\ShareOwnerService\.;
kubectl apply -f .\StockService\.;
kubectl apply -f .\BrokerService\.;
kubectl apply -f .\FundService\.;
kubectl apply -f .\StockTraderNotificationService\.;
kubectl apply -f .\BuyOrderService\.;
kubectl apply -f .\SellOrderService\.;
kubectl apply -f .\OrderRepository\.;
kubectl apply -f .\TaxationRepository\.;
kubectl apply -f .\AddTaxationService\.;
kubectl apply -f .\DeleteTaxationService\.;
kubectl apply -f .\TaxationApiGateway\.;
kubectl apply -f .\SMSService\.;
kubectl apply -f .\EmailService\.;
kubectl apply -f .\Ingress\.;