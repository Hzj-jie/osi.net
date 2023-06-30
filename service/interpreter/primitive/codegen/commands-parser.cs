
using System.Collections.Generic;
using System.IO;
using System.Text;

public class commands_parser {
    private class command {
        public readonly string str;
        public readonly List<string> args;
        public readonly List<string> cmts;

        public command(string line) {
            string[] seps = line.Split('\t');
            str = seps[0];
            args = new List<string>();
            cmts = new List<string>();
            for (int i = 1; i < seps.Length; i++) {
                if (seps[i].StartsWith("#")) {
                    cmts.Add(seps[i].Substring(1));
                } else {
                    args.Add(seps[i]);
                }
            }
        }

        public string comments() {
            StringBuilder output = new StringBuilder();
            for (int i = 0; i < cmts.Count; i++) {
                output.Append(cmts[i])
                      .Append(' ');
            }
            return output.ToString();
        }
    }

    private static readonly StreamWriter enum_writer;
    private static readonly StreamWriter instruction_wrapper_import_bytes_writer;
    private static readonly StreamWriter instruction_wrapper_import_str_writer;
    private static readonly StreamWriter instructions_writer;
    private static readonly StreamWriter exportable_test_writer;

    static commands_parser() {
        enum_writer = new StreamWriter("commands.enum.vb");
        instruction_wrapper_import_bytes_writer = new StreamWriter("instruction_wrapper.import.bytes.vb");
        instruction_wrapper_import_str_writer = new StreamWriter("instruction_wrapper.import.str.vb");
        instructions_writer = new StreamWriter("instructions.vb");
        exportable_test_writer = new StreamWriter("instructions_exportable_test.vb");
    }

    private static void write_head() {
        const string common_head = 
            "\r\n" +
            "' This file is generated by commands-parser, with commands.txt file.\r\n" +
            "' So change commands-parser or commands.txt instead of this file.\r\n" +
            "\r\n" +
            "Option Explicit On\r\n" +
            "Option Infer Off\r\n" +
            "Option Strict On\r\n";
        enum_writer.WriteLine(common_head);
        instruction_wrapper_import_bytes_writer.WriteLine(common_head);
        instruction_wrapper_import_str_writer.WriteLine(common_head);
        instructions_writer.WriteLine(common_head);
        exportable_test_writer.WriteLine(common_head);

        enum_writer.WriteLine("Namespace primitive\r\n" +
                              "    Public Enum command As UInt32");

        instruction_wrapper_import_bytes_writer.WriteLine("Imports osi.root.connector\r\n\r\n" +
                                                          "Namespace primitive\r\n" +
                                                          "    Partial Public NotInheritable Class instruction_wrapper\r\n" +
                                                          "        Public Function import(ByVal i() As Byte, ByRef p As UInt32) As Boolean _\r\n" +
                                                          "                              Implements exportable.import\r\n" +
                                                          "            Me.i = Nothing\r\n" +
                                                          "            Dim x As UInt32 = 0\r\n" +
                                                          "            If Not bytes_uint32(i, x, unref(p)) Then\r\n" +
                                                          "                Return False\r\n" +
                                                          "            End If\r\n" +
                                                          "            Select Case x");
        instruction_wrapper_import_str_writer.WriteLine("Imports osi.root.connector\r\n" +
                                                        "Imports osi.root.formation\r\n\r\n" +
                                                        "Namespace primitive\r\n" +
                                                        "    Partial Public NotInheritable Class instruction_wrapper\r\n" +
                                                        "        Public Function import(ByVal s As vector(Of String), ByRef p As UInt32) As Boolean _\r\n" +
                                                        "                              Implements exportable.import\r\n" +
                                                        "            Me.i = Nothing\r\n" +
                                                        "            If s.null_or_empty() OrElse s.size() <= p Then\r\n" +
                                                        "                Return False\r\n" +
                                                        "            End If\r\n" +
                                                        "            Select Case s(p)");
        instructions_writer.WriteLine("Imports System.Text\r\n" +
                                      "Imports osi.root.constants\r\n" +
                                      "Imports osi.root.connector\r\n" +
                                      "Imports osi.root.formation\r\n\r\n" +
                                      "Namespace primitive\r\n" +
                                      "    Namespace instructions");
        exportable_test_writer.WriteLine("Imports osi.service.interpreter.primitive\r\n" +
                                         "Imports osi.service.interpreter.primitive.instructions\r\n\r\n" +
                                         "Namespace primitive\r\n" +
                                         "    Namespace instructions");
    }

