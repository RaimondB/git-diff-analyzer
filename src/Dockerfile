# TODO: Check what type of shell is in alpine, otherwise switch to default image and fix the dotnet publish.

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /App

# Copy everything
COPY . ./

# Use the invariant mode to reduce the size of the image
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true

# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
#RUN dotnet publish -c Release -o out
RUN dotnet publish --runtime linux-musl-x64 -c Release --self-contained false


# Build runtime image
#FROM alpine:3.18

FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine

RUN apk update && \
    apk add git 
#    && \
#    apk add bash

#RUN wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
#RUN chmod +x ./dotnet-install.sh
#RUN ./dotnet-install.sh -c 8.0 --runtime dotnet


#RUN apt-get update && \
#    apt-get install -y --no-install-recommends git


WORKDIR /App

#RUN apk add --no-cache \ 
#   openssh libunwind \
#   nghttp2-libs libidn krb5-libs libuuid lttng-ust zlib \
#   libstdc++ libintl \
#   icuexit

COPY --from=build-env /App/bin/Release/net8.0/linux-musl-x64/publish .
#COPY --from=build-env /App/out .
COPY --from=build-env /App/entrypoint.sh /entrypoint.sh

RUN chmod +x /entrypoint.sh

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
#ENV LD_LIBRARY_PATH=/usr/local/lib

ENTRYPOINT ["/entrypoint.sh"]