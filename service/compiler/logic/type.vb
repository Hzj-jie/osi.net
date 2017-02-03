
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public Class type
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly type As String
        Private ReadOnly size As UInt32

        Public Sub New(ByVal types As types, ByVal type As String, ByVal size As UInt32)
            assert(Not types Is Nothing)
            Me.types = types
            Me.type = type
            Me.size = size
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            If size = uint32_0 Then
                Return types.define_variable_size(type)
            Else
                Return types.define(type, size)
            End If
        End Function
    End Class
End Namespace
