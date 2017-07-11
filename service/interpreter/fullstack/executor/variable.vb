
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports def_bool = System.Boolean
Imports def_int = System.Int32
Imports def_float = System.Double
Imports def_char = System.Char
Imports def_string = System.String
Imports def_var = System.Object

Namespace fullstack.executor
    Public Class variable
        Implements IComparable(Of variable), IComparable

        Public ReadOnly type As type
        Public ReadOnly value As Object
        Public ReadOnly cvalues() As variable

        Public Sub New(ByVal value As def_bool)
            Me.type = type.bool
            Me.value = value
        End Sub

        Public Sub New(ByVal value As def_int)
            Me.type = type.int
            Me.value = value
        End Sub

        Public Sub New(ByVal value As def_float)
            Me.type = type.float
            Me.value = value
        End Sub

        Public Sub New(ByVal value As def_char)
            Me.type = type.char
            Me.value = value
        End Sub

        Public Sub New(ByVal value As def_string)
            Me.type = type.string
            Me.value = value
        End Sub

        Public Sub New(ByVal value As def_var)
            Me.type = type.var
            Me.value = value
        End Sub

        Public Sub New(ByVal type As type)
            assert(Not type Is Nothing)
            Me.type = type
        End Sub

        Public Sub New(ByVal type As type,
                       ByVal value As Object)
            assert(Not type Is Nothing)
            assert(Not type.is_struct())
#If DEBUG Then
            If type.is_bool() Then
                assert(TypeOf value Is def_bool)
            ElseIf type.is_float() Then
                assert(TypeOf value Is def_float)
            ElseIf type.is_int() Then
                assert(TypeOf value Is def_int)
            ElseIf type.is_char() Then
                assert(TypeOf value Is def_char)
            ElseIf type.is_string() Then
                assert(TypeOf value Is def_string)
            ElseIf type.is_var() Then
                assert(value Is Nothing OrElse TypeOf value Is def_var)
            Else
                assert(False)
            End If
