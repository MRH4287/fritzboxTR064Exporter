# fritzboxTR064Exporter
An Prometheus exporter for the FritzBox using the TR064 Protocol

This Tools allows you to export metrics from your FritzBox to a Prometheus Server.

It uses the TR064 Protocol.

## Build
You need to run the Dockerfile with the root Folder as Context.
```bash
docker build . -f "TR064Exporter/Dockerfile" -t "mrh4287/fritzbox_tr064_exporter"
```

## Setup
```bash
docker run -p 8123:5000 --name fritzboxExporter mrh4287/fritzbox_tr064_exporter ip=192.168.1.1 username=apiUser password=apiPassword
```

This would start a new Container with the name `fritzboxExporter`, which checks the Fritzbox at `192.168.1.1`.
It uses the User `apiUser` and the password `apiPassword`

# Licence
MIT Licence
