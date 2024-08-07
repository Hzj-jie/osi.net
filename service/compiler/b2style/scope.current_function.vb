
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        ' It's a different current_function implementation.
        Public NotInheritable Shadows Class current_function_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal name As String) As scope
                assert(s.cn Is Nothing)
                assert(Not name.null_or_whitespace())
                s.f = name
                Return s
            End Function

            Public Function name() As [optional](Of String)
                Dim s As scope = Me.s
                While s.f Is Nothing
                    s = s.parent
                    If s Is Nothing Then
                        Return [optional].empty(Of String)()
                    End If
                End While
                Return [optional].of(s.f)
            End Function
        End Class

        Public Shadows Function current_function() As current_function_proxy
            Return New current_function_proxy(Me)
        End Function
    End Class
End Class
