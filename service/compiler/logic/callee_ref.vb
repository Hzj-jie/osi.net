
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Public NotInheritable Class callee_ref
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly types As types
        Private ReadOnly name As String
        Private ReadOnly type As String
        Private ReadOnly parameters() As builders.parameter

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       ByVal type As String,
                       ByVal parameters As unique_ptr(Of pair(Of String, String)()))
            assert(Not anchors Is Nothing)
            assert(Not types Is Nothing)
            assert(Not String.IsNullOrEmpty(name))
            Me.anchors = anchors
            Me.types = types
            Me.name = name
            Me.type = type
            Me.parameters = builders.parameter.from_logic_callee_input(parameters.release_or_null())
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(False)
        End Function
    End Class
End Namespace