    private static void write_end() {
        enum_writer.WriteLine("\r\n" +
                              "        COUNT\r\n" +
                              "    End Enum\r\n" +
                              "End Namespace");
        instruction_wrapper_import_bytes_writer.WriteLine("                Case Else\r\n" +
                                                          "                    Return False\r\n" +
                                                          "            End Select\r\n" +
                                                          "            assert(Not Me.i Is Nothing)\r\n" +
                                                          "            Return Me.i.import(i, p)\r\n" +
                                                          "        End Function\r\n" +
                                                          "    End Class\r\n" +
                                                          "End Namespace");
        instruction_wrapper_import_str_writer.WriteLine("                Case Else\r\n" +
                                                        "                    Return False\r\n" +
                                                        "            End Select\r\n" +
                                                        "            assert(Not Me.i Is Nothing)\r\n" +
                                                        "            Return Me.i.import(s, p)\r\n" +
                                                        "        End Function\r\n" +
                                                        "    End Class\r\n" +
                                                        "End Namespace");
        instructions_writer.WriteLine("    End Namespace\r\n" +
                                      "End Namespace");
        exportable_test_writer.WriteLine("    End Namespace\r\n" +
                                         "End Namespace");

        enum_writer.Close();
        instruction_wrapper_import_bytes_writer.Close();
        instruction_wrapper_import_str_writer.Close();
        instructions_writer.Close();
        exportable_test_writer.Close();
    }

    private static void write_enum(command cmd) {
        enum_writer.WriteLine("        ' " + cmd.comments());
        enum_writer.WriteLine("        [" + cmd.str + "]");
    }

    private static void write_instruction_wrapper_import_bytes(command cmd) {
        instruction_wrapper_import_bytes_writer.WriteLine("                Case command." + cmd.str);
        instruction_wrapper_import_bytes_writer.WriteLine("                    Me.i = New instructions." + cmd.str + "()");
    }

    private static void write_instruction_wrapper_import_str(command cmd) {
        instruction_wrapper_import_str_writer.WriteLine("                Case command_str(command." + cmd.str + ")");
        instruction_wrapper_import_str_writer.WriteLine("                    Me.i = New instructions." + cmd.str + "()");
    }

