cd ..
docker buildx build --platform linux/amd64,linux/arm64,linux/arm/v7 -t mrh4287/fritzbox_tr064_exporter -f TR064Exporter/Dockerfile --push .