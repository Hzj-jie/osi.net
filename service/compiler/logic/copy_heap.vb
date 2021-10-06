
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation

Namespace logic
    Public MustInherit Class copy_heap
        Implements exportable

        Private ReadOnly anchors As anchors
        Private ReadOnly types As types
        Private ReadOnly array As String
        Private ReadOnly array_index As String
        Private ReadOnly stack As String
        Private ReadOnly type_match As Func(Of variable, variable, Boolean)
        Private ReadOnly operation As out_bool(Of String, String, String)

        Public Sub New(ByVal anchors As anchors,
                       ByVal types As types,
                       ByVal array As String,
                       ByVal array_index As String,
                       ByVal stack As String,
                       ByVal type_match As Func(Of variable, variable, Boolean),
                       ByVal operation As out_bool(Of String, String, String))
            assert(Not anchors Is Nothing)
            assert(Not types Is Nothing)
            assert(Not String.IsNullOrEmpty(array))
            assert(Not String.IsNullOrEmpty(array_index))
            assert(Not String.IsNullOrEmpty(stack))
            assert(Not type_match Is Nothing)
            assert(Not operation Is Nothing)
            Me.anchors = anchors
            Me.types = types
            Me.array = array
            Me.array_index = array_index
            Me.stack = stack
            Me.type_match = type_match
            Me.operation = operation
        End Sub

        Public Function export(ByVal scope As scope,
                               ByVal o As vector(Of String)) As Boolean Implements exportable.export
            assert(Not scope Is Nothing)
            assert(Not o Is Nothing)
            Dim array_ptr As String = scope.unique_name()
            If Not define.export(anchors, types, array_ptr, heaps.ptr_type, scope, o) Then
                Return False
            End If
            If Not New add(types, array_ptr, array, array_index).export(scope, o) Then
                Return False
            End If
            Dim stack_var As variable = Nothing
            If Not variable.of_stack(scope, types, stack, stack_var) Then
                Return False
            End If
            Dim heap_var As variable = Nothing
            If Not variable.of_heap(scope, types, array, heap_var) Then
                Return False
            End If
            If Not type_match(stack_var, heap_var) Then
                Return False
            End If
            Dim operation_str As String = Nothing
            If Not operation(array_ptr, stack_var.ref(), operation_str) Then
                Return False
            End If
            o.emplace_back(operation_str)
            Return True
        End Function
    End Class
End Namespace
