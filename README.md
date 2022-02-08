# fritzboxTR064Exporter
An Prometheus exporter for the FritzBox using the TR064 protocol

This Tools allows you to export metrics from your FritzBox to a Prometheus server.

It uses the TR064-protocol.

## Build
You need to run the Dockerfile with the root folder as context.
```bash
docker build . -f "TR064Exporter/Dockerfile" -t "mrh4287/fritzbox_tr064_exporter"
```

## Setup
```bash
docker run -p 8123:5000 --name fritzboxExporter mrh4287/fritzbox_tr064_exporter ip=192.168.1.1 username=apiUser password=apiPassword
```

This would start a new container with the name `fritzboxExporter`, which checks the Fritzbox at `192.168.1.1`.
It uses the User `apiUser` and the password `apiPassword`.

You can access the metrics at `http://localhost:8123/metrics`.

## Loki-Support
You can push your Logs to a local Loki instance.
To do so, add the `lokiEndpoint` to the Parameter.
You can define the `job` Tab by adding the `lokiJobName` Parameter.

The default is `fritzbox`.

Example:
```bash
docker run -p 8123:5000 --name fritzboxExporter mrh4287/fritzbox_tr064_exporter ip=192.168.1.1 username=apiUser password=apiPassword lokiEndpoint=http://10.0.0.1:3100 lokiJobName=router
```


# Licence
MIT Licence
