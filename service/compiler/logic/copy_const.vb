
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Copy (instead of moving) a @data to @target
    Public Class copy_const
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

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            Dim t As variable = Nothing
            If variable.[New](scope, types, target, t) AndAlso
               t.is_assignable_from(data.value_bytes_size()) Then
                o.emplace_back(instruction_builder.str(command.cpc, t, data))
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace
