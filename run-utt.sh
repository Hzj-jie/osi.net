#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/setenv.sh"

CONFIGURATION="${CONFIGURATION:-Release}"
UTT_BIN_DIR="$SCRIPT_DIR/root/utt/bin/$CONFIGURATION/net8.0"
mkdir -p "$UTT_BIN_DIR"

# Sync all built libraries and tests to UTT directory so UTT can discover and run them
find "$SCRIPT_DIR/root" "$SCRIPT_DIR/service" -name "*.dll" -path "*/bin/$CONFIGURATION/net8.0/*" -exec cp -u {} "$UTT_BIN_DIR/" \; 2>/dev/null || true
find "$SCRIPT_DIR/root" "$SCRIPT_DIR/service" -name "*.pdb" -path "*/bin/$CONFIGURATION/net8.0/*" -exec cp -u {} "$UTT_BIN_DIR/" \; 2>/dev/null || true

export utt_no_debug_mode=true
export utt_report_case_name="${utt_report_case_name:-true}"

echo "=== Running UTT Test Suite (.NET 8/10 - $CONFIGURATION) ==="
cd "$UTT_BIN_DIR"
exec ./osi.root.utt "$@"
