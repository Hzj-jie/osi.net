#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/setenv.sh"

echo "=== Building root.net8.sln ==="
dotnet build "$SCRIPT_DIR/root/root.net8.sln" "$@"

echo "=== Building service.net8.sln ==="
dotnet build "$SCRIPT_DIR/service/service.net8.sln" "$@"

echo "=== Building production.net8.sln ==="
dotnet build "$SCRIPT_DIR/production/production.net8.sln" "$@"

echo "=== All builds completed successfully! ==="
