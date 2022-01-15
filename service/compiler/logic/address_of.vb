
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class address_of
        Implements exportable

        Private ReadOnly target As String
        Private ReadOnly name As String

        Public Sub New(ByVal target As String, ByVal name As String)
            assert(Not target.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            Me.target = target
            Me.name = name
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Dim t As variable = Nothing
            If Not variable.of(target, o, t) OrElse
               Not t.is_assignable_from_uint32() Then
                Return False
            End If
            Dim a As scope.anchor = Nothing
            If Not scope.current().anchors().of(name, a) Then
                Return False
            End If
            o.emplace_back(instruction_builder.str(command.cpc, t, New data_block(a.begin)))
            Return True
        End Function
    End Class
End Namespace
