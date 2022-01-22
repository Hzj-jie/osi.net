
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Public Structure current_namespace_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal name As String) As scope
                assert(s.cn Is Nothing)
                assert(Not name.null_or_whitespace())
                s.cn = name
                Return s
            End Function

            Public Function name() As String
                Dim s As scope = Me.s
                While s.cn Is Nothing
                    s = s.parent
                    If s Is Nothing Then
                        Return ""
                    End If
                End While
                Return s.cn
            End Function
        End Structure

        Public Function current_namespace() As current_namespace_proxy
            Return New current_namespace_proxy(Me)
        End Function
    End Class
End Class
