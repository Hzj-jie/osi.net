﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    Public NotInheritable Class variable
        Private ReadOnly scope As scope
        Public ReadOnly name As String
        Public ReadOnly type As String
        Public ReadOnly size As [optional](Of UInt32)
        Public ReadOnly heap As Boolean

        Private Sub New(ByVal scope As scope,
                        ByVal name As String,
                        ByVal v As scope.var_ref,
                        ByVal size As [optional](Of UInt32))
            assert(Not scope Is Nothing)
            assert(Not name.null_or_whitespace())
            assert(Not v Is Nothing)
            assert(Not v.type.null_or_whitespace())
            Me.scope = scope
            Me.name = name
            Me.type = v.type
            Me.size = size
            Me.heap = v.heap
        End Sub

        Public Shared Function [New](ByVal scope As scope,
                                     ByVal types As types,
                                     ByVal name As String,
                                     ByRef o As variable) As Boolean
            assert(Not scope Is Nothing)
            assert(Not name.null_or_whitespace())
            Dim v As scope.var_ref = Nothing
            If Not scope.export(name, v) Then
                errors.variable_undefined(name)
                Return False
            End If

            If types Is Nothing Then
                o = New variable(scope, name, v, [optional].empty(Of UInt32)())
                Return True
            End If
            Dim size As UInt32 = 0
            If Not types.retrieve(v.type, size) Then
                errors.type_undefined(v.type, name)
                Return False
            End If
            o = New variable(scope, name, v, [optional].of(size))
            Return True
        End Function

        ' Create a variable without retrieving @size from types. Consumers who use this constructor should not use
        ' is_assignable or similar functions.
        Public Shared Function [New](ByVal scope As scope, ByVal name As String, ByRef o As variable) As Boolean
            Return [New](scope, Nothing, name, o)
        End Function

        Private Function is_zero_size() As Boolean
            assert(size)
            If types.is_zero_size(+size) Then
                errors.unassignable_zero_type(Me)
                Return True
            End If
            Return False
        End Function

        Private Function is_assignable_from_size(ByVal exp_size As UInt32) As Boolean
            assert(size)
            If is_zero_size() Then
                Return False
            End If
            If types.is_size_or_variable(+size, exp_size) Then
                Return True
            End If
            ' TODO: Should this be allowed?
            If (+size) >= exp_size Then
                Return True
            End If
            Return False
        End Function

        Private Function is_assignable_from(ByVal source As variable) As Boolean
            assert(Not source Is Nothing)
            assert(source.size)
            If is_assignable_from_size(+(source.size)) Then
                Return True
            End If
            errors.unassignable(Me, source)
            Return False
        End Function

        Public Function is_assignable_from(ByVal exp_size As UInt32) As Boolean
            If is_assignable_from_size(exp_size) Then
                Return True
            End If
            errors.unassignable_array(Me, exp_size)
            Return False
        End Function

        Public Function is_assignable_from_uint32() As Boolean
            If is_assignable_from_size(sizeof_uint32) Then
                Return True
            End If
            errors.unassignable_from_uint32(Me)
            Return False
        End Function

        Public Function is_assignable_to_uint32() As Boolean
            assert(size)
            If is_zero_size() Then
                Return False
            End If
            If (+size) <= sizeof_uint32 OrElse types.is_variable_size(+size) Then
                Return True
            End If
            errors.unassignable_to_uint32(Me)
            Return False
        End Function

        Public Function is_assignable_from_bool() As Boolean
            If is_assignable_from_size(sizeof_bool_implementation) Then
                Return True
            End If
            errors.unassignable_from_bool(Me)
            Return False
        End Function

        Public Function is_variable_size() As Boolean
            assert(size)
            If is_zero_size() Then
                Return False
            End If
            If types.is_variable_size(+size) Then
                Return True
            End If
            errors.unassignable_variable_size(Me)
            Return False
        End Function

        Public Function copy_or_move_from(ByVal i As variable, ByVal ins As command, ByRef o As String) As Boolean
            assert(Not i Is Nothing)
            If is_assignable_from(i) Then
                o = instruction_builder.str(ins, Me, i)
                Return True
            End If
            Return False
        End Function

        Public Function copy_from(ByVal i As variable, ByRef o As String) As Boolean
            Return copy_or_move_from(i, command.cp, o)
        End Function

        Public Function move_from(ByVal i As variable, ByRef o As String) As Boolean
            Return copy_or_move_from(i, command.mov, o)
        End Function

        Public Function ref() As String
            Dim r As scope.exported_var_ref = Nothing
            assert(scope.export(name, r))
            Return r.ref
        End Function

        Public Overrides Function ToString() As String
            Return ref()
        End Function
    End Class
End Namespace
