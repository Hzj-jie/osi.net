
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class [return]
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly types As types
        Private ReadOnly name As String
        Private ReadOnly return_value As [optional](Of String)

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal name As String,
                       Optional ByVal return_value As String = Nothing)
            assert(Not anchors Is Nothing)
            assert(Not types Is Nothing)
            assert(Not name.null_or_whitespace())
            assert(return_value Is Nothing OrElse Not return_value.null_or_whitespace())
            Me.anchors = anchors
            Me.types = types
            Me.name = name
            Me.return_value = [optional].of_nullable(return_value)
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            Dim r As variable = Nothing
            If Not logic.return_value.retrieve(types, name, r) Then
                Return False
            End If
            assert(r.size)
            If return_value Then
                Dim var As variable = Nothing
                If Not variable.of(types, +return_value, var) Then
                    Return False
                End If
                ' TODO: Check if +return_value is a temporary variable or a reference to decide whether move can be
                ' used.
                If Not copy.export(r, var, o) Then
                    Return False
                End If
            Else
                If Not types.is_zero_size(+(r.size)) Then
                    errors.no_return_value_provided(r)
                    Return False
                End If
            End If
            o.emplace_back(instruction_builder.str(command.rest))
            Return True
        End Function
    End Class
End Namespace
