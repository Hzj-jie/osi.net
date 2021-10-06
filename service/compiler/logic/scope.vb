
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class scope
        Private ReadOnly parent As scope
        ' This stack can be a real stack, but using map with offset can provide a better lookup performance.
        Private ReadOnly stack As New unordered_map(Of String, stack_ref)()
        ' Heap allocations need no offset, but only type to check assignability.
        Private ReadOnly heap As New unordered_map(Of String, String)()
        Private child As scope = Nothing

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

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Sub New(ByVal parent As scope)
            Me.parent = parent
        End Sub

        Public Function start_scope() As scope
            assert(child Is Nothing)
            child = New scope(Me)
            Return child
        End Function

        Public Function end_scope() As scope
            If Not parent Is Nothing Then
                parent.child = Nothing
            End If
            Return parent
        End Function

        Public Function unique_name() As String
            Return strcat("@scope_", GetHashCode(), "_unique_name_", size() + uint32_1)
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
            stack.emplace(name, New stack_ref(size() + uint32_1, type))
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

        Public Function is_root() As Boolean
            Return parent Is Nothing
        End Function

        Public Function empty() As Boolean
            Return size() = uint32_0
        End Function

        Public Function size() As UInt32
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
                    size += s.size()
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
                    If Not data_ref.rel(CLng(s.size() - r.offset + size), d) Then
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
End Namespace
