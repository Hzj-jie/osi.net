
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class type
        Implements exportable

        Private ReadOnly type As String
        Private ReadOnly size As UInt32

        Public Sub New(ByVal type As String, ByVal size As UInt32)
            Me.type = type
            Me.size = size
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Return scope.current().types().define(type, size)
        End Function
    End Class
End Namespace