    private static void write_instructions(command cmd) {

        instructions_writer.WriteLine("        Partial Public NotInheritable Class [" + cmd.str + "]\r\n" +
                                      "            Implements instruction, IComparable, IComparable(Of [" + cmd.str + "])\r\n");
        for (int i = 0; i < cmd.args.Count; i++) {
            instructions_writer.WriteLine("            Private ReadOnly d" + i.ToString() + " As " + cmd.args[i]);
        }
        if (cmd.args.Count > 0) {
            instructions_writer.WriteLine("\r\n" +
                                          "            Public Sub New()");
            for (int i = 0; i < cmd.args.Count; i++) {
                instructions_writer.WriteLine("                d" + i.ToString() + " = New " + cmd.args[i] + "()");
            }
            instructions_writer.WriteLine("            End Sub\r\n\r\n" +
                                          "            Public Sub New( _");
            for (int i = 0; i < cmd.args.Count; i++) {
                string ext = i < cmd.args.Count - 1 ? "," : ")";
                instructions_writer.WriteLine("                       ByVal d" + i.ToString() + " As " + cmd.args[i] + ext);
            }
            for (int i = 0; i < cmd.args.Count; i++) {
                instructions_writer.WriteLine("                Me.d" + i.ToString() + " = d" + i.ToString());
            }
            instructions_writer.WriteLine("            End Sub\r\n");
        }
        instructions_writer.WriteLine("            Public Function bytes_size() As UInt32 Implements exportable.bytes_size\r\n" +
                                      "                Return sizeof_uint32" + (cmd.args.Count > 0 ? " +" : ""));
        for (int i = 0; i < cmd.args.Count; i++) {
            string ext = (i < cmd.args.Count - 1 ? " +" : "");
            instructions_writer.WriteLine("                       d" + i.ToString() + ".bytes_size()" + ext);
        }
        instructions_writer.WriteLine("            End Function\r\n\r\n" +
                                      "            Public Function export(ByRef b() As Byte) As Boolean Implements exportable.export");
        for (int i = 0; i < cmd.args.Count; i++) {
            instructions_writer.WriteLine("                Dim b" + i.ToString() + "() As Byte = Nothing\r\n" +
                                          "                If Not d" + i.ToString() + ".export(b" + i.ToString() + ") Then\r\n" +
                                          "                    Return False\r\n" +
                                          "                End If");
        }
        instructions_writer.WriteLine("                b = array_concat(uint32_bytes(command." + cmd.str + ")" + (cmd.args.Count > 0 ? "," : ")"));
        for (int i = 0; i < cmd.args.Count; i++) {
            string ext = (i < cmd.args.Count - 1 ? "," : ")");
            instructions_writer.WriteLine("                                 b" + i.ToString() + ext);
        }
        instructions_writer.WriteLine("                Return True\r\n" +
                                      "            End Function\r\n\r\n" +
                                      "            Public Function export(ByRef s As String) As Boolean Implements exportable.export\r\n" +
                                      "                Dim b As New StringBuilder()\r\n" +
                                      "                b.Append(command_str(command." + cmd.str + "))");
        for (int i = 0; i < cmd.args.Count; i++) {
            instructions_writer.WriteLine("                If Not d" + i.ToString() + ".export(s) Then\r\n" +
                                          "                    Return False\r\n" +
                                          "                End If\r\n" +
                                          "                b.Append(character.blank)\r\n" +
                                          "                b.Append(s)\r\n");
        }
        instructions_writer.WriteLine("                s = Convert.ToString(b)\r\n" +
                                      "                Return True\r\n" +
                                      "            End Function\r\n\r\n" +
                                      "            Public Function import(ByVal i() As Byte, ByRef p As UInt32) As Boolean Implements exportable.import\r\n" +
                                      "                Dim o As UInt32 = 0\r\n" +
                                      "                Return assert(bytes_uint32(i, o, p) AndAlso o = command." + cmd.str + ")" + (cmd.args.Count > 0 ? " AndAlso" : ""));
        for (int i = 0; i < cmd.args.Count; i++) {
            string ext = (i < cmd.args.Count - 1 ? " AndAlso" : "");
            instructions_writer.WriteLine("                       d" + i.ToString() + ".import(i, p)" + ext);
        }
        instructions_writer.WriteLine("            End Function\r\n\r\n" +
                                      "            Public Function import(s As vector(Of String), ByRef p As UInt32) As Boolean Implements exportable.import\r\n" +
                                      "                assert(Not s.null_or_empty() AndAlso s.size() > p)\r\n" +
                                      "                assert(s(p) = command_str(command." + cmd.str + "))\r\n" +
                                      "                p += uint32_1\r\n" +
                                      "                Return True" + (cmd.args.Count > 0 ? " AndAlso" : ""));
        for (int i = 0; i < cmd.args.Count; i++) {
            string ext = (i < cmd.args.Count - 1 ? " AndAlso" : "");
            instructions_writer.WriteLine("                       d" + i.ToString() + ".import(s, p)" + ext);
        }
        instructions_writer.WriteLine("            End Function\r\n\r\n" +
                                      "            Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo\r\n" +
                                      "                Return CompareTo(cast(Of [" + cmd.str + "])(obj, False))\r\n" +
                                      "            End Function\r\n\r\n" +
                                      "            Public Function CompareTo(ByVal other As [" + cmd.str + "]) As Int32 Implements IComparable(Of [" + cmd.str + "]).CompareTo\r\n" +
                                      "                Dim c As Int32 = object_compare(Me, other)\r\n" +
                                      "                If c <> object_compare_undetermined Then\r\n" +
                                      "                    Return c\r\n" +
                                      "                End If\r\n" +
                                      "                assert(Not other Is Nothing)");
        for (int i = 0; i < cmd.args.Count; i++) {
            instructions_writer.WriteLine("                c = Me.d" + i.ToString() + ".CompareTo(other.d" + i.ToString() + ")\r\n" +
                                          "                If c <> 0 Then\r\n" +
                                          "                    Return c\r\n" +
                                          "                End If\r\n");
        }
        instructions_writer.WriteLine("                Return 0\r\n" +
                                      "            End Function\r\n\r\n" +
                                      "            Public Overrides Function ToString() As String\r\n" +
                                      "                Dim s As String = Nothing\r\n" +
                                      "                assert(export(s))\r\n" +
                                      "                Return s\r\n" +
                                      "            End Function");
        for (int i = 0; i < cmd.args.Count;i ++) {
            if (cmd.args[i] == "data_ref") {
                instructions_writer.WriteLine("\r\n" +
                                              "            Private Function p" + i.ToString() + "(ByVal imi As imitation) As ref(Of Byte())\r\n" +
                                              "                assert(Not imi Is Nothing)\r\n" +
                                              "                Dim p As ref(Of Byte()) = imi.access(d" + i.ToString() + ")\r\n" +
                                              "                assert(Not p Is Nothing)\r\n" +
                                              "                Return p\r\n" +
                                              "            End Function");
            }
        }
        instructions_writer.WriteLine("        End Class\r\n");
    }

