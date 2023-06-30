
using System;
using osi.root.connector;
using osi.root.constants;
using osi.root.envs;
using osi.root.utils;

public static class priority_activity {
    public static void Main(string[] args) {
        string priority = null;
        if (_array.isemptyarray<string>(args))
            priority = "Idle";
        else
            priority = args[0];
        _env_value.set_env("priority", priority);
        global_init.execute((byte) global_init_level.foundamental);
        Console.WriteLine(String.Format("Finished global_init.execute({0})", (byte) global_init_level.foundamental));
        Console.Read();
    }
}
