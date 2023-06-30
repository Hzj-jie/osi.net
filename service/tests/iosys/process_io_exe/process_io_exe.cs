
using System;
using System.IO;

public static class process_io_exe
{
    public static void Main(string[] args)
    {
        TextWriter writer = null;
        if(!ReferenceEquals(args, null) &&
           args.Length > 0)
            writer = Console.Error;
        else
            writer = Console.Out;
        int c = 0;
        while((c = Console.Read()) != -1)
            writer.Write(Convert.ToChar(c));
    }
}

