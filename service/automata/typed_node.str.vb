
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class typed_node
    Private Function nodes_str(ByVal s As StringBuilder) As StringBuilder
        assert(Not s Is Nothing)
        Dim i As UInt32 = 0
        While i < subnodes.size()
            If i > 0 Then
                s.Append(", ")
            End If
            subnodes(i).str(s)
            i += uint32_1
        End While
        Return s
    End Function

    Private Function str(ByVal s As StringBuilder) As StringBuilder
        assert(Not s Is Nothing)
        s.Append("[").Append(type).Append("<").Append(start).Append(",").Append([end]).Append("> {").Append(newline.incode())
        nodes_str(s)
        s.Append(newline.incode()).Append("}]")
        Return s
    End Function

    Public Function str() As String
        Return Convert.ToString(str(New StringBuilder()))
    End Function

    ' TODO: Implementation
    Public Function stack_trace() As String

    End Function

    Public Overrides Function ToString() As String
        Return str()
    End Function
End Class
