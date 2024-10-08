
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    ' while(var) { do() }
    ' VisibleForTesting
    Public NotInheritable Class _while_then
        Implements instruction_gen

        Private ReadOnly v As String
        Private ReadOnly p As paragraph

        Public Sub New(ByVal v As String, ByVal p As paragraph)
            assert(Not v.null_or_empty())
            assert(Not p Is Nothing)
            Me.v = v
            Me.p = p
        End Sub

        Private Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            assert(Not o Is Nothing)
            Dim var As variable = Nothing
            If Not variable.of(v, o, var) Then
                Return False
            End If
            Dim po As New vector(Of String)()
            If Not p.build(po) Then
                Return False
            End If

            o.emplace_back(instruction_builder.str(command.jumpif, data_ref.rel(2), var))
            o.emplace_back(instruction_builder.str(command.jump, data_ref.rel(po.size() + 2)))
            assert(o.emplace_back(po))
            o.emplace_back(instruction_builder.str(command.jump, data_ref.rel(-po.size() - 2)))

            Return True
        End Function
    End Class
End Class
