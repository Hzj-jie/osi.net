
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.interpreter.primitive

Namespace logic
    ' A variable in stack.
    Public NotInheritable Class variable
        Public ReadOnly name As String
        Public ReadOnly index As [optional](Of variable)
        Public ReadOnly type As String
        Public ReadOnly size As [optional](Of UInt32)

        Private Sub New(ByVal types As types,
                        ByVal name As String,
                        ByVal index As [optional](Of variable),
                        ByVal type As String)
            assert(Not name.null_or_whitespace())
            assert(Not type.null_or_whitespace())
            Me.name = name
            Me.index = index
            Me.type = type
            Me.size = size_of(types, type)
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

        Public Shared Function name_of(ByVal array As String, ByVal index As String) As String
            assert(Not array.null_or_whitespace())
            array = array.Trim()
            assert(Not array.null_or_whitespace())
            assert(Not index.null_or_whitespace())
            index = index.Trim()
            assert(Not index.null_or_whitespace())
            Return strcat(array, character.left_mid_bracket, index, character.right_mid_bracket)
        End Function

        Public Shared Function heap_name_of_or_origin(ByVal array_with_index As String) As String
            assert(Not array_with_index.null_or_whitespace())
            array_with_index = array_with_index.Trim()
            assert(Not array_with_index.null_or_whitespace())
            ' TODO: More checks
            Dim i As Int32 = array_with_index.IndexOf(character.left_mid_bracket)
            If i = npos Then
                Return array_with_index
            End If
            assert(i > 0)
            Return array_with_index.Substring(0, i)
        End Function

        Public Shared Function [of](ByVal types As types,
                                    ByVal name As String,
                                    ByVal v As vector(Of String),
                                    ByRef o As variable) As Boolean
            'TODO: Decide if accessing heap ptr as stack variable or vice versa are allowed.
            assert(Not name.null_or_whitespace())
            name = name.Trim()
            assert(Not name.null_or_whitespace())
            Dim index_start As Int32 = name.IndexOf(character.left_mid_bracket)
            If index_start = npos Then
                If name.IndexOf(character.right_mid_bracket) <> npos Then
                    errors.invalid_variable_name(name, "Unexpected closing bracket.")
                    Return False
                End If
                Dim r As scope.exported_ref = Nothing
                If Not scope.current().export(name, r) Then
                    errors.variable_undefined(name)
                    Return False
                End If

                o = New variable(types, name, [optional].empty(Of variable), r.type)
                Return True
            Else
                If Not name.EndsWith(character.right_mid_bracket) Then
                    errors.invalid_variable_name(name, "Closing bracket is not at the end of the name.")
                    Return False
                End If
                If name.IndexOf(character.right_mid_bracket) < index_start Then
                    errors.invalid_variable_name(name, "Closing bracket is before openning bracket.")
                    Return False
                End If
                If index_start = name.Length() - 2 Then
                    errors.invalid_variable_name(name, "Empty index string.")
                    Return False
                End If
                Dim index As variable = Nothing
                If Not [of](types, name.Substring(index_start + 1, name.Length() - index_start - 2), v, index) Then
                    errors.invalid_variable_name(name, "Index cannot be parsed.")
                    Return False
                End If
                Dim ptr_name As String = scope.current().unique_name()
                assert(define.export(ptr_name, types.heap_ptr_type, v))
                Dim d As data_ref = scope.current().export(ptr_name).data_ref
                Dim r As scope.exported_ref = Nothing
                If Not scope.current().export(name.Substring(0, index_start), r) Then
                    errors.variable_undefined(name.Substring(0, index_start))
                    Return False
                End If
                v.emplace_back(instruction_builder.str(
                    command.add,
                    d,
                    r.data_ref.ToString(),
                    index.ToString()))
                o = New variable(types, ptr_name, [optional].of(index), +r.ref_type)
                Return True
            End If
        End Function

        ' Create a variable without retrieving @size from types. Consumers who use this constructor should not use
        ' is_assignable or similar functions.
        Public Shared Function [of](ByVal name As String, ByVal v As vector(Of String), ByRef o As variable) As Boolean
            Return [of](Nothing, name, v, o)
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

        Public Overrides Function ToString() As String
            Dim d As data_ref = scope.current().export(name).data_ref
            If index Then
                Return d.to_heap().ToString()
            End If
            Return d.ToString()
        End Function
    End Class
End Namespace
