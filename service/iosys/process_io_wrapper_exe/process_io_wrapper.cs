
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using osi.root.connector;
using osi.root.utils;

public static class process_io_wrapper
{
    private static void read(StreamReader r, bool from_error)
    {
        Thread.CurrentThread.IsBackground = true;
        int i = 0;
        try
        {
            while ((i = r.Read()) != -1)
            {
                string s = Convert.ToString(Convert.ToChar(i)).c_escape();
                if (from_error) Console.Error.WriteLine(s);
                else Console.Out.WriteLine(s);
            }
        }
        finally
        {
            r.Close();
        }
    }

    private static void read_output(shell_less_process p)
    {
        read(p.stdout(), false);
    }

    private static void read_error(shell_less_process p)
    {
        read(p.stderr(), true);
    }

    private static void write_input(shell_less_process p)
    {
        Thread.CurrentThread.IsBackground = true;
        int c = 0;
        try
        {
            while((c = Console.Read()) != -1)
            {
                p.stdin().Write(Convert.ToChar(c));
            }
        }
        finally
        {
            p.stdin().Close();
        }
    }

    public static int Main(string[] args)
    {
        if (args == null || args.Length < 1 || args.Length > 2)
        {
            Console.WriteLine("arguments error");
            return -1;
        }
        else
        {
            shell_less_process p = new shell_less_process();
            p.start_info().FileName = args[0];
            p.start_info().Arguments = args.Length == 1 ? null : args[1];
            Exception ex = null;
            if (!p.start(ref ex))
            {
                Console.WriteLine("failed to start process " +
                                  args[0] +
                                  " with args " +
                                  args[1] +
                                  (ex == null ? null : ", ex " + ex.Message));
                return -2;
            }

            (new Thread(() => read_output(p))).Start();
            (new Thread(() => read_error(p))).Start();
            (new Thread(() => write_input(p))).Start();
            p.wait_for_exit();
            int exit_code = p.exit_code();
            p.dispose();
            return exit_code;
        }
    }
}

