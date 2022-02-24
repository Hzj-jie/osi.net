
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class _return
        Implements instruction_gen

        Private ReadOnly name As String
        Private ReadOnly return_value As [optional](Of String)

        Public Sub New(ByVal name As String, ByVal return_value As String)
            assert(Not name.null_or_whitespace())
            assert(return_value Is Nothing OrElse Not return_value.null_or_whitespace())
            Me.name = name
            Me.return_value = [optional].of_nullable(return_value)
        End Sub

        Public Sub New(ByVal name As String, ByVal no_return_value As importer.place_holder)
            Me.New(name, [default](Of String).null)
        End Sub

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(Not o Is Nothing)
            Dim r As variable = Nothing
            If Not logic.return_value.retrieve(name, o, r) Then
                Return False
            End If
            If return_value Then
                Dim var As variable = Nothing
                If Not variable.of(+return_value, o, var) Then
                    Return False
                End If
                ' TODO: Check if +return_value is a temporary variable or a reference to decide whether move can be
                ' used.
                If Not _copy.export(r, var, o) Then
                    Return False
                End If
            Else
                If Not scope.type_t.is_zero_size(r.size) Then
                    errors.no_return_value_provided(r)
                    Return False
                End If
            End If
            o.emplace_back(instruction_builder.str(command.rest))
            Return True
        End Function
    End Class
End Namespace
