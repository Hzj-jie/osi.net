
using System;
using System.IO;
using System.IO.Compression;

public static class zipgen
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

    private static string strleft(string s, int len)
    {
        if(s.Length < len) return s;
        else return s.Substring(0, len);
    }

    private static string strmid(string s, int start)
    {
        if(s.Length < start) return null;
        else return s.Substring(start);
    }

    private static void cut_lines(string s)
    {
        // const int line_len = 64;
        const int line_len = 65523;
        int i = 0;
        while (i < s.Length) {
            int l = line_len;
            if (s.Length - i < l) l = s.Length - i;
            w("        \"" + s.Substring(i, l) + "\"" + (i + l < s.Length ? "," : "))"));
            i += l;
        }
    }

    public static void Main(string[] args)
    {
        if(args == null || (args.Length & 1) != 1 || args.Length < 3)
            w("zipgen module_name variable_name1 file1 variable_name2 file2 ...");
        else
        {
            w();
            w("Option Explicit On");
            w("Option Infer Off");
            w("Option Strict On");
            w();
            w("\'this file is generated by /osi/service/resource/zipgen/zipgen.exe with");
            w("\'" + merge_args(args));
            w("\'so change /osi/service/resource/gen/gen.cs or resource files instead of this file");
            w();
            w("Imports System.IO");
            w("Imports System.IO.Compression");
            w("Imports osi.root.connector");
            w();
            w("Friend Module " + args[0]);
            for(int i = 1; i < args.Length; i += 2)
            {
                // a long initialize list will crash visual studio 2012 update 4
                w("    Public ReadOnly " + args[i] + "() As Byte");
            }
            w();
            w("    Sub New()");
            for(int i = 1; i < args.Length; i += 2)
            {
                using(MemoryStream ms = new MemoryStream())
                {
                    using(Stream fs = new FileStream(args[i + 1], FileMode.Open, FileAccess.Read),
                                 zs = new GZipStream(ms, CompressionMode.Compress, true))
                    {
                        byte[] buf = new byte[fs.Length];
                        fs.Read(buf, 0, (int)fs.Length);
                        zs.Write(buf, 0, (int)fs.Length);
                    }
                    ms.Capacity = (int)ms.Length;
                    string base64 = Convert.ToBase64String(ms.ToArray());
                    w("        " + args[i] + " = Convert.FromBase64String(strcat_hint(CUInt(" + base64.Length.ToString() + "), _");
                    cut_lines(base64);
                }
                w();
                w("        assert(" + args[i] + ".ungzip(" + args[i] + "))");
            }
            w("    End Sub");
            w("End Module");
        }
    }
}

