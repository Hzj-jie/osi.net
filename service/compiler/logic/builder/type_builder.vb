
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Namespace logic
    Public NotInheritable Class type_builder
        Inherits builder

        Private ReadOnly name As String
        Private ReadOnly size As UInt32

        Public Sub New(ByVal name As String, ByVal size As UInt32)
            assert(Not name.null_or_whitespace())
            Me.name = name
            Me.size = size
        End Sub

        Public Overrides Sub [to](ByVal o As writer)
            o.append("type", name, size)
        End Sub
    End Class
End Namespace
