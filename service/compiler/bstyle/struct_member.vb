
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class bstyle
    Public NotInheritable Class struct_member
        Public ReadOnly type As String
        Public ReadOnly name As String

        Public Sub New(ByVal type As String, ByVal name As String)
            assert(Not type.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            type = scope.current().type_alias()(type)

            Me.type = type
            Me.name = name
        End Sub
    End Class
End Class
