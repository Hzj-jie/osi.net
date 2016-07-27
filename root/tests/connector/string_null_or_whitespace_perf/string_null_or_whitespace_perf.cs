
// Targeting .net 4.0 or upper
using System;
using System.Diagnostics;
using System.IO;
using osi.root.connector;

public static class string_null_or_whitespace_perf
{
    private static string[] strs =
    {
        "",
        new string(' ', 1024),
        string.Concat(new string(' ', 1024), new string('a', 1024)),
        string.Concat(new string('a', 1024), new string(' ', 1024)),
        "a",
        new string('a', 1024),
        string.Concat("a", new string(' ', 1024), "a"),
        string.Concat(" ", new string('a', 1024), " "),
    };

    static string_null_or_whitespace_perf()
    {
        _char_detection.null_or_whitespace("");  // **1
    }

    private static void internal_func(long round)
    {
        for (long i = 0; i < round; i++)
        {
            string.IsNullOrWhiteSpace(strs[i % strs.Length]);
        }
    }

    private static void implemented_func(long round)
    {
        for (long i = 0; i < round; i++)
        {
            strs[i % strs.Length].null_or_whitespace();
        }
    }

    private static void implemented_func2(long round)
    {
        for (long i = 0; i < round; i++)
        {
            _char_detection.null_or_whitespace(strs[i % strs.Length]);
        }
    }

    public static int Main(string[] args)
    {
        const long round = 1024L * 1024;
        long int_ticks;
        Stopwatch watch = Stopwatch.StartNew();
        internal_func(round);
        int_ticks = watch.Elapsed.Ticks;
        long impl_ticks;
        watch = Stopwatch.StartNew();
        implemented_func(round);
        impl_ticks = watch.Elapsed.Ticks;
        long impl2_ticks;
        watch = Stopwatch.StartNew();
        implemented_func2(round);
        impl2_ticks = watch.Elapsed.Ticks;

        Console.WriteLine("internal_func: " + int_ticks.ToString());
        Console.WriteLine("implemented_func: " + impl_ticks.ToString());
        Console.WriteLine("implemented_func2: " + impl2_ticks.ToString());
        if (impl_ticks * 1.0 / int_ticks >= 1.5)
            return -1;
        return 0;
    }
}

