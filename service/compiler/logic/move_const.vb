
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Copy (instead of moving) a @data to @target
    Public Class move_const
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
            Dim target_ref As String = Nothing
            Dim target_type As String = Nothing
            If Not scope.export(target, target_ref) OrElse
               Not assert(scope.type(target, target_type)) Then
                errors.variable_undefined(target)
                Return False
            End If

            If types.is_assignable(target_type, data.bytes_size()) Then
                o.emplace_back(instruction_builder.str(command.movc, target_ref, data.export()))
                Return True
            Else
                errors.unassignable_array(target, target_type, data.bytes_size())
                Return False
            End If
        End Function
    End Class
End Namespace
