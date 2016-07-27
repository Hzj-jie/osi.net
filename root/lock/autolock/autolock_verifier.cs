
using System;
using osi.root.@lock.slimlock;
using osi.root.@lock._internal;

public static class autolock_verifier
{
    private class broken_lock : islimlock
    {
        public void wait() {}
        public void release() {}
        public bool can_thread_owned() { return true; }
        public bool can_cross_thread() { return true; }
    }

    public static int Main(String[] argv)
    {
        islimlock l = new broken_lock();
        autolock al = new autolock(ref l);
        return 0;
    }
}
