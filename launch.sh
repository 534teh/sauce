#!/bin/bash
# Runs the test image.
# Pass --debug to run in debug mode.

set -e

IMAGE_NAME="sauce-tests"
DEBUG_MODE="false"
DOCKER_RUN_OPTS="--rm"
BROWSER="chrome"

while [[ "$#" -gt 0 ]]; do
    case $1 in
        --debug) DEBUG_MODE="true";;
        --image-name) IMAGE_NAME="$2"; shift;;
        --browser) BROWSER="$2"; shift;;
        *) echo "Unknown parameter passed: $1"; exit 1;;
    esac
    shift
done

# Build the image if it doesn't exist
if [[ "$(docker images -q ${IMAGE_NAME} 2> /dev/null)" == "" ]]; then
  echo "Image ${IMAGE_NAME} not found. Building..."
  ./build.sh --image-name ${IMAGE_NAME} --debug=${DEBUG_MODE}
fi

if [ "$DEBUG_MODE" = "true" ]; then
    echo "Running ${IMAGE_NAME} in DEBUG mode."
    echo "Attach your debugger to port 4026..."
    DOCKER_RUN_OPTS="${DOCKER_RUN_OPTS} -e VSTEST_HOST_DEBUG=true -p 4026:4026"
else
    echo "Running ${IMAGE_NAME} in normal mode..."
fi

DOCKER_RUN_OPTS="${DOCKER_RUN_OPTS} -e BROWSER=${BROWSER} -v $(pwd)/sauce/TestResults:/src/sauce/TestResults"
docker run ${DOCKER_RUN_OPTS} ${IMAGE_NAME}