    private static void write_exportable_test(command cmd) {
        exportable_test_writer.WriteLine("        Public NotInheritable Class " + cmd.str + "_exportable_test\r\n" +
                                         "            Inherits exportable_test(Of [" + cmd.str + "])\r\n\r\n" +
                                         "            Protected Overrides Function create() As [" + cmd.str + "]");
        if (cmd.args.Count > 0) {
            exportable_test_writer.WriteLine("                Return New [" + cmd.str + "]( _");
            for (int i = 0; i < cmd.args.Count; i++) {
                string ext = i < cmd.args.Count - 1 ? "," : ")";
                exportable_test_writer.WriteLine("                        " + cmd.args[i] + ".random()" + ext);
            }
        } else {
            exportable_test_writer.WriteLine("                Return New [" + cmd.str + "]()");
        }
        exportable_test_writer.WriteLine("            End Function\r\n" +
                                         "        End Class\r\n");
    }

    public static void Main(string[] args) {
        write_head();
        string file = (args == null || args.Length == 0 ? "commands.txt" : args[0]);
        string[] lines = File.ReadAllLines(file);
        for (int i = 0; i < lines.Length; i++) {
            if (!string.IsNullOrWhiteSpace(lines[i])) {
                lines[i] = lines[i].Trim();
                if (!string.IsNullOrWhiteSpace(lines[i]) && !lines[i].StartsWith("#")) {
                    command cmd = new command(lines[i]);
                    write_enum(cmd);
                    write_instruction_wrapper_import_bytes(cmd);
                    write_instruction_wrapper_import_str(cmd);
                    write_instructions(cmd);
                    write_exportable_test(cmd);
                }
            }
        }
        write_end();
    }
}

