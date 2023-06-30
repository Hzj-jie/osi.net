
using System;
using System.IO;

public static class gen
{
    private static void w(string s)
    {
        Console.WriteLine(s);
    }

    private static void w(int x)
    {
        Console.Write(x);
    }

    private static void wc()
    {
        Console.Write(",");
    }

    private static void wb()
    {
        Console.Write(" ");
    }

    private static void w()
    {
        Console.WriteLine();
    }

    private static void ws()
    {
        Console.Write("          ");
    }

    private static void wvs()
    {
        Console.Write("        { ");
    }

    private static string strcat(params string[] ss)
    {
        if(ss == null) return null;
        else
        {
            string r = null;
            for(int i = 0; i < ss.Length; i++)
                r += ss[i];
            return r;
        }
    }

    private static string merge_args(string[] args)
    {
        string r = null;
        for(int i = 1; i < args.Length; i += 2)
            r += args[i] + (i < args.Length - 2 ? " " : "");
        return r;
    }

    public static void Main(string[] args)
    {
        if(args == null || (args.Length & 1) != 1 || args.Length < 3)
            w("gen module_name variable_name1 file1 variable_name2 file2 ...");
        else
        {
            w();
            w("Option Explicit On");
            w("Option Infer Off");
            w("Option Strict On");
            w();
            w("\'this file is generated by /osi/service/resource/gen/gen.exe with");
            w("\'" + merge_args(args));
            w("\'so change /osi/service/resource/gen/gen.cs or resource files instead of this file");
            w();
            w("Friend Module " + args[0]);
            // if using initialize list, visual studio 2012 update 4 will crash
            for(int i = 1; i < args.Length; i += 2)
            {
                w("    Public ReadOnly " + args[i] + "() As Byte");
            }
            w();
            w("    Sub New()");
            for(int i = 1; i < args.Length; i+= 2)
            {
                using(Stream s = new FileStream(args[i + 1], FileMode.Open, FileAccess.Read))
                {
                    w("        ReDim " + args[i] + "(" + s.Length + " - 1)");
                    for(int j = 0; j < s.Length; j++)
                    {
                        int v = 0;
                        v = s.ReadByte();
                        if(v < 0) throw new Exception("ASSERT v >= 0");
                        else if(v > 0) w("        " + args[i] + "(" + Convert.ToString(j) + ") = CByte(" + v + ")");
                    }
                }
            }
            w("    End Sub");
            w("End Module");
        }
    }
}

