
grep "finish running" %1 | grep -v "all the selected" | sed "s/finish running /\t/g" | cut -f 2 | sort > %1.time
