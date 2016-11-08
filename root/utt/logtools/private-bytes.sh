#!/bin/sh

grep "memory status" $1 | sed 's/, workset bytes /\t/g' | sed 's/, virtual bytes/\t/g' | sed 's/after case /\t/g' | sed 's/: private /\t/g' | cut -f 2,4 | vim -
