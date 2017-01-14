
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public Class scope_wrapper
        Implements IDisposable

        Private ReadOnly new_scope As scope
        Private ReadOnly o As vector(Of String)

        Public Sub New(ByVal scope As scope, ByVal o As vector(Of String))
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Me.o = o
            Me.new_scope = scope.start_scope()
        End Sub

        Public Shared Function [New](ByVal scope As scope, ByVal o As vector(Of String)) As scope_wrapper
            Return New scope_wrapper(scope, o)
        End Function

        Public Function scope() As scope
            Return new_scope
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            o.emplace_back(instruction_builder.str(command.popm, new_scope.size()))
        End Sub
    End Class
End Namespace
