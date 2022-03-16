# k8s.keda

docker run -d -p 15672:15672 -p 5672:5672 rabbitmq:3-management

user:  guest
senha: guest

-----------------------------------
Utilizando k3d (cluster)
	k3d cluster create kedacluster --no-lb

	https://helm.sh/docs/intro/install/
		choco install kubernetes-helm

	helm repo add kedacore https://kedacore.github.io/charts
	helm repo update

	kubectl create namespace keda
	
	helm install keda kedacore/keda --namespace keda

https://kubernetes.io/pt-br/docs/reference/kubectl/cheatsheet/

Deployment
	POD

kubectl create namespace sendreceive
kubectl apply -f deployment-send.yml -n sendreceive

kubectl get all -n sendreceive
kubectl get pod -n sendreceive
kubectl get deployment -n sendreceive

kubectl scale deployment kedasendk8s --replicas 5 -n sendreceive

kubectl describe pods -n sendreceive

Scalling
	gitbash
		echo -n "amqp://calazans:calazans@192.168.0.176:5672/%2F" | base64

kubectl apply -f scalling.yml -n sendreceive
kubectl get secrets
kubectl get scaledobjects -n sendreceive
kubectl get pod -n sendreceive

