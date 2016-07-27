# osi.net

(Moved from https://geminibranch.codeplex.com/SourceControl/latest#osi/ @ https://geminibranch.codeplex.com/SourceControl/changeset/117615)

the branch for all Hzj_jie's code

I will write some discussions about the design or implementation details in discussions https://geminibranch.codeplex.com/discussions

most of the code are actively re-organizing and re-fectoring under /osi
/osi/root contains useful stuffs for .net development
say
/osi/root/connector, some wrapper to .net functions
/osi/root/envs, hardware & software information, include environment variables, system performance
/osi/root/formation, some data structures
/osi/root/lock, a set of locks for different purpose
/osi/root/procedure, provide two different procedure based programming modules, by using lambda expression, they can save the time for working on lock / multi-threading / async-io, etc.
/osi/root/threadpool, a simple threadpool, which shares a same interface as managed threadpool, which can be switched by one line code change
/osi/root/utils, logging, counter, unhandled exception, stopwatch, resolver, etc
/osi/root/utt, a simple UniTTest framework
/osi/root/tests/, test cases for /osi/root stuffs

osi is shorting for Operation System Interface, which is targeting a cross platform, cross environment basic interfaces to save the programmers from duplicated work for different devices / module / online service.
the basic design is to split the logic from IO. and the target is, writing a single logic, and running on mobile / pc / browser / tv / distributing system, etc.
but diff from a virtual machine, i am not planning to create a virtual layer between device and logic, instead the logic is running in the real device <or .net runtime, since i am trying to implement it based on .net>, while osi is more like a set of plug-ins, by accessing the osi interfaces, you can get users input, render screen, read / write files as usual.



you will find lots of things in this code enlistment, from a piece of code to an entire solution.
including but not only
create an object without knowing its type
self-adapting compare without need an IComparable restriction
serialize / deserialize object for network transport or filesystem storage
different locks
how to generate build info to code
template design
hi perf counter
character escape
network / html / http utilities
pool
string utilities / lazyStrCat / lazyString
console application control break handling
catch unhandled exception in domain / thread
webBrowserDrawer
pinvoke
callbackManager / callbackAction
eventComb / eventDriver, both are to write async program as sync
configuration
hi perf threadpool
cacheManager
DoS protection
elpmis, scripting language engine
free cluster, storing structured data into linear stream
hi perf tcp / http server
remotecall engine
storoom
frontdoor
runrroom
... ... ... ...

i am actively working on these projects, so no release can be found in downloads. you need to refer to source code http://geminibranch.codeplex.com/SourceControl/list/changesets.

enjoy, my pleasure if any can help you.

/*******************************
non-commercial use only, or please contact me if you need to use the code for commercial purpose
except for the companies i am now working or was working at.
*******************************/

any questions, feel free to drop me a msg to hzj_jie@hotmail.com
