
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class struct
    Public Class definition
        Public ReadOnly type As Type
        Public ReadOnly name As String

        Public Sub New(ByVal type As Type, ByVal name As String)
            assert(Not type Is Nothing)
            assert(Not String.IsNullOrEmpty(name))
            Me.type = type
            Me.name = name
        End Sub
    End Class
End Class
