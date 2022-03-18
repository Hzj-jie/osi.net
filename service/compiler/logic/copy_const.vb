
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Partial Public NotInheritable Class logic
    ' Copy (instead of moving) a @data to @target
    ' VisibleForTesting
    Public NotInheritable Class _copy_const
        Implements instruction_gen

        Private ReadOnly target As String
        Private ReadOnly data As data_block

        Public Sub New(ByVal target As String, ByVal data As data_block)
            assert(Not String.IsNullOrEmpty(target))
            assert(Not data Is Nothing)
            Me.target = target
            Me.data = data
        End Sub

        Public Function build(ByVal o As vector(Of String)) As Boolean Implements instruction_gen.build
            Dim t As variable = Nothing
            If variable.of(target, o, t) AndAlso
               t.is_assignable_from(data.value_bytes_size()) Then
                o.emplace_back(instruction_builder.str(command.cpc, t, data))
                Return True
            End If
            Return False
        End Function
    End Class
End Class
