
using System;

public static class program
{
    public static void Main(string[] args)
    {
        if(ReferenceEquals(args, null) ||
           args.Length == 0)
            Console.WriteLine("no input");
        else
        {
            for(int i = 0; i < args.Length; i++)
                Console.WriteLine(args[i]);
        }
    }
}

