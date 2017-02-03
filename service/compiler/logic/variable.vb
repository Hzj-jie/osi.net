
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector

Namespace logic
    Public Class variable
        Private Const unknown_size As UInt32 = min_uint32
        Public ReadOnly name As String
        Public ReadOnly type As String
        Public ReadOnly ref As String
        Public ReadOnly size As UInt32

        Private Sub New(ByVal name As String, ByVal type As String, ByVal ref As String, ByVal size As UInt32)
            assert(Not String.IsNullOrEmpty(name))
            assert(Not String.IsNullOrEmpty(type))
            assert(Not String.IsNullOrEmpty(ref))
            Me.name = name
            Me.type = type
            Me.ref = ref
            Me.size = size
        End Sub

        Public Shared Function [New](ByVal scope As scope,
                                     ByVal types As types,
                                     ByVal name As String,
                                     ByRef o As variable) As Boolean
            assert(Not scope Is Nothing)
            assert(Not String.IsNullOrEmpty(name))
            Dim type As String = Nothing
            Dim ref As String = Nothing
            If Not scope.export(name, ref) OrElse Not assert(scope.type(name, type)) Then
                errors.variable_undefined(name)
                Return False
            End If

            Dim size As UInt32 = 0
            If types Is Nothing Then
                size = unknown_size
            ElseIf Not types.retrieve(type, size) Then
                errors.type_undefined(type, name)
                Return False
            End If

            o = New variable(name, type, ref, size)
            Return True
        End Function

        ' Create a variable without retrieving @size from types. Consumers who use this constructor should not use
        ' is_assignable or similar functions.
        Public Shared Function [New](ByVal scope As scope, ByVal name As String, ByRef o As variable) As Boolean
            Return [New](scope, Nothing, name, o)
        End Function

        Public Function is_assignable_from(ByVal source As variable) As Boolean
            assert(size <> unknown_size)
            assert(Not source Is Nothing)
            If types.is_size_or_variable(size, source.size) Then
                Return True
            Else
                errors.unassignable(Me, source)
                Return False
            End If
        End Function

        Public Function is_assignable_from(ByVal exp_size As UInt32) As Boolean
            assert(size <> unknown_size)
            If types.is_size_or_variable(size, exp_size) Then
                Return True
            Else
                errors.unassignable_array(Me, exp_size)
                Return False
            End If
        End Function

        Public Function is_assignable_from_uint32() As Boolean
            assert(size <> unknown_size)
            If types.is_size_or_variable(size, sizeof_uint32) Then
                Return True
            Else
                errors.unassignable_from_uint32(Me)
                Return False
            End If
        End Function

        Public Function is_assignable_to_uint32() As Boolean
            assert(size <> unknown_size)
            If size <= sizeof_uint32 OrElse types.is_variable_size(size) Then
                Return True
            Else
                errors.unassignable_to_uint32(Me)
                Return False
            End If
        End Function

        Public Function is_assignable_from_bool() As Boolean
            assert(size <> unknown_size)
            If types.is_size_or_variable(size, sizeof_bool_implementation) Then
                Return True
            Else
                errors.unassignable_from_bool(Me)
                Return False
            End If
        End Function

        Public Function is_variable_size() As Boolean
            assert(size <> unknown_size)
            If types.is_variable_size(size) Then
                Return True
            Else
                errors.unassignable_variable_size(Me)
                Return False
            End If
        End Function

        Public Overrides Function ToString() As String
            Return ref
        End Function
    End Class
End Namespace
