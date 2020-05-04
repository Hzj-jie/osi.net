
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class onebound
    Partial Public NotInheritable Class typed(Of K)
        Public NotInheritable Class evaluator
            Private ReadOnly m As model

            Public Sub New(ByVal m As model)
                assert(Not m Is Nothing)
                Me.m = m
            End Sub
        End Class
    End Class
End Class
