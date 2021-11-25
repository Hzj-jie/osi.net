
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

public static class genall {
    public static void Main(string[] args) {
        bool ignore_suffix = (Array.IndexOf(args, "--include-suffix") == -1);
        bool ignore_cmd = (Array.IndexOf(args, "--include-cmd") == -1);
        bool use_zipgen = (Array.IndexOf(args, "--use-zipgen") >= 0);
        string module_name = Array.Find(args, s => !s.StartsWith("--"));
        string pattern = Array.Find(args, s => s.StartsWith("--pattern=")).Substring("--pattern=".Length);
        string[] files = Directory.GetFiles(Directory.GetCurrentDirectory(), pattern, SearchOption.AllDirectories);
        var gen_args = new List<string>();
        gen_args.Add(module_name);
        foreach (var full_filename in files) {
            var file = full_filename.Substring(Directory.GetCurrentDirectory().Length + 1);
            if (ignore_cmd && (file.EndsWith(".cmd") || file.EndsWith(".bat"))) {
                continue;
            }
            string variable;
            if (ignore_suffix) {
                if (Path.GetDirectoryName(file).Length == 0) {
                    variable = Path.GetFileNameWithoutExtension(file);
                } else {
                    variable = Path.GetDirectoryName(file) + '\\' + Path.GetFileNameWithoutExtension(file);
                }
            } else {
                variable = file;
            }
            variable = variable.Replace('\\', '_')
                               .Replace('-', '_')
                               .Replace('.', '_')
                               .Replace(' ', '_')
                               .Replace('+', '_');
            gen_args.Add("\"[" + variable + "]\"");
            gen_args.Add('"' + file + '"');
        }

        string binary_name = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) +
                             '\\' +
                             (use_zipgen ? "zipgen.exe" : "gen.exe");
        string binary_args = string.Join(" ", gen_args.ToArray());
        Console.WriteLine("' This file is created by genall with ");
        Console.WriteLine("' " + binary_name);
        Console.WriteLine("' " + binary_args);

        var start_info = new ProcessStartInfo(binary_name, binary_args);
        start_info.UseShellExecute = false;
        start_info.RedirectStandardOutput = true;
        start_info.RedirectStandardError = true;
        start_info.CreateNoWindow = true;
        var process = new Process();
        process.StartInfo = start_info;
        process.Start();
        Console.Write(process.StandardOutput.ReadToEnd());
        Console.Error.Write(process.StandardError.ReadToEnd());
        process.WaitForExit();
        process.Close();
    }
}