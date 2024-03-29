# Docker 
Install Docker on Ubuntu VM

`sudo apt-get update`

`sudo apt install docker.io`

# Rancher
Instal rancher on master node:

`sudo docker run --privileged -d --restart=unless-stopped -p 80:80 -p 443:443 rancher/rancher`

Install etcd/control plane/worker on slave:

`sudo docker run -d --privileged --restart=unless-stopped --net=host -v /etc/kubernetes:/etc/kubernetes -v /var/run:/var/run rancher/rancher-agent:v2.2.7 --server https://40.68.151.169 --token <token> --worker`

Name the node:

`--node-name <node_name>`

Specify **public/internal address**:

`--address <Public_IP> --internal-address <Internal_Node_IP>`


# Istio
Run for each namespace to allow sidecars injection:

`kubectl label namespace <$your-namesapce> istio-injection=enabled`

Istio requires also labels for **app** and **version**


# In case of ETCD failure...

```bash
docker rm -f $(sudo docker ps -aq);
docker volume rm $(sudo docker volume ls -q);

rm -rf /etc/ceph \
       /etc/cni \
       /etc/kubernetes \
       /opt/cni \
       /opt/rke \
       /run/secrets/kubernetes.io \
       /run/calico \
       /run/flannel \
       /var/lib/calico \
       /var/lib/etcd \
       /var/lib/cni \
       /var/lib/kubelet \
       /var/lib/rancher/rke/log \
       /var/log/containers \
       /var/log/pods \
       /var/run/calico

for mount in $(mount | grep tmpfs | grep '/var/lib/kubelet' | awk '{ print $3 }') /var/lib/kubelet /var/lib/rancher; do umount $mount; done

rm -f /var/lib/containerd/io.containerd.metadata.v1.bolt/meta.db
sudo systemctl restart containerd
sudo systemctl restart docker

IPTABLES="/sbin/iptables"
cat /proc/net/ip_tables_names | while read table; do
  $IPTABLES -t $table -L -n | while read c chain rest; do
      if test "X$c" = "XChain" ; then
        $IPTABLES -t $table -F $chain
      fi
  done
  $IPTABLES -t $table -X
done

```



