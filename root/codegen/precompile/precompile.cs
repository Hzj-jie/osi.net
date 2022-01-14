
using System;
using System.Collections.Generic;
using System.IO;

public static class Program
{
    private static readonly IDictionary<string, string> defines;
    private static bool debug_mode;
    private static int depth;

    static Program()
    {
        defines = new Dictionary<string, string>();
    }

    private static void debug_output(params object[] infos)
    {
        if (infos != null && debug_mode)
        {
            Console.Error.Write(">>>");
            for (int i = 0; i < infos.Length; i++)
            {
                Console.Error.Write(infos[i]);
            }
            Console.Error.WriteLine("<<<");
        }
    }

    private static bool separate_key_value(string l, out string f, out string s)
    {
        f = null;
        s = null;
        if(string.IsNullOrWhiteSpace(l)) return false;
        else
        {
            l = l.TrimStart();
            if(string.IsNullOrWhiteSpace(l)) return false;
            int index = l.IndexOfAny(new char[] { ' ', '\t' });
            if(index == -1) return false;
            else
            {
                f = l.Substring(0, index);
                s = l.Substring(index + 1);
                if(s == null) s = "";
                if(string.IsNullOrWhiteSpace(f)) return false;
                else
                {
                    f = f.Trim();
                    s = s.Trim();
                    return !string.IsNullOrWhiteSpace(f);
                }
            }
        }
    }

    private static bool handle_define(string l)
    {
        const string define_start = "##DEFINE ";
        l = l.TrimStart();
        if(l.StartsWith(define_start))
        {
            string f;
            string s;
            if(separate_key_value(l.Substring(define_start.Length), out f, out s))
            {
                debug_output("define ", f, " as ", s);
                defines[f] = s;
                return true;
            }
            else return false;
        }
        else return false;
    }

    private static bool handle_default(string l)
    {
        const string default_start = "##DEFAULT ";
        l = l.TrimStart();
        if(l.StartsWith(default_start))
        {
            string f;
            string s;
            if(separate_key_value(l.Substring(default_start.Length), out f, out s))
            {
                if(!defines.ContainsKey(f))
                {
                    debug_output("default define ", f, " as ", s);
                    defines[f] = s;
                }
                return true;
            }
            else return false;
        }
        else return false;
    }

    private static bool handle_undef(string l)
    {
        const string undef_start = "##UNDEF ";
        l = l.TrimStart();
        if(l.StartsWith(undef_start))
        {
            l = l.Substring(undef_start.Length).Trim();
            if(defines.ContainsKey(l))
            {
                debug_output("undefine ", l);
                defines.Remove(l);
            }
            return true;
        }
        else return false;
    }

    private static bool handle_include(string current_file, string l, TextWriter writer)
    {
        const string include_start = "##INCLUDE ";
        l = l.TrimStart();
        if(l.StartsWith(include_start))
        {
            l = l.Substring(include_start.Length);
            debug_output("include ", l);
            handle(Path.Combine(Path.GetDirectoryName(current_file), l), writer);
            return true;
        }
        else return false;
    }

    private static string replace_marks(string l)
    {
        const string prefix = "##";
        const string suffix = "##";
        string orig_l = l;
        string last_l = null;
        while (last_l != l)
        {
            last_l = l;
            foreach(var p in defines)
                l = l.Replace(prefix + p.Key + suffix, p.Value);
        }
        if (orig_l != l)
            debug_output("replace marks of {", orig_l, "} into {", l, "}");
        return l;
    }

    private static void handle(string filename, TextReader reader, TextWriter writer)
    {
        depth++;
        Action<string> w = (a) => writer.WriteLine(a);
        Action n = () => writer.WriteLine();
        n();
        if (depth == 1)
        {
            w("Option Explicit On");
            w("Option Infer Off");
            w("Option Strict On");
            n();
            defines["FILENAME"] = Path.GetFileNameWithoutExtension(filename);
            defines["FULL_FILENAME"] = filename;
            String root_filename = filename;
            while (true) {
                if (root_filename.Equals(Path.GetFileNameWithoutExtension(root_filename))) {
                    break;
                }
                root_filename = Path.GetFileNameWithoutExtension(root_filename);
            }
            defines["ROOT_FILENAME"] = root_filename;
        }
        w("\'the following code is generated by /osi/root/codegen/precompile/precompile.exe");
        w("\'with " + filename + " ----------");
        w("\'so change " + filename + " instead of this file");
        n();

        defines["CURRENT_FILENAME"] = Path.GetFileNameWithoutExtension(filename);
        defines["CURRENT_FULL_FILENAME"] = filename;
        string l = null;
        while((l = reader.ReadLine()) != null)
        {
            l = replace_marks(l);
            if(!handle_define(l) &&
               !handle_default(l) &&
               !handle_undef(l) &&
               !handle_include(filename, l, writer))
            {
                writer.WriteLine(l);
            }
        }

        w("\'finish " + filename + " --------");
        depth--;
    }

    private static void handle(string filename, TextWriter writer)
    {
        using(StreamReader r = new StreamReader(filename))
        {
            handle(filename, r, writer);
        }
    }

    public static void Main(string[] args)
    {
        if(args == null || args.Length == 0)
            handle("Console.In", Console.In, Console.Out);
        else
        {
            for(int i = 0; i < args.Length; i++)
            {
                if (args[i] == "/d") debug_mode = true;
                else handle(args[i], Console.Out);
            }
        }
    }
}

