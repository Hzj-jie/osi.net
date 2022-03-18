
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    Private NotInheritable Class _address_of
        Implements instruction_gen

        Private ReadOnly target As String
        Private ReadOnly name As String

        Public Sub New(ByVal target As String, ByVal name As String)
            assert(Not target.null_or_whitespace())
            assert(Not name.null_or_whitespace())
            Me.target = target
            Me.name = name
        End Sub

        ' TODO: Check the consistency of both signatures.
        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            Dim t As variable = Nothing
            If Not variable.of(target, o, t) OrElse
               Not t.is_assignable_from_uint32() Then
                Return False
            End If
            Dim a As scope.anchor = Nothing
            If Not scope.current().anchors().of(name, a) Then
                errors.anchor_undefined(name)
                Return False
            End If
            assert((+a.begin).on_stack())
            assert((+a.begin).absolute())
            o.emplace_back(instruction_builder.str(command.cpc, t, New data_block((+a.begin).offset())))
            Return True
        End Function
    End Class
End Class
