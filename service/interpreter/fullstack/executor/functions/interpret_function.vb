
Imports osi.root.connector

Namespace fullstack.executor
    Public Class interpret_function
        Inherits [function]

        Private ReadOnly body As sentence

        Public Sub New(ByVal return_type As type,
                       ByVal input_types() As type,
                       ByVal body As sentence)
            MyBase.New(return_type, input_types)
            Me.body = body
        End Sub

        Public Overrides Sub execute(ByVal domain As domain)
            assert(Not domain Is Nothing)
            If has_return() Then
                domain.define(New variable(return_type))
            End If
            If Not body Is Nothing Then
                Try
                    body.execute(domain)
                Catch ex As return_exception
                Catch ex As break_exception
                End Try
            End If
        End Sub
    End Class
End Namespace
