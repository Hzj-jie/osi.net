
using System;
using osi.root.connector;
using osi.root.envs;
using osi.root.utils;

public static class priority_activity
{
    public static void Main(string[] args)
    {
        string priority = null;
        if (_array.isemptyarray<string>(args))
            priority = "Idle";
        else
            priority = args[0];
        _env_value.set_env("priority", priority);
        global_init.execute(1);
        Console.WriteLine("Finished global_init.execute(1)");
        Console.Read();
    }
}
