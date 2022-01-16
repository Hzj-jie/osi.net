
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Copy (instead of moving) a @data to @target
    Public NotInheritable Class _copy_const
        Implements exportable

        Private ReadOnly target As String
        Private ReadOnly data As data_block

        Public Sub New(ByVal target As String, ByVal data As data_block)
            assert(Not String.IsNullOrEmpty(target))
            assert(Not data Is Nothing)
            Me.target = target
            Me.data = data
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Dim t As variable = Nothing
            If variable.of(target, o, t) AndAlso
               t.is_assignable_from(data.value_bytes_size()) Then
                o.emplace_back(instruction_builder.str(command.cpc, t, data))
                Return True
            End If
            Return False
        End Function
    End Class
End Namespace
