#!/usr/bin/env bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd -P)"
source "$SCRIPT_DIR/setenv.sh"

CONFIGURATION="${CONFIGURATION:-Release}"

# If run from repo root, default to root/utt/bin/$CONFIGURATION; otherwise use current directory.
CURRENT_DIR="$(pwd -P)"
if [ "$CURRENT_DIR" = "$SCRIPT_DIR" ]; then
    TARGET_DIR="$SCRIPT_DIR/root/utt/bin/$CONFIGURATION"
else
    TARGET_DIR="$PWD"
fi
mkdir -p "$TARGET_DIR"

# Sync utt executable and runtime config files to target directory
UTT_SRC_DIR="$SCRIPT_DIR/root/utt/bin/$CONFIGURATION"
if [ -d "$UTT_SRC_DIR" ] && [ "$UTT_SRC_DIR" != "$TARGET_DIR" ]; then
    for f in "$UTT_SRC_DIR"/osi.root.utt*; do
        if [ -f "$f" ]; then
            cp -f "$f" "$TARGET_DIR/"
        fi
    done
fi

# Sync all built libraries and tests to target directory so UTT can discover and run them
for proj in $(find "$SCRIPT_DIR/root" "$SCRIPT_DIR/service" -name "*.net8.vbproj"); do
    proj_dir="$(dirname "$proj")"
    asm="$(grep -oPm1 "(?<=<AssemblyName>)[^<]+" "$proj" || true)"
    if [ -z "$asm" ]; then
        asm="$(basename "$proj" .net8.vbproj)"
    fi
    src_dll="$proj_dir/bin/$CONFIGURATION/$asm.dll"
    if [ -f "$src_dll" ] && [ "$src_dll" != "$TARGET_DIR/$asm.dll" ]; then
        cp -f "$src_dll" "$TARGET_DIR/"
    fi
    src_pdb="$proj_dir/bin/$CONFIGURATION/$asm.pdb"
    if [ -f "$src_pdb" ] && [ "$src_pdb" != "$TARGET_DIR/$asm.pdb" ]; then
        cp -f "$src_pdb" "$TARGET_DIR/"
    fi
done

export utt_no_debug_mode=true
export utt_report_case_name="${utt_report_case_name:-true}"

echo "=== Running UTT Test Suite (.NET 8/10 - $CONFIGURATION) in $TARGET_DIR ==="
cd "$TARGET_DIR"
exec ./osi.root.utt "$@"
