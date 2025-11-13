#!/bin/bash

set -e

IMAGE_NAME="sauce-tests"
DOCKER_BUILD_TARGET="release"

while [[ "$#" -gt 0 ]]; do
    case $1 in
        --image-name) IMAGE_NAME="$2"; shift;;
        --debug) DOCKER_BUILD_TARGET="debug";;
        *) echo "Unknown parameter passed: $1"; exit 1;;
    esac
    shift
done

if [[ "${DOCKER_BUILD_TARGET}" == "debug" ]]; then
    IMAGE_NAME="${IMAGE_NAME}-debug"
fi

echo "Building image ${IMAGE_NAME}..."
docker build \
    --target ${DOCKER_BUILD_TARGET} \
    -t ${IMAGE_NAME} \
    -f ./Dockerfile .

echo "Build complete."
