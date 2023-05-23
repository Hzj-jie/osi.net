
using System;
using System.IO;

public static class Program {
    public static void Main(string[] args) {
        if (args == null || args.Length != 3) {
            Console.WriteLine("No strings to change or missing file name");
            Environment.Exit(1);
            return;
        }
        Action<String> w = a => Console.WriteLine(a);
        Action n = () => Console.WriteLine();

        n();
        w("'The following code is generated by /osi/root/codegen/sed/sed.exe");
        w("'by replacing");
        w("' " + args[0]);
        w("'into");
        w("' " + args[1]);
        w("'from the input file " + args[2]);
        w("'Do not edit it manually.");

        using (StreamReader r = new StreamReader(args[2])) {
            string l = null;
            while((l = r.ReadLine()) != null) {
                w(l.Replace(args[0], args[1]));
            }
        }
    }
}
