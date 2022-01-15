
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' while(var) { do() }
    Public NotInheritable Class _while_then
        Implements exportable

        Private ReadOnly v As String
        Private ReadOnly p As paragraph

        Public Sub New(ByVal v As String, ByVal p As paragraph)
            assert(Not String.IsNullOrEmpty(v))
            assert(Not p Is Nothing)
            Me.v = v
            Me.p = p
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not o Is Nothing)
            Dim var As variable = Nothing
            If Not variable.of(v, o, var) Then
                Return False
            End If
            Dim po As New vector(Of String)()
            If Not p.export(po) Then
                Return False
            End If

            o.emplace_back(instruction_builder.str(command.jumpif, data_ref.rel(2), var))
            o.emplace_back(instruction_builder.str(command.jump, data_ref.rel(po.size() + 2)))
            assert(o.emplace_back(po))
            o.emplace_back(instruction_builder.str(command.jump, data_ref.rel(-po.size() - 2)))

            Return True
        End Function
    End Class
End Namespace
