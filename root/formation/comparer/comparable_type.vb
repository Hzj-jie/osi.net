
Option Explicit On
Option Infer Off
Option Strict On

#Const recursive_compare = False
Imports osi.root.connector

Public NotInheritable Class comparable_type
    Implements IComparable(Of comparable_type),
               IComparable(Of Type),
               IComparable,
               IEquatable(Of comparable_type),
               IEquatable(Of Type)

    Private Shared ReadOnly cmp As Func(Of Type, Type, Int32) =
        Function() As Func(Of Type, Type, Int32)
            If use_recursive_compare() Then
                Return Function(x As Type, y As Type) As Int32
                           Return x.GUID().CompareTo(y.GUID())
                       End Function
            End If
            Return Function(x As Type, y As Type) As Int32
                       Return strcmp(x.AssemblyQualifiedName(), y.AssemblyQualifiedName())
                   End Function
        End Function()
    Private ReadOnly t As Type

    Private Shared Function use_recursive_compare() As Boolean
#If recursive_compare Then
        ' mono does not support Type.GUID, it always returns an identical GUID.
        Return Not envs.mono
#Else
        Return False
#End If
    End Function

    Public Shared Function [New](Of T)() As comparable_type
        Return New comparable_type(GetType(T))
    End Function

    Public Sub New(ByVal i As Type)
        Me.t = i
    End Sub

    Public Sub New(ByVal i As comparable_type)
        Me.New(+i)
    End Sub

    Public Shared Function compare_type(ByVal i As Type, ByVal j As Type) As Int32
        Dim r As Int32 = 0
        r = object_compare(i, j)
        If r <> object_compare_undetermined Then
            Return r
        End If

        If Not use_recursive_compare() Then
            Return cmp(i, j)
        End If

        ' recursive_compare
        r = cmp(i, j)
        If r <> 0 Then
            Return r
        End If
        Dim gl() As Type = Nothing
        Dim gr() As Type = Nothing
        gl = i.GetGenericArguments()
        gr = j.GetGenericArguments()
        assert(array_size(gl) = array_size(gr))
        For k As Int32 = 0 To array_size_i(gl) - 1
            r = compare_type(gl(k), gr(k))
            If r <> 0 Then
                Return r
            End If
        Next
        Return 0
    End Function

    Public Shared Operator +(ByVal this As comparable_type) As Type
        If this Is Nothing Then
            Return Nothing
        End If
        Return this.t
    End Operator

    Public Shared Widening Operator CType(ByVal this As comparable_type) As Type
        Return +this
    End Operator

    Public Shared Widening Operator CType(ByVal this As Type) As comparable_type
        Return New comparable_type(this)
    End Operator

    Public Function CompareTo(ByVal other As comparable_type) As Int32 _
                             Implements IComparable(Of comparable_type).CompareTo
        Return compare_type(+Me, +other)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Dim a As comparable_type = Nothing
        If cast(Of comparable_type)(obj, a) Then
            Return CompareTo(a)
        End If
        Return CompareTo(cast(Of Type)(obj, False))
    End Function

    Public Function CompareTo(ByVal other As Type) As Int32 Implements IComparable(Of Type).CompareTo
        Return compare_type(+Me, other)
    End Function

    Public Overloads Function Equals(ByVal other As comparable_type) As Boolean _
                                    Implements IEquatable(Of comparable_type).Equals
        Return CompareTo(other) = 0
    End Function

    Public Overloads Function Equals(ByVal other As Type) As Boolean Implements IEquatable(Of Type).Equals
        Return CompareTo(other) = 0
    End Function

    Public Overrides Function GetHashCode() As Int32
        Return If(t Is Nothing, 0, t.GetHashCode())
    End Function

    Public Overrides Function ToString() As String
        Return If(t Is Nothing, Nothing, t.ToString())
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Dim a As comparable_type = Nothing
        If cast(obj, a) Then
            Return Equals(a)
        End If
        Return Equals(cast(Of Type)(obj, False))
    End Function
End Class
