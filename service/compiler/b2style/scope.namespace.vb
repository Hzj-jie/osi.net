
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Public NotInheritable Class current_namespace_t
            Private ReadOnly s As New stack(Of String)()

            Public Function define(ByVal name As String) As IDisposable
                assert(Not name.null_or_whitespace())
                s.emplace(name)
                Return defer.to(Sub()
                                    assert(Not s.empty())
                                    s.pop()
                                End Sub)
            End Function

            Public Function name() As String
                If s.empty() Then
                    Return ""
                End If
                Return s.back()
            End Function
        End Class

        Public Function current_namespace() As current_namespace_t
            If is_root() Then
                assert(cn IsNot Nothing)
                Return cn
            End If
            Return (+root).current_namespace()
        End Function
    End Class
End Class
