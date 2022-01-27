# thread-pools
1. Never start\_thread unless in the test cases.
1. If the delegates are confirmed to be blocking and deadlock free (event\_comb,
   callback, promise, etc), use osi.root.threadpool.thread\_pool.
1. Others should go to osi.root.connector.managed\_thread\_pool.
