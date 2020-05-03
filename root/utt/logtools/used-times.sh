#!/bin/sh

grep -a "finish running " $1 | grep -a -v " all the selected " | sed 's/finish running /\t/g' | sed 's/, total time in milliseconds /\t/g' | sed 's/, processor usage /\t/g' | cut -f 2,3 | sort -rn -k2 | vim -
