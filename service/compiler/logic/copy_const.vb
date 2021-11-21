
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Copy (instead of moving) a @data to @target
    Public NotInheritable Class copy_const
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly target As String
        Private ReadOnly data As data_block

        Public Sub New(ByVal types As types, ByVal target As String, ByVal data As unique_ptr(Of data_block))
            assert(Not types Is Nothing)
            assert(Not String.IsNullOrEmpty(target))
            assert(Not data Is Nothing)
            Me.types = types
            Me.target = target
            Me.data = data.release()
        End Sub

        Public Function export(ByVal o As vector(Of String)) As Boolean Implements exportable.export
            Dim t As variable = Nothing
            If variable.of(types, target, o, t) AndAlso
               t.is_assignable_from(data.value_bytes_size()) Then
                o.emplace_back(instruction_builder.str(command.cpc, t, data))
                Return True
            End If
            Return False
        End Function
    End Class
End Namespace
