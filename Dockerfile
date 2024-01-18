FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build-env
WORKDIR /App

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
#RUN dotnet publish -c Release -o out
RUN dotnet publish --runtime linux-musl-x64 -c Release --self-contained true -o ./out


# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:8.0-alpine

RUN apt-get update && \
    apg-get install -y --no-install-recommends git

#FROM alpine:3.18
WORKDIR /App

#RUN apk add --no-cache \ 
#   openssh libunwind \
#   nghttp2-libs libidn krb5-libs libuuid lttng-ust zlib \
#   libstdc++ libintl \
#   icu

COPY --from=build-env /App/out .
COPY --from=build-env /App/entrypoint.sh .
#RUN chmod +x /App/git-diff-analyzer.dll

#ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
#ENV LD_LIBRARY_PATH=/usr/local/lib

ENTRYPOINT ["/entrypoint.sh"]