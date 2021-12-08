
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Public NotInheritable Class function_call_t
            ' To -> From
            Private ReadOnly m As New unordered_map(Of String, vector(Of String))()
            Private ReadOnly tm As New unordered_map(Of String, Boolean)()

            Public Sub define(ByVal [to] As String)
                assert(Not [to].null_or_whitespace())
                Dim [from] As String = Nothing
                If scope.current().current_function().is_defined() Then
                    [from] = scope.current().current_function().name()
                Else
                    [from] = "main"
                End If
                assert(Not [from].null_or_whitespace())
                [to] = [to].Trim()
                [from] = [from].Trim()
                m([to]).emplace_back([from])
            End Sub

            Public Function can_reach_main(ByVal f As String) As Boolean
                assert(Not f.null_or_whitespace())
                f = f.Trim()
                If f.Equals("main") Then
                    Return True
                End If
                Dim it As unordered_map(Of String, Boolean).iterator = tm.find(f)
                If it <> tm.end() Then
                    Return (+it).second
                End If
                Dim r As Boolean = False
                Dim it2 As unordered_map(Of String, vector(Of String)).iterator = m.find(f)
                If it2 <> m.end() Then
                    Dim v As vector(Of String) = (+it2).second
                    assert(Not v.null_or_empty())
                    For i As UInt32 = 0 To v.size() - uint32_1
                        If can_reach_main(v(i)) Then
                            r = True
                            Exit For
                        End If
                    Next
                End If
                tm(f) = r
                Return r
            End Function
        End Class

        Public Function function_calls() As function_call_t
            If is_root() Then
                assert(Not fc Is Nothing)
                Return fc
            End If
            assert(fc Is Nothing)
            Return (+root).fc
        End Function
    End Class
End Class
