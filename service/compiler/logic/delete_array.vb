
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    ' Delete an array with @name
    Public NotInheritable Class delete_array
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly name As String

        Public Sub New(ByVal anchors As anchors,
                       ByVal name As String)
            assert(Not anchors Is Nothing)
            assert(Not name.null_or_whitespace())
            Me.anchors = anchors
            Me.name = name
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Return assert(False)
        End Function
    End Class
End Namespace
