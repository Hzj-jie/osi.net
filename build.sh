#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/setenv.sh"

CONFIGURATION="${CONFIGURATION:-Release}"

echo "=== Building root.net8.sln ($CONFIGURATION) ==="
dotnet build "$SCRIPT_DIR/root/root.net8.sln" -c "$CONFIGURATION" "$@"

echo "=== Building service.net8.sln ($CONFIGURATION) ==="
dotnet build "$SCRIPT_DIR/service/service.net8.sln" -c "$CONFIGURATION" "$@"

echo "=== Building production.net8.sln ($CONFIGURATION) ==="
dotnet build "$SCRIPT_DIR/production/production.net8.sln" -c "$CONFIGURATION" "$@"

echo "=== All builds completed successfully! ==="
