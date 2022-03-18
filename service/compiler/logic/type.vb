
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Partial Public NotInheritable Class logic
    Public NotInheritable Class _type
        Implements instruction_gen

        Private ReadOnly type As String
        Private ReadOnly size As UInt32

        Public Sub New(ByVal type As String, ByVal size As UInt32)
            assert(Not type.null_or_whitespace())
            Me.type = type
            Me.size = size
        End Sub

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            Return scope.current().types().define(type, size)
        End Function
    End Class
End Class
