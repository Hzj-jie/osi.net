
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
        s.Append("[").
          Append(type).
          Append("<").
          Append(start).
          Append(",").
          Append([end]).
          Append("> {").
          Append(newline.incode())
        nodes_str(s)
        s.Append(newline.incode()).Append("}]")
        Return s
    End Function

    Public Function str() As String
        Return Convert.ToString(str(New StringBuilder()))
    End Function

    Private Function self_debug_str(ByVal s As StringBuilder) As StringBuilder
        assert(Not s Is Nothing)
        s.Append("@").Append(type_name).Append(": ")
        For i As UInt32 = 0 To min(child_count(), uint32_3) - uint32_1
            s.Append(word(i).str()).Append(" ")
        Next
        s.Append(newline.incode())
        Return s
    End Function

    Private Function debug_str(ByVal s As StringBuilder) As StringBuilder
        assert(Not s Is Nothing)
        self_debug_str(s)
        If Not root() Then
            parent.debug_str(s)
        End If
        Return s
    End Function

    Public Function debug_str() As String
        Return Convert.ToString(debug_str(New StringBuilder()))
    End Function

    Public Overrides Function ToString() As String
        Return str()
    End Function
End Class
