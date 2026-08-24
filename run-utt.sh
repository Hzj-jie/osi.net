#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
source "$SCRIPT_DIR/setenv.sh"

CONFIGURATION="${CONFIGURATION:-Release}"
UTT_BIN_DIR="$SCRIPT_DIR/root/utt/bin/$CONFIGURATION"
mkdir -p "$UTT_BIN_DIR"

# Sync all built libraries and tests to UTT directory so UTT can discover and run them
for proj in $(find "$SCRIPT_DIR/root" "$SCRIPT_DIR/service" -name "*.net8.vbproj"); do
    proj_dir="$(dirname "$proj")"
    asm="$(grep -oPm1 "(?<=<AssemblyName>)[^<]+" "$proj" || true)"
    if [ -z "$asm" ]; then
        asm="$(basename "$proj" .net8.vbproj)"
    fi
    src_dll="$proj_dir/bin/$CONFIGURATION/$asm.dll"
    if [ -f "$src_dll" ] && [ "$src_dll" != "$UTT_BIN_DIR/$asm.dll" ]; then
        cp -f "$src_dll" "$UTT_BIN_DIR/"
    fi
    src_pdb="$proj_dir/bin/$CONFIGURATION/$asm.pdb"
    if [ -f "$src_pdb" ] && [ "$src_pdb" != "$UTT_BIN_DIR/$asm.pdb" ]; then
        cp -f "$src_pdb" "$UTT_BIN_DIR/"
    fi
done

export utt_no_debug_mode=true
export utt_report_case_name="${utt_report_case_name:-true}"

echo "=== Running UTT Test Suite (.NET 8/10 - $CONFIGURATION) ==="
cd "$UTT_BIN_DIR"
exec ./osi.root.utt "$@"
