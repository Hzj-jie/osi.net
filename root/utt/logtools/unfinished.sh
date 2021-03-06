#!/bin/sh

grep -a "start running " $1 | sed 's/start running /\t/g' | sed 's/ at /\t/g' | cut -f 2 | sort > $1.started
grep -a "finish running " $1 | sed 's/finish running /\t/g' | sed 's/, total time /\t/g' | cut -f 2 | sort > $1.finished
diff $1.started  $1.finished | vim -
rm $1.started
rm $1.finished
