#!/bin/bash
# This script is for running the Docker container.
# To build the container, run build.sh first.

set -e

IMAGE_NAME="sauce-tests"
BROWSER="chrome"
DEBUG_MODE=false

while [[ "$#" -gt 0 ]]; do
    case $1 in
        --image-name) IMAGE_NAME="$2"; shift;;
        --browser) BROWSER="$2"; shift;;
        --debug) DEBUG_MODE=true;;
        *) echo "Unknown parameter passed: "; exit 1;;
    esac
    shift
done

if [ "$DEBUG_MODE" = true ]; then
    IMAGE_NAME="${IMAGE_NAME}-debug"
    echo "Running ${IMAGE_NAME} in debug mode..."
    docker run \
        --rm \
        -d \
        --name sauce-debug \
        -p 10000:10000 \
        -v $(pwd)/sauce:/src/sauce \
        ${IMAGE_NAME}
else
    echo "Running ${IMAGE_NAME} in normal mode..."

    DOCKER_RUN_OPTS="-it --rm -e HOST_UID=$(id -u) -e HOST_GID=$(id -g) -e HOME=/tmp -e BROWSER=${BROWSER} -v $(pwd)/sauce/TestResults:/src/sauce/TestResults"

    # Override the entrypoint to run our command
    docker run \
        ${DOCKER_RUN_OPTS} \
        ${IMAGE_NAME}
fi

