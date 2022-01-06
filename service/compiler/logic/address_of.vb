
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class address_of
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly anchors As anchors
        Private ReadOnly target As String
        Private ReadOnly name As String

        Public Sub New(ByVal types As types, ByVal anchors As anchors, ByVal target As String, ByVal name As String)
            assert(Not types Is Nothing)
            assert(Not anchors Is Nothing)
            assert(Not target.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            Me.types = types
            Me.anchors = anchors
            Me.target = target
            Me.name = name
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Dim t As variable = Nothing
            If Not variable.of(types, target, o, t) OrElse
               Not t.is_assignable_from_uint32() Then
                Return False
            End If
            Dim a As anchor = Nothing
            If Not anchors.of(name, a) Then
                Return False
            End If
            o.emplace_back(instruction_builder.str(command.cpc, t, New data_block(a.begin)))
            Return True
        End Function
    End Class
End Namespace
