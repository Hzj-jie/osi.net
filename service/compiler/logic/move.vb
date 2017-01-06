
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' Copy (instead of moving) a variable from @source to @target.
    Public Class move
        Implements exportable

        Private ReadOnly types As types
        Private ReadOnly target As String
        Private ReadOnly source As String

        Public Sub New(ByVal types As types, ByVal target As String, ByVal source As String)
            assert(Not types Is Nothing)
            assert(Not String.IsNullOrEmpty(target))
            assert(Not String.IsNullOrEmpty(source))
            Me.types = types
            Me.target = target
            Me.source = source
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Dim target_ref As String = Nothing
            Dim target_type As String = Nothing
            If Not scope.export(target, target_ref) OrElse
               Not assert(scope.type(target, target_type)) Then
                errors.variable_undefined(target)
                Return False
            End If

            Dim source_ref As String = Nothing
            Dim source_type As String = Nothing
            If Not scope.export(source, source_ref) OrElse
               Not assert(scope.type(source, source_type)) Then
                errors.variable_undefined(source)
                Return False
            End If

            If types.is_assignable(source_type, target_type) Then
                o.emplace_back(instruction_builder.str(command.mov, target_ref, source_ref))
                Return True
            Else
                errors.unassignable(target, target_type, source, source_type)
                Return False
            End If
        End Function
    End Class
End Namespace
