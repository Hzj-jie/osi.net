
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Partial Public NotInheritable Class b2style
    Partial Public NotInheritable Class scope
        Private NotInheritable Class class_t
            Private ReadOnly m As New unordered_set(Of name_with_namespace)()

            Public Function define(ByVal name As String) As Boolean
                If m.emplace(name_with_namespace.of(name)).second() Then
                    Return True
                End If
                raise_error(error_type.user, "Class ", name_with_namespace.of(name), " has been defined already.")
                Return False
            End Function

            Public Function resolve(ByVal name As String) As Boolean
                Return m.find(name_with_namespace.of(name)) <> m.end()
            End Function
        End Class

        Public Structure class_proxy
            Private ReadOnly s As scope

            Public Sub New(ByVal s As scope)
                assert(Not s Is Nothing)
                Me.s = s
            End Sub

            Public Function define(ByVal name As String) As Boolean
                Return s.c.define(name)
            End Function

            Public Function resolve(ByVal name As String) As Boolean
                Dim s As scope = Me.s
                While Not s Is Nothing
                    If s.c.resolve(name) Then
                        Return True
                    End If
                    s = s.parent
                End While
                Return False
            End Function
        End Structure

        Public Function classes() As class_proxy
            Return New class_proxy(Me)
        End Function
    End Class
End Class
