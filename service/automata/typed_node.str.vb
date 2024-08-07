
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class typed_node
    Private Sub nodes_debug_str(ByVal s As StringBuilder)
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
            subnodes(i).debug_str(s)
            i += uint32_1
        End While
        s.Append("}")
    End Sub

    Private Function debug_str(ByVal s As StringBuilder) As StringBuilder
        assert(Not s Is Nothing)
        s.Append("[").
          Append(type).
          Append(":").
          Append(type_name).
          Append("<").
          Append(word_start).
          Append(",").
          Append(word_end).
          Append(">")
        nodes_debug_str(s)
        s.Append("]")
        Return s
    End Function

    Public Function debug_str() As String
        Return Convert.ToString(debug_str(New StringBuilder()))
    End Function

    Public Function only_descendant_str(ByRef o As String) As Boolean
        Dim n As typed_node = Nothing
        If Not only_descendant(n) Then
            Return False
        End If
        o = n.word().str()
        Return True
    End Function

    Public Function only_descendant_str() As String
        Dim r As String = Nothing
        assert(only_descendant_str(r))
        Return r
    End Function

    ' TODO: The space character should be defined by ignore-types.
    Public Function input() As String
        Return input(" ")
    End Function

    Private Function input(ByVal space As String) As String
        Dim s As New StringBuilder()
        Dim i As UInt32 = 0
        While i < word_count()
            s.Append(word(i).str())
            i += uint32_1
            If i < word_count() Then
                s.Append(space)
            End If
        End While
        Return s.ToString()
    End Function

    ' In most of the cases, using this function is not right. Instead input() should be used.
    ' TODO: restrict the use of this function.
    Public Function input_without_ignored() As String
        Return input("")
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
        Return debug_str()
    End Function
End Class
