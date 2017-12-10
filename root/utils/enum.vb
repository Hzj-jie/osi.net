
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Public Module _enum_utils
    Public Function enum_to_string_pair(Of T)(ByRef o() As pair(Of T, String)) As Boolean
        Dim i As Int32 = 0
        Dim r() As pair(Of T, String) = Nothing
        If enum_traversal(Sub(ByVal x As T, ByVal s As String)
                              r(i) = emplace_make_pair(x, s)
                              i += 1
                          End Sub,
                          Sub(ByVal c As Int32)
                              ReDim r(c - 1)
                          End Sub) Then
            o = r
            Return True
        Else
            Return False
        End If
    End Function

    Public Function enum_to_string_pair(Of T)() As pair(Of T, String)()
        Dim r() As pair(Of T, String) = Nothing
        assert(enum_to_string_pair(r))
        Return r
    End Function
End Module
