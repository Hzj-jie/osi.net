
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class scope
        Private ReadOnly parent As scope
        Private ReadOnly m As New unordered_map(Of String, variable_ref)()
        Private child As scope = Nothing

        Private NotInheritable Class variable_ref
            'Starts from 1 to allow size()-top.offset=0.
            Public ReadOnly offset As UInt64
            Public ReadOnly type As String
            Public ReadOnly heap As Boolean

            Private Sub New(ByVal offset As UInt64, ByVal type As String, ByVal heap As Boolean)
                assert(Not type.null_or_whitespace())
                Me.offset = offset
                Me.type = type
                Me.heap = heap
            End Sub

            Public Shared Function of_stack(ByVal offset As UInt64, ByVal type As String) As variable_ref
                Return New variable_ref(offset, type, False)
            End Function

            Public Shared Function of_heap(ByVal offset As UInt64, ByVal type As String) As variable_ref
                Return New variable_ref(offset, type, True)
            End Function
        End Class

        Public NotInheritable Class var_ref
            Public ReadOnly data_ref As data_ref
            Public ReadOnly heap As Boolean

            Private Sub New(ByVal data_ref As data_ref, ByVal heap As Boolean)
                assert(Not data_ref Is Nothing)
                Me.data_ref = data_ref
                Me.heap = heap
            End Sub

            Private Shared Function [of](ByVal f As _do_val_ref(Of Int64, data_ref, Boolean),
                                         ByVal offset As Int64,
                                         ByVal heap As Boolean,
                                         ByRef o As var_ref) As Boolean
                assert(Not f Is Nothing)
                Dim d As data_ref = Nothing
                If Not f(offset, d) Then
                    Return False
                End If
                o = New var_ref(d, heap)
                Return True
            End Function

            Public Shared Function of_heap_abs(ByVal offset As Int64, ByRef o As var_ref) As Boolean
                Return [of](AddressOf data_ref.abs, offset, True, o)
            End Function

            Public Shared Function of_stack_abs(ByVal offset As Int64, ByRef o As var_ref) As Boolean
                Return [of](AddressOf data_ref.abs, offset, False, o)
            End Function

            Public Shared Function of_heap_rel(ByVal offset As Int64, ByRef o As var_ref) As Boolean
                Return [of](AddressOf data_ref.rel, offset, True, o)
            End Function

            Public Shared Function of_stack_rel(ByVal offset As Int64, ByRef o As var_ref) As Boolean
                Return [of](AddressOf data_ref.rel, offset, False, o)
            End Function
        End Class

        Public NotInheritable Class exported_var_ref
            Public ReadOnly ref As String
            Public ReadOnly heap As Boolean

            Private Sub New(ByVal ref As String, ByVal heap As Boolean)
                assert(Not String.IsNullOrWhiteSpace(ref))
                Me.ref = ref
                Me.heap = heap
            End Sub

            Public Shared Function [of](ByVal i As var_ref, ByRef o As exported_var_ref) As Boolean
                assert(Not i Is Nothing)
                Dim ref As String = Nothing
                If Not i.data_ref.export(ref) Then
                    Return False
                End If
                o = New exported_var_ref(ref, i.heap)
                Return True
            End Function
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

        Private Function define(ByVal name As String,
                                ByVal type As String,
                                ByVal f As Func(Of UInt32, String, variable_ref)) As Boolean
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            assert(Not f Is Nothing)
            If m.find(name) <> m.end() Then
                errors.redefine(name, type, Me.type(name))
                Return False
            End If
            m.emplace(name, f(size() + uint32_1, type))
            Return True
        End Function

        Public Function define(ByVal name As String, ByVal type As String) As Boolean
            Return define(name, type, AddressOf variable_ref.of_stack)
        End Function

        Public Function define_array(ByVal name As String, ByVal type As String) As Boolean
            Return define(name, type, AddressOf variable_ref.of_heap)
        End Function

        Public Function is_root() As Boolean
            Return parent Is Nothing
        End Function

        Public Function empty() As Boolean
            Return size() = uint32_0
        End Function

        Public Function size() As UInt32
            Return m.size()
        End Function

        Public Function type(ByVal name As String, ByRef o As String) As Boolean
            Dim s As scope = Me
            While Not s Is Nothing
                Dim r As variable_ref = Nothing
                If s.m.find(name, r) Then
                    o = r.type
                    Return True
                End If
                s = s.parent
            End While
            Return False
        End Function

        Public Function type(ByVal name As String) As String
            Dim o As String = Nothing
            assert(type(name, o))
            Return o
        End Function

        Public Function defined(ByVal name As String) As Boolean
            Dim o As var_ref = Nothing
            Return export(name, o)
        End Function

        Public Function export(ByVal name As String, ByRef o As var_ref) As Boolean
            Dim size As UInt64 = 0
            Dim s As scope = Me
            While Not s Is Nothing
                Dim r As variable_ref = Nothing
                If s.m.find(name, r) Then
                    Dim offset As Int64
                    If s.is_root() Then
                        ' To allow a callee to access global variables.
                        offset = CLng(r.offset - 1)
                        If r.heap Then
                            Return var_ref.of_heap_abs(offset, o)
                        End If
                        Return var_ref.of_stack_abs(offset, o)
                    End If
                    offset = CLng(s.size() - r.offset + size)
                    If r.heap Then
                        Return var_ref.of_heap_rel(offset, o)
                    End If
                    Return var_ref.of_stack_rel(offset, o)
                End If
                size += s.size()
                s = s.parent
            End While
            Return False
        End Function

        Public Function export(ByVal name As String, ByRef o As exported_var_ref) As Boolean
            Dim ref As var_ref = Nothing
            If Not export(name, ref) Then
                Return False
            End If
            Return exported_var_ref.of(ref, o)
        End Function
    End Class
End Namespace
