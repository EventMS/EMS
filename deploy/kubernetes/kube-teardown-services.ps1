#Change namespace
kubectl config set-context --current --namespace=eventms;

#Delete all resources
# kubectl delete -f .\Database\.; # Uncomment if needed
kubectl delete -f .\ShareOwnerService\.;
kubectl delete -f .\StockTraderService\.;
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
# kubectl delete -f .\Ingress\.; # No need to delete the ingress always as it takes forever to get it up again

#Wait for services to be stopped, otherwise removal of images will fail
Start-Sleep -Second 1

#Remove all images
docker rmi duck2412/itonk-ap2-gr2-templateservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-shareownerservice:0.0.1;
docker rmi duck2412/itonk-ap2-gr2-stocktraderservice:0.0.1;
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
