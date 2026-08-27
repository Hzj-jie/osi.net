#!/usr/bin/env bash

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd -P)"
source "$SCRIPT_DIR/setenv.sh"

SRC_ROOT="$SCRIPT_DIR"
DEPLOY_DIR="${DEPLOY_DIR:-$HOME/deploys/apps/osi.root.utt}"

rm -f "$SRC_ROOT/build.log" "$SRC_ROOT/run.log"

while true; do
    if [ -f "$SRC_ROOT/prepare.sh" ]; then
        "$SRC_ROOT/prepare.sh"
    fi

    echo "------------" >> "$SRC_ROOT/build.log"
    date >> "$SRC_ROOT/build.log"
    "$SRC_ROOT/build.sh" >> "$SRC_ROOT/build.log" 2>&1 || true

    mkdir -p "$DEPLOY_DIR"
    pushd "$DEPLOY_DIR" > /dev/null

    echo "------------" >> "$SRC_ROOT/run.log"
    date >> "$SRC_ROOT/run.log"
    "$SRC_ROOT/run-utt.sh" "$@" >> "$SRC_ROOT/run.log" 2>&1 || true

    popd > /dev/null

    if [ -n "$EXIT_NOW" ]; then
        break
    fi
done
