#!/bin/bash
# Builds the test image.
# Assumes Dockerfile is in the current directory.

set -e

IMAGE_NAME="sauce-tests"
DEBUG_MODE="false"

while [[ "$#" -gt 0 ]]; do
    case $1 in
        --debug) DEBUG_MODE="true";;
        --image-name) IMAGE_NAME="$2"; shift;;
        *) echo "Unknown parameter passed: $1"; exit 1;;
    esac
    shift
done

echo "Building image ${IMAGE_NAME} with DEBUG_MODE=${DEBUG_MODE}..."
docker build \
    --build-arg DEBUG_MODE=${DEBUG_MODE} \
    -t ${IMAGE_NAME} \
    -f ./Dockerfile .

echo "Build complete."