
using System;

public static class shell_less_process_test
{
    public static int Main()
    {
        int i = 0;
        while ((i = Console.Read()) != -1)
        {
            Console.Out.Write(Convert.ToChar(i));
            Console.Error.Write(Convert.ToChar(i));
        }

        return 0;
    }
}
