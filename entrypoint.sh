#!/bin/bash
set -e

# if command is dotnet test, then create user and run as that user
if [ "$1" = 'dotnet' ] && [ "$2" = 'test' ]; then
    # Create a non-root user if HOST_UID and HOST_GID are set
    if [ -n "$HOST_UID" ] && [ -n "$HOST_GID" ]; then
        # Create the user group
        if ! getent group appgroup > /dev/null; then
            addgroup --gid "$HOST_GID" appgroup
        fi

        # Create the user
        if ! id -u appuser > /dev/null 2>&1; then
            adduser --disabled-password --gecos '' --uid "$HOST_UID" --gid "$HOST_GID" appuser
        fi

        # Change ownership of the source directory
        chown -R appuser:appgroup /src

        exec gosu appuser "$@"
    else
        exec "$@"
    fi
else
    exec "$@"
fi
