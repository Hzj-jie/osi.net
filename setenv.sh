#!/usr/bin/env bash

DOTNET_DIR="${DOTNET_DIR:-$HOME/.dotnet}"
DOTNET_BIN="$DOTNET_DIR/dotnet"

if [ ! -x "$DOTNET_BIN" ]; then
    echo "Installing .NET SDK (channel 10.0) into $DOTNET_DIR..."
    mkdir -p "$DOTNET_DIR"
    curl -sSL https://dot.net/v1/dotnet-install.sh | bash -s -- --channel 10.0 --install-dir "$DOTNET_DIR"
fi

export DOTNET_ROOT="$DOTNET_DIR"
export PATH="$DOTNET_ROOT:$PATH"
export DOTNET_CLI_TELEMETRY_OPTOUT=1
export DOTNET_NOLOGO=1
export DOTNET_ROLL_FORWARD=LatestMajor

if [ -x "$DOTNET_BIN" ]; then
    echo ".NET SDK ready: $($DOTNET_BIN --version)"
fi
