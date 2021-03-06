﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class typed_node
    Private Sub nodes_str(ByVal s As StringBuilder)
        assert(Not s Is Nothing)
        If subnodes.empty() Then
            Return
        End If
        s.Append("{")
        Dim i As UInt32 = 0
        While i < subnodes.size()
            If i > 0 Then
                s.Append(", ")
            End If
            subnodes(i).str(s)
            i += uint32_1
        End While
        s.Append("}")
    End Sub

    Private Function str(ByVal s As StringBuilder) As StringBuilder
        assert(Not s Is Nothing)
        s.Append("[").
          Append(type).
          Append(":").
          Append(type_name).
          Append("<").
          Append(start).
          Append(",").
          Append([end]).
          Append(">")
        nodes_str(s)
        s.Append("]")
        Return s
    End Function

    Public Function str() As String
        Return Convert.ToString(str(New StringBuilder()))
    End Function

    Private Function self_debug_str(ByVal s As StringBuilder) As StringBuilder
        assert(Not s Is Nothing)
        s.Append("@").Append(type_name).Append(": ")
        For i As Int32 = 0 To min(CInt(word_count()), 3) - 1
            s.Append(word(CUInt(i)).str()).Append(" ")
        Next
        s.Append(newline.incode())
        Return s
    End Function

    Private Function trace_back_str(ByVal s As StringBuilder) As StringBuilder
        assert(Not s Is Nothing)
        self_debug_str(s)
        If Not root() Then
            parent.trace_back_str(s)
        End If
        Return s
    End Function

    Public Function trace_back_str() As String
        Return Convert.ToString(trace_back_str(New StringBuilder()))
    End Function

    Public Overrides Function ToString() As String
        Return str()
    End Function
End Class
