
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Text
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utt.attributes
Imports osi.service.resource

<test>
Public NotInheritable Class tar_gen
    Private Shared output As argument(Of String)

    <command_line_specified>
    <test>
    Public Shared Sub run()
        Dim s As MemoryStream = tar.gen.read()
        s.shrink_to_fit()
        Dim r As String = multilines(
            "Option Explicit On",
            "Option Infer Off",
            "Option Strict On",
            "Imports osi.root.connector",
            "Public NotInheritable Class " + (output Or "tar_gen"),
            "    Private Sub New()",
            "    End Sub",
            "    Public Shared ReadOnly data() As Byte = ",
            "        Convert.FromBase64String(strcat_hint(CUInt(" + s.Length().ToString() + "), ",
            cut_lines(Convert.ToBase64String(s.GetBuffer())),
            "        ))",
            "End Class")
        If -output Then
            File.WriteAllText(+output + ".vb", r)
        Else
            Console.Out().Write(r)
        End If
    End Sub

    Private Shared Function cut_lines(ByVal s As String) As Object()
        Dim v As New vector(Of Object)()
        Const line_len As Int32 = 65523
        Dim i As Int32 = 0
        While i < s.Length()
            Dim l As Int32 = line_len
            If s.Length - i < l Then
                l = s.Length - i
            End If
            v.emplace_back("            """ + s.Substring(i, l) + """" + If(i + l < s.Length, ",", ""))
            i += l
        End While
        Return +v
    End Function

    Private Shared Function multilines(ByVal ParamArray objs() As Object) As String
        assert(Not objs.null_or_empty())
        Dim r As New StringBuilder()
        For Each obj As Object In objs
            Dim a() As Object = Nothing
            If direct_cast(obj, a) Then
                r.Append(multilines(a))
            Else
                r.Append(obj).Append(newline.incode())
            End If
        Next
        Return r.ToString()
    End Function

    Private Sub New()
    End Sub
End Class
