
using System;

public static class Program
{
    private const string template_parameter = "T";
    private const string public_accessmodifier = "Public";
    private static void w(string a)
    {
        Console.WriteLine(a);
    }

    private static void w()
    {
        Console.WriteLine();
    }

    private static string strcat(params string[] s)
    {
        string r = null;
        for(int i = 0; i < s.Length; i++)
            r += s[i] + "\r\n";
        return r;
    }

    private class arguments
    {
        private const string tp = "t:";
        private const string ocp = "oc:";
        private const string ocam = "ocam:";
        private const string nsp = "ns:";
        private const string cam = "cam:";
        private const string ntp = "ntp:";
        private const string pp = "pp:";
        public readonly string type;
        public readonly string outter_class;
        public readonly string outter_class_accessmodifier;
        public readonly string namespaces;
        public readonly string class_accessmodifier;
        public readonly bool is_not_template_parameter;
        public readonly bool is_plugin;

        public arguments(string[] args)
        {
            type = template_parameter;
            outter_class = null;
            outter_class_accessmodifier = public_accessmodifier;
            namespaces = null;
            class_accessmodifier = public_accessmodifier;
            is_not_template_parameter = false;
            is_plugin = false;
            if(args != null)
            {
                for(int i = 0; i < args.Length; i++)
                {
                    if(!string.IsNullOrEmpty(args[i]))
                    {
                        if(args[i].StartsWith(tp))
                        {
                            type = args[i].Substring(tp.Length);
                        }
                        else if(args[i].StartsWith(ocp))
                        {
                            outter_class = args[i].Substring(ocp.Length);
                        }
                        else if(args[i].StartsWith(ocam))
                        {
                            outter_class_accessmodifier = args[i].Substring(ocam.Length);
                        }
                        else if(args[i].StartsWith(nsp))
                        {
                            namespaces = args[i].Substring(nsp.Length);
                        }
                        else if(args[i].StartsWith(cam))
                        {
                            class_accessmodifier = args[i].Substring(cam.Length);
                        }
                        else if(args[i].StartsWith(ntp))
                        {
                            is_not_template_parameter = true;
                        }
                        else if(args[i].StartsWith(pp))
                        {
                            is_plugin = true;
                        }
                    }
                }
            }
        }

        public bool template_type
        {
            get
            {
                return type == template_parameter && (!is_not_template_parameter);
            }
        }
    }

    private static string class_name(arguments a)
    {
        if(a.is_plugin) return a.outter_class;
        else return "adaptive_array" + (a.template_type ? "(Of " + a.type + ")" : "_" + a.type.ToLower());
    }

