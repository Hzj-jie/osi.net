
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class bstyle
    Partial Public NotInheritable Class scope
        Private NotInheritable Class delegate_t
            Private ReadOnly s As New unordered_set(Of String)()

            Public Function define(ByVal d As String) As Boolean
                assert(Not d.null_or_whitespace())
                If s.emplace(d).second() Then
                    Return True
                End If
                raise_error(error_type.user, "Delegate type ", d, " has been defined already.")
                Return False
            End Function

            Public Function is_defined(ByVal d As String) As Boolean
                assert(Not d.null_or_whitespace())
                Return s.find(d) <> s.end()
            End Function
        End Class

        Public Structure delegate_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal d As String) As Boolean
                Return s.de.define(d)
            End Function

            Public Function is_defined(ByVal d As String) As Boolean
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.de.is_defined(d) Then
                        Return True
                    End If
                    s = s.parent
                End While
                Return False
            End Function
        End Structure

        Public Function delegates() As delegate_proxy
            Return New delegate_proxy(Me)
        End Function
    End Class
End Class
