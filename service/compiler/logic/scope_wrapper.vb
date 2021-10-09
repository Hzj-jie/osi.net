
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class scope_wrapper
        Implements IDisposable

        Private ReadOnly new_scope As scope
        Private ReadOnly o As vector(Of String)

        Public Sub New(ByVal scope As scope, ByVal o As vector(Of String))
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Me.o = o
            Me.new_scope = scope.start_scope()
        End Sub

        Public Function scope() As scope
            Return new_scope
        End Function

        Public Sub Dispose() Implements IDisposable.Dispose
            new_scope.move_heap().
                      stream().
                      map(Function(ByVal x As first_const_pair(Of String, String)) As String
                              assert(Not x Is Nothing)
                              Return x.first
                          End Function).
                      foreach(Sub(ByVal name As String)
                                  Dim v As variable = Nothing
                                  assert(variable.of_stack(new_scope, heaps.original_name_of(name), v))
                                  o.emplace_back(instruction_builder.str(command.dealloc, v))
                              End Sub)
            Dim i As UInt32 = 0
            While i < new_scope.stack_size()
                o.emplace_back(instruction_builder.str(command.pop))
                i += uint32_1
            End While
            new_scope.end_scope()
        End Sub
    End Class
End Namespace
