# Docker 
Install Docker on Ubuntu VM
`sudo apt-get update`

`sudo apt install docker.io`

# Rancher
Instal rancher on master node:

`sudo docker run -d --restart=unless-stopped -p 80:80 -p 443:443 rancher/rancher`

Install etcd/control plane/worker on slave:

`sudo docker run -d --privileged --restart=unless-stopped --net=host -v /etc/kubernetes:/etc/kubernetes -v /var/run:/var/run rancher/rancher-agent:v2.2.7 --server https://40.68.151.169 --token hz989dbn6n7tffc9462w6f9s8w9q6trn4hsqknqbw49hrc25zjpw6r --ca-checksum 43fabf29b2b4b82d9bc17a7fc3e2d7444078c8e8e6db77bf03e47e9f0c554ee1 --worker`

Specify **public/internal address**:

`--address <Public_IP> --internal-address <Internal_Node_IP>`




