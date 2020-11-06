$ProjectName = "dark-balancer-294711"
gcloud builds submit --project=$ProjectName --config gke-namespaces
gcloud builds submit --project=$ProjectName --config gke-ingress
gcloud builds submit --project=$ProjectName --config gke-frontendservice