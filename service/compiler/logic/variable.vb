
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Namespace logic
    ' A variable in stack.
    Public NotInheritable Class variable
        Public ReadOnly name As String
        Public ReadOnly index As [optional](Of variable)
        Public ReadOnly type As String
        Public ReadOnly size As [optional](Of UInt32)

        Private Sub New(ByVal name As String,
                        ByVal index As [optional](Of variable),
                        ByVal type As String,
                        ByVal size As [optional](Of UInt32))
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            Me.name = name
            Me.type = type
            Me.size = size
        End Sub

        Private Shared Function size_of(ByVal types As types, ByVal type As String) As [optional](Of UInt32)
            If types Is Nothing Then
                Return [optional].empty(Of UInt32)()
            End If
            Dim size As UInt32 = 0
            ' type should be checked when a variable is defined in the scope.
            assert(types.retrieve(type, size))
            Return [optional].of(size)
        End Function

        Private Shared Function of_primitive(ByVal types As types,
                                             ByVal name As String,
                                             ByVal index As [optional](Of variable),
                                             ByRef o As variable) As Boolean
            assert(Not name.null_or_whitespace())
            If name.IndexOf(character.left_mid_bracket) <> npos Then
                errors.invalid_variable_name(name, "Unexpected openning bracket.")
                Return False
            End If
            If name.IndexOf(character.right_mid_bracket) <> npos Then
                errors.invalid_variable_name(name, "Unexpected closing bracket.")
                Return False
            End If
            Dim r As scope.exported_ref = Nothing
            If Not scope.current().export(name, r) Then
                errors.variable_undefined(name)
                Return False
            End If

            o = New variable(name, index, r.value_type(), size_of(types, r.value_type()))
            Return True
        End Function

        Public Shared Function [of](ByVal types As types,
                                    ByVal name As String,
                                    ByRef o As variable) As Boolean
            'TODO: Decide if accessing heap ptr as stack variable or vice versa are allowed.
            assert(Not name.null_or_whitespace())
            If name.IndexOf(character.left_mid_bracket) = npos Then
                Return of_primitive(types, name, [optional].empty(Of variable)(), o)
            End If
            Dim index_start As UInt32 = CUInt(name.IndexOf(character.left_mid_bracket))
            If name.IndexOf(character.right_mid_bracket) < index_start Then
                errors.invalid_variable_name(name, "Closing bracket is before openning bracket.")
                Return False
            End If
            Dim index_end As Int32 = name.LastIndexOf(character.right_mid_bracket)
            assert(index_end > index_start)
            If index_end - 1 = index_start Then
                errors.invalid_variable_name(name, "Empty index string.")
                Return False
            End If
            Dim index As variable = Nothing
            If Not [of](types, name.Substring(CInt(index_start + 1), CInt(index_end - index_start - 1)), index) Then
                errors.invalid_variable_name(name, "Index cannot be parsed.")
                Return False
            End If
            Return of_primitive(types, name.Substring(0, CInt(index_start)), [optional].of(index), o)
        End Function

        ' Create a variable without retrieving @size from types. Consumers who use this constructor should not use
        ' is_assignable or similar functions.
        Public Shared Function [of](ByVal name As String, ByRef o As variable) As Boolean
            Return [of](Nothing, name, o)
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

        Public Function is_assignable_from(ByVal source As variable) As Boolean
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

        Public Function export(ByVal o As vector(Of String),
                               ByVal f As Func(Of String, Boolean)) As Boolean
            assert(Not o Is Nothing)
            assert(Not f Is Nothing)
            If Not index Then
                ' This is a stack value, just use its data_ref.
                Dim r As String = Nothing
                assert(scope.current().export(name).data_ref.export(r))
                Return f(r)
            End If
            Return (+index).export(o,
                                   Function(ByVal index_str As String) As Boolean
                                       Dim ptr_name As String = scope.current().unique_name()
                                       If Not define.export(ptr_name, types.heap_ptr_type, o) Then
                                           Return False
                                       End If
                                       If Not add.export(ptr_name, name, index_str, o) Then
                                           Return False
                                       End If
                                       Return f(ptr_name)
                                   End Function)
        End Function

        Public Overrides Function ToString() As String
            assert(False)
            Return Nothing
        End Function
    End Class
End Namespace