    public static void Main(string[] args)
    {
        arguments a = new arguments(args);
        w("\'this file is generated by /osi/root/codegen/adaptive_array/adaptive_array.exe");
        w("\'so change /osi/root/codegen/adaptive_array/adaptive_array.cs instead of this file");
        w("\'usually you do not need to use this codegen and the code generated unless it's in a very strict performance related code");
        w("\'use vector is a better way, while the implementation of vector is also using the code generated by this codegen");
        w("\'p.s. this file needs to work with osi.root.connector project");
        w();
        w("Option Explicit On");
        w("Option Infer Off");
        w("Option Strict On");
        w();
        w("Imports System.Runtime.CompilerServices");
        w();
        w("Imports osi.root");
        w("Imports osi.root.connector");
        w("Imports osi.root.constants");
        w();
        if(!string.IsNullOrEmpty(a.namespaces))
        {
            w("Namespace " + a.namespaces);
        }

        if(!string.IsNullOrEmpty(a.outter_class))
        {
            w("Partial " + a.outter_class_accessmodifier + " Class " + a.outter_class);
        }

        if(!a.is_plugin)
        {
            w(a.class_accessmodifier + " Class " + class_name(a));
        }
        w(strcat(
"    Implements ICloneable, ICloneable(Of " + class_name(a) + "), IComparable(Of " + class_name(a) + "), IComparable",
"",
"    Private Const size_limitation As UInt32 = (max_uint32 >> 1)",
"    'Private Shared ReadOnly default_value As " + a.type + " = Nothing",
"",
"    Private Shared Function expected_capacity(ByVal n As UInt32) As UInt32",
"        assert(n <= max_array_size)",
"        If n = max_array_size Then",
"            root.connector.throws.out_of_memory(\"adaptive_array size \", n, \" exceeds limitation.\")",
"        End If",
"        If n <= 2 Then",
"            Return 4",
"        End If",
"        n <<= 1",
"        Return If(n > max_array_size, max_array_size, n)",
"    End Function",
"",
"    Private d() As " + a.type,
"    Private s As UInt32",
"",
"    Public Sub New()",
"    End Sub",
"",
"    Public Sub New(ByVal n As UInt32)",
"        reserve(n)",
"    End Sub",
"",
"    Public Shared Function move(ByVal that As " + class_name(a) + ") As " + class_name(a),
"        If that Is Nothing Then",
"            Return Nothing",
"        End If",
"",
"        Dim r As " + class_name(a) + " = Nothing",
"        r = New " + class_name(a) + "()",
"        r.d = that.d",
"        r.s = that.s",
"        that.d = Nothing",
"        that.s = 0",
"        Return r",
"    End Function",
"",
"    Public Shared Function swap(ByVal this As " + class_name(a) + ", ByVal that As " + class_name(a) + ") As Boolean",
"        If this Is Nothing OrElse that Is Nothing Then",
"            Return False",
"        End If",
"        connector.swap(this.d, that.d)",
"        connector.swap(this.s, that.s)",
"        Return True",
"    End Function",
"",
"    Public Function replace_by(ByVal d() As " + a.type + ", ByVal s As UInt32) As Boolean",
"        If array_size(d) >= s Then",
"            Me.d = d",
"            Me.s = s",
"            Return True",
"        End If",
"        Return False",
"    End Function",
"",
"    Public Sub replace_by(ByVal d() As " + a.type + ")",
"        assert(replace_by(d, array_size(d)))",
"    End Sub",
"",
"    <MethodImpl(method_impl_options.aggressive_inlining)>",
"    Public Function max_size() As UInt32",
"        Return size_limitation",
"    End Function",
"",
"    <MethodImpl(method_impl_options.aggressive_inlining)>",
"    Public Function data() As " + a.type + "()",
"        Return d",
"    End Function",
"",
"' Property access is expensive.",
"#If 0 Then",
"    Default Public Property at(ByVal p As UInt32) As " + a.type,
"        Get",
"            Return [get](p)",
"        End Get",
"        Set(ByVal value As " + a.type + ")",
"            [set](p, value)",
"        End Set",
"    End Property",
"#End If",
"",
"    <MethodImpl(method_impl_options.aggressive_inlining)>",
"    Public Function [get](ByVal p As UInt32) As " + a.type,
"        Return d(CInt(p))",
"    End Function",
"",
"    <MethodImpl(method_impl_options.aggressive_inlining)>",
"    Public Sub [set](ByVal p As UInt32, ByVal v As " + a.type + ")",
"        d(CInt(p)) = v",
"    End Sub",
"",
"    <MethodImpl(method_impl_options.aggressive_inlining)>",
"    Public Function size() As UInt32",
"        Return s",
"    End Function",
"",
"    <MethodImpl(method_impl_options.aggressive_inlining)>",
"    Public Function empty() As Boolean",
"        Return size() = uint32_0",
"    End Function",
"",
"    <MethodImpl(method_impl_options.aggressive_inlining)>",
"    Public Function capacity() As UInt32",
"        Return array_size(d)",
"    End Function",
"",
"    <MethodImpl(method_impl_options.aggressive_inlining)>",
"    Public Function back() As " + a.type,
"        Return d(CInt(size() - uint32_1))",
"    End Function",
"",
"    Public Sub clear()",
"        s = uint32_0",
"#If 0 Then",
"        If size() > uint32_0 Then",
"            memclr(d, uint32_0, s)",
"            s = uint32_0",
"        End If",
"#End If",
"    End Sub",
"",
"    Public Sub push_back(ByVal v As " + a.type + ")",
"        reserve(size() + uint32_1)",
"        d(CInt(size())) = v",
"        s += uint32_1",
"    End Sub",
"",
"    Public Sub pop_back()",
"        s -= uint32_1",
"        'd(CInt(size())) = default_value",
"    End Sub",
"",
"    Public Sub reserve(ByVal n As UInt32)",
"        If capacity() < n Then",
"            Dim ec As UInt32 = 0",
"            ec = expected_capacity(n)",
"            assert(ec >= uint32_1)",
"            If empty() Then",
"                ReDim d(CInt(ec - uint32_1))",
"            Else",
"                ReDim Preserve d(CInt(ec - uint32_1))",
"            End If",
"        End If",
"    End Sub",
"",
"    Public Sub resize(ByVal n As UInt32)",
"        If capacity() < n Then",
"            reserve(n)",
"        'ElseIf size() > n Then",
"            'memclr(d, n, size() - n)",
"        End If",
"        s = n",
"    End Sub",
"",
"    Public Sub resize(ByVal n As UInt32, ByVal v As " + a.type + ")",
"        Dim os As UInt32 = 0",
"        os = size()",
"        If n > os Then",
"            resize(n)",
"            memset(d, os, n - os, v)",
"        Else",
"            resize(n)",
"        End If",
"    End Sub",
"",
"    Public Sub shrink_to_fit()",
"        If empty() Then",
"            ReDim d(-1)",
"        ElseIf capacity() > size() Then",
"            assert(size() >= uint32_1)",
"            ReDim Preserve d(CInt(size() - uint32_1))",
"        End If",
"    End Sub",
"",
"    Public Function Clone() As Object Implements ICloneable.Clone",
"        Return CloneT()",
"    End Function",
"",
"    Public Function CloneT() As " + class_name(a) + " Implements ICloneable(Of " + class_name(a) + ").Clone",
"        Dim r As " + class_name(a) + " = Nothing",
"        r = New " + class_name(a) + "()",
"        r.d = deep_clone(d)",
"        r.s = s",
"        Return r",
"    End Function",
"",
"    Public Shared Function compare(ByVal this As " + class_name(a) + ", ByVal that As " + class_name(a) + ") As Int32",
"        Dim c As Int32 = 0",
"        c = object_compare(this, that)",
"        If c <> object_compare_undetermined Then",
"            Return c",
"        End If",
"        assert(Not this Is Nothing)",
"        assert(Not that Is Nothing)",
"        If this.size() < that.size() Then",
"            Return -1",
"        ElseIf this.size() > that.size() Then",
"            Return 1",
"        Else",
"            Return deep_compare(this.d, that.d, this.size())",
"        End If",
"    End Function",
"",
"    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo",
"        Return CompareTo(cast(Of " + class_name(a) + ")(obj, False))",
"    End Function",
"",
"    Public Function CompareTo(ByVal other As " + class_name(a) + ") As Int32 Implements IComparable(Of " + class_name(a) + ").CompareTo",
"        Return compare(Me, other)",
"    End Function"
        ));

        if(!a.is_plugin)
        {
            w("End Class");
        }

        if(!string.IsNullOrEmpty(a.outter_class))
        {
            w("End Class");
        }

        if(!string.IsNullOrEmpty(a.namespaces))
        {
            w("End Namespace");
        }
    }
}