#End If
            Me.type = type
            Me.value = value
        End Sub

        Public Sub New(ByVal type As type,
                       ByVal cvalues() As variable)
            assert(type.is_struct() OrElse type.is_var())
            assert(type.ctype_count() = array_size(cvalues))
            'type.ctype_count() should always be > 0
            assert(Not isemptyarray(cvalues))
            Me.type = type
            Me.cvalues = cvalues
        End Sub

        Public Function cvalue_count() As Int32
            Return array_size_i(cvalues)
        End Function

        Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
            Return CompareTo(cast(Of variable)(obj, False))
        End Function

        Public Function CompareTo(ByVal other As variable) As Int32 Implements IComparable(Of variable).CompareTo
            Dim c As Int32 = 0
            c = object_compare(Me, other)
            If c = object_compare_undetermined Then
                assert(Not other Is Nothing)
                If is_struct() AndAlso other.is_struct() Then
                    If cvalue_count() <> other.cvalue_count() Then
                        Return compare(cvalue_count(), other.cvalue_count())
                    Else
                        For i As Int32 = 0 To cvalue_count() - 1
                            c = compare(cvalues(i), other.cvalues(i))
                            If c <> 0 Then
                                Return c
                            End If
                        Next
                        Return 0
                    End If
                ElseIf is_struct() OrElse other.is_struct() Then
                    Return -1
                Else
                    Return compare(value, other.value)
                End If
            Else
                Return c
            End If
        End Function
    End Class

    Public Module _variable
        <Extension()> Public Function is_int(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_int() OrElse
                   (i.type.is_var() AndAlso TypeOf i.value Is def_int)
        End Function

        <Extension()> Public Function is_bool(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_bool() OrElse
                   (i.type.is_var() AndAlso TypeOf i.value Is def_bool)
        End Function

        <Extension()> Public Function is_float(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_float() OrElse
                   (i.type.is_var() AndAlso TypeOf i.value Is def_float)
        End Function

        <Extension()> Public Function is_char(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_char() OrElse
                   (i.type.is_var() AndAlso TypeOf i.value Is def_char)
        End Function

        <Extension()> Public Function is_string(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_string() OrElse
                   (i.type.is_var() AndAlso TypeOf i.value Is def_string)
        End Function

        <Extension()> Public Function is_var(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_var()
        End Function

        <Extension()> Public Function is_struct(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_struct() OrElse
                   (i.type.is_var() AndAlso Not isemptyarray(i.cvalues))
        End Function

        <Extension()> Public Function is_struct(ByVal i As variable, ByVal t As type) As Boolean
            assert(Not i Is Nothing)
            assert(Not t Is Nothing)
            assert(t.is_struct())
            If i.type.is_type(t) Then
                Return True
            ElseIf i.type.is_var() Then
                If i.cvalue_count() <> t.ctype_count() Then
                    Return False
                Else
                    For j As Int32 = 0 To i.cvalue_count() - 1
                        If Not i.cvalues(j).is_type(t.ctypes(j)) Then
                            Return False
                        End If
                    Next
                    Return True
                End If
            Else
                Return False
            End If
        End Function

        'a variable with type t can replace current variable
        <Extension()> Public Function is_type(ByVal i As variable, ByVal t As type) As Boolean
            assert(Not i Is Nothing)
            Return (t.is_bool() AndAlso i.is_bool()) OrElse
                   (t.is_char() AndAlso i.is_char()) OrElse
                   (t.is_float() AndAlso i.is_float()) OrElse
                   (t.is_int() AndAlso i.is_int()) OrElse
                   (t.is_string() AndAlso i.is_string()) OrElse
                   (t.is_var() AndAlso i.is_var()) OrElse
                   (t.is_struct() AndAlso i.is_struct(t))
        End Function

        <Extension()> Public Function int(ByVal i As variable, ByRef o As def_int) As Boolean
            assert(Not i Is Nothing)
            Return i.is_int() AndAlso
                   assert(cast(Of def_int)(i.value, o))
        End Function

        <Extension()> Public Function int(ByVal i As variable) As def_int
            Dim o As def_int = 0
            assert(i.int(o))
            Return o
        End Function

        <Extension()> Public Function bool(ByVal i As variable, ByRef o As def_bool) As Boolean
            assert(Not i Is Nothing)
            Return i.is_bool() AndAlso
                   assert(cast_to(Of Boolean)(i.value, o))
            'cast(Of def_bool)(i.value, o) will trigger require_assert overload
        End Function

        <Extension()> Public Function bool(ByVal i As variable) As def_bool
            Dim o As def_bool = False
            assert(i.bool(o))
            Return o
        End Function

        <Extension()> Public Function float(ByVal i As variable, ByRef o As def_float) As Boolean
            assert(Not i Is Nothing)
            Return i.is_float() AndAlso
                   assert(cast(Of def_float)(i.value, o))
        End Function

        <Extension()> Public Function float(ByVal i As variable) As def_float
            Dim o As def_float = 0
            assert(i.float(o))
            Return o
        End Function

        <Extension()> Public Function [char](ByVal i As variable, ByRef o As def_char) As Boolean
            assert(Not i Is Nothing)
            Return i.is_char() AndAlso
                   assert(cast(Of def_char)(i.value, o))
        End Function

        <Extension()> Public Function [char](ByVal i As variable) As def_char
            Dim o As def_char = character.null
            assert(i.char(o))
            Return o
        End Function

        <Extension()> Public Function [string](ByVal i As variable, ByRef o As def_string) As Boolean
            assert(Not i Is Nothing)
            Return i.is_string() AndAlso
                   assert(cast(Of def_string)(i.value, o))
        End Function

        <Extension()> Public Function [string](ByVal i As variable) As String
            Dim o As def_string = Nothing
            assert(i.string(o))
            Return o
        End Function

        <Extension()> Public Function var(ByVal i As variable, ByRef o As def_var) As Boolean
            assert(Not i Is Nothing)
            o = i.value
            Return True
        End Function

        <Extension()> Public Function var(ByVal i As variable) As def_var
            Dim o As def_var = Nothing
            assert(i.var(o))
            Return o
        End Function

        <Extension()> Public Function [true](ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Dim vb As def_bool = False
            Dim vi As def_int = 0
            Dim vf As def_float = 0
            Dim vc As def_char = character.null
            Dim vs As def_string = Nothing
            Dim vv As def_var = Nothing
            If i.bool(vb) Then
                Return vb
            ElseIf i.int(vi) Then
                Return vi <> 0
            ElseIf i.float(vf) Then
                Return vf <> 0
            ElseIf i.char(vc) Then
                Return vc <> character.null
            ElseIf i.string(vs) Then
                Return Not String.IsNullOrEmpty(vs)
            ElseIf i.is_struct() Then
                For j As Int32 = 0 To array_size_i(i.cvalues) - 1
                    If Not i.cvalues(j) Is Nothing Then
                        Return True
                    End If
                Next
                Return False
            ElseIf i.is_var() Then
                Return Not i.value Is Nothing
            Else
                Return assert(False)
            End If
        End Function

        <Extension()> Public Function [false](ByVal i As variable) As Boolean
            Return Not i.true()
        End Function
    End Module
End Namespace
