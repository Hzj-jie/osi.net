
using System;
using System.IO;

public static class exec_case_exe
{
    public static int Main(string[] args)
    {
        bool failure = (args == null || args.Length == 0 || args[0] == "false");
        if(failure)
        {
            Console.Error.WriteLine("error");
            return -1;
        }
        else
        {
            Console.Out.WriteLine("output");
            return 0;
        }
    }
}

