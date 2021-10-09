
using System;
using System.IO;
using System.IO.Compression;

public static class zipgen {
    private static void w(string s) {
        Console.WriteLine(s);
    }

    private static void w(int x) {
        Console.Write(x);
    }

    private static void wc() {
        Console.Write(",");
    }

    private static void wb() {
        Console.Write(" ");
    }

    private static void w() {
        Console.WriteLine();
    }

    private static void ws() {
        Console.Write("          ");
    }

    private static void wvs() {
        Console.Write("        { ");
    }

    private static string strcat(params string[] ss) {
        if(ss == null) return null;
        string r = null;
        for(int i = 0; i < ss.Length; i++)
            r += ss[i];
        return r;
    }

    private static string merge_args(string[] args) {
        string r = null;
        for(int i = 1; i < args.Length; i += 2)
            r += args[i] + (i < args.Length - 2 ? " " : "");
        return r;
    }

    public static void Main(string[] args) {
        if(args == null || (args.Length & 1) != 1) {
            w("zipgen module_name variable_name1 file1 variable_name2 file2 ...");
            return;
        }
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
            w("    Private ReadOnly _" + args[i] + "() As Byte =");
            wvs();
            using(Stream ms = new MemoryStream())
            {
                int j = 0;
                using(Stream fs = new FileStream(args[i + 1], FileMode.Open, FileAccess.Read),
                             zs = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    while((j = fs.ReadByte()) != -1)
                        zs.WriteByte((byte)j);
                }
                ms.Seek(0, SeekOrigin.Begin);
                int l = 0;
                while((j = ms.ReadByte()) != -1)
                {
                    w(j);
                    if(ms.Position < ms.Length) wc();
                    l++;
                    if(l == 20)
                    {
                        w();
                        ws();
                        l = 0;
                    }
                    else wb();
                }
            }
            w("}");
            w("    Public ReadOnly " + args[i] + "() As Byte");
        }
        w();
        w("    Sub New()");
        for(int i = 1; i < args.Length; i += 2)
        {
            w("        assert(_" + args[i] + ".ungzip(" + args[i] + "))");
            w("        _" + args[i] + " = Nothing");
        }
        w("    End Sub");
        w("End Module");
    }
}

