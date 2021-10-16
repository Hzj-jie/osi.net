
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.constructor
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class scope
        Inherits scope(Of scope)

        ' This stack can be a real stack, but using map with offset can provide a better lookup performance.
        Private ReadOnly stack As New unordered_map(Of String, stack_ref)()
        ' Heap allocations need no offset, but only type to check assignability.
        Private ReadOnly heap As New unordered_map(Of String, String)()

        Private NotInheritable Class stack_ref
            'Starts from 1 to allow size()-top.offset=0.
            Public ReadOnly offset As UInt64
            Public ReadOnly type As String

            Public Sub New(ByVal offset As UInt64, ByVal type As String)
                assert(Not type.null_or_whitespace())
                Me.offset = offset
                Me.type = type
            End Sub
        End Class

        Public NotInheritable Class exported_stack_ref
            Public ReadOnly data_ref As data_ref
            Public ReadOnly type As String

            Public Sub New(ByVal data_ref As data_ref, ByVal type As String)
                assert(Not data_ref Is Nothing)
                assert(Not type.null_or_whitespace())
                Me.data_ref = data_ref
                Me.type = type
            End Sub
        End Class

        <inject_constructor>
        Public Sub New(ByVal parent As scope)
            MyBase.New(parent)
        End Sub

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Protected Overrides Sub when_end_scope()
            ' The root scope needs not to clear the heap, the memory will be released when the root scope is running out
            ' of the scope, e.g. the process finished.
            If Not is_root() Then
                assert(heap.empty())
            End If
            MyBase.when_end_scope()
        End Sub

        Public Function move_heap() As unordered_map(Of String, String)
            Return unordered_map(Of String, String).move(heap)
        End Function

        Public Function unique_name() As String
            Return strcat("@scope_", GetHashCode(), "_unique_name_", stack_size() + uint32_1)
        End Function

        Private Function find_duplication(ByVal name As String, ByVal type As String) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            If stack.find(name) <> stack.end() OrElse heap.find(name) <> heap.end() Then
                errors.redefine(name, type, Me.type(name))
                Return True
            End If
            Return False
        End Function

        Public Function define_stack(ByVal name As String, ByVal type As String) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            If find_duplication(name, type) Then
                Return False
            End If
            stack.emplace(name, New stack_ref(stack_size() + uint32_1, type))
            Return True
        End Function

        Public Function define_heap(ByVal name As String, ByVal type As String) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            name = heaps.name_of(name)
            If find_duplication(name, type) Then
                Return False
            End If
            heap.emplace(name, type)
            Return True
        End Function

        Public Function stack_empty() As Boolean
            Return stack_size() = uint32_0
        End Function

        Public Function stack_size() As UInt32
            Return stack.size()
        End Function

        Public Function type(ByVal name As String, ByRef o As String) As Boolean
            Dim s As scope = Me
            While Not s Is Nothing
                Using code_block
                    Dim r As stack_ref = Nothing
                    If s.stack.find(name, r) Then
                        o = r.type
                        Return True
                    End If
                End Using
                Using code_block
                    Dim r As String = Nothing
                    If s.heap.find(name, r) Then
                        o = r
                        Return True
                    End If
                End Using
                s = s.parent
            End While
            Return False
        End Function

        Public Function type(ByVal name As String) As String
            Dim o As String = Nothing
            assert(type(name, o))
            Return o
        End Function

        Public Function export(ByVal name As String, ByRef o As exported_stack_ref) As Boolean
            Dim size As UInt64 = 0
            Dim s As scope = Me
            While Not s Is Nothing
                Dim r As stack_ref = Nothing
                If Not s.stack.find(name, r) Then
                    size += s.stack_size()
                    s = s.parent
                    Continue While
                End If
                Dim d As data_ref = Nothing
                If s.is_root() Then
                    ' To allow a callee to access global variables.
                    If Not data_ref.abs(CLng(r.offset - 1), d) Then
                        Return False
                    End If
                Else
                    If Not data_ref.rel(CLng(s.stack_size() - r.offset + size), d) Then
                        Return False
                    End If
                End If
                o = New exported_stack_ref(d, r.type)
                Return True
            End While
            Return False
        End Function

        Public Function export(ByVal name As String, ByRef o As String) As Boolean
            Dim ref As exported_stack_ref = Nothing
            If Not export(name, ref) Then
                Return False
            End If
            Return ref.data_ref.export(o)
        End Function
    End Class

    Public NotInheritable Class scope_wrapper
        Inherits scope_wrapper(Of scope)

        Private ReadOnly o As vector(Of String)

        Public Sub New(ByVal o As vector(Of String))
            MyBase.New(logic.scope.current())
            assert(Not o Is Nothing)
            Me.o = o
        End Sub

        Protected Overrides Sub when_dispose()
            new_scope.move_heap().
                      stream().
                      map(Function(ByVal x As first_const_pair(Of String, String)) As String
                              assert(Not x Is Nothing)
                              Return x.first
                          End Function).
                      foreach(Sub(ByVal name As String)
                                  Dim v As variable = Nothing
                                  assert(variable.of_stack(heaps.original_name_of(name), v))
                                  o.emplace_back(instruction_builder.str(command.dealloc, v))
                              End Sub)
            Dim i As UInt32 = 0
            While i < new_scope.stack_size()
                o.emplace_back(instruction_builder.str(command.pop))
                i += uint32_1
            End While
            MyBase.when_dispose()
        End Sub
    End Class
End Namespace
