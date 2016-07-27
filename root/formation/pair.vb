
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public NotInheritable Class pair(Of FT, ST)
    Implements IComparable(Of pair(Of FT, ST)), IComparable, ICloneable

    Public first As FT
    Public second As ST

    Private Sub New(ByVal first As FT, ByVal second As ST)
        Me.first = first
        Me.second = second
    End Sub

    Public Sub New()
    End Sub

    Public Shared Function make_pair(ByVal first As FT, ByVal second As ST) As pair(Of FT, ST)
        Return New pair(Of FT, ST)(copy_no_error(first), copy_no_error(second))
    End Function

    Public Shared Function make_pair(ByVal first As FT) As pair(Of FT, ST)
        Return make_pair(first, Nothing)
    End Function

    Public Shared Function make_pair(ByVal second As ST) As pair(Of FT, ST)
        Return make_pair(Nothing, second)
    End Function

    Public Shared Function make_pair() As pair(Of FT, ST)
        Return make_pair(Nothing, Nothing)
    End Function

    Public Shared Function emplace_make_pair(ByVal first As FT, ByVal second As ST) As pair(Of FT, ST)
        Return New pair(Of FT, ST)(first, second)
    End Function

    Public Shared Function emplace_make_pair(ByVal first As FT) As pair(Of FT, ST)
        Return emplace_make_pair(first, Nothing)
    End Function

    Public Shared Function emplace_make_pair(ByVal second As ST) As pair(Of FT, ST)
        Return emplace_make_pair(Nothing, second)
    End Function

    Public Shared Function emplace_make_pair() As pair(Of FT, ST)
        Return emplace_make_pair(Nothing, Nothing)
    End Function

    Public Shared Function move(ByVal that As pair(Of FT, ST)) As pair(Of FT, ST)
        If that Is Nothing Then
            Return Nothing
        Else
            Dim r As pair(Of FT, ST) = Nothing
            r = New pair(Of FT, ST)()
            r.first = that.first
            r.second = that.second
            that.first = Nothing
            that.second = Nothing
            Return r
        End If
    End Function

    Public Function CompareTo(ByVal other As pair(Of FT, ST)) As Int32 _
                             Implements IComparable(Of pair(Of FT, ST)).CompareTo
        Dim c As Int32 = 0
        c = object_compare(Me, other)
        If c = object_compare_undetermined Then
            assert(Not other Is Nothing)
            c = compare(first, other.first)
            If c = 0 Then
                Return compare(second, other.second)
            Else
                Return c
            End If
        Else
            Return c
        End If
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of pair(Of FT, ST))(obj, False))
    End Function

    Public Shared Operator =(ByVal this As pair(Of FT, ST), ByVal that As pair(Of FT, ST)) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator =(ByVal this As pair(Of FT, ST), ByVal that As Object) As Boolean
        Return compare(this, that) = 0
    End Operator

    Public Shared Operator <>(ByVal this As pair(Of FT, ST), ByVal that As pair(Of FT, ST)) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Shared Operator <>(ByVal this As pair(Of FT, ST), ByVal that As Object) As Boolean
        Return compare(this, that) <> 0
    End Operator

    Public Function Clone() As Object Implements System.ICloneable.Clone
        If Me Is Nothing Then
            Return Nothing
        Else
            Dim rtn As pair(Of FT, ST) = Nothing
            rtn = alloc(Me)
            copy(rtn.first, first)
            copy(rtn.second, second)
            Return rtn
        End If
    End Function

    Public Overrides Function ToString() As String
        Return Convert.ToString(first) + ", " + Convert.ToString(second)
    End Function

    Public Overrides Function GetHashCode() As Int32
        Return (If(first Is Nothing, 0, first.GetHashCode()) Xor If(second Is Nothing, 0, second.GetHashCode()))
    End Function

    Public Overrides Function Equals(ByVal obj As Object) As Boolean
        Return compare(Me, obj) = 0
    End Function
End Class

Public Module _pair
    Public Function make_pair(Of FT, ST)(ByVal first As FT, ByVal second As ST) As pair(Of FT, ST)
        Return pair(Of FT, ST).make_pair(first, second)
    End Function

    Public Function make_pair(Of FT, ST)(ByVal first As FT) As pair(Of FT, ST)
        Return pair(Of FT, ST).make_pair(first)
    End Function

    Public Function make_pair(Of FT, ST)(ByVal second As ST) As pair(Of FT, ST)
        Return pair(Of FT, ST).make_pair(second)
    End Function

    Public Function make_pair(Of FT, ST)() As pair(Of FT, ST)
        Return pair(Of FT, ST).make_pair()
    End Function

    Public Function emplace_make_pair(Of FT, ST)(ByVal first As FT, ByVal second As ST) As pair(Of FT, ST)
        Return pair(Of FT, ST).emplace_make_pair(first, second)
    End Function

    Public Function emplace_make_pair(Of FT, ST)(ByVal first As FT) As pair(Of FT, ST)
        Return pair(Of FT, ST).emplace_make_pair(first)
    End Function

    Public Function emplace_make_pair(Of FT, ST)(ByVal second As ST) As pair(Of FT, ST)
        Return pair(Of FT, ST).emplace_make_pair(second)
    End Function

    Public Function emplace_make_pair(Of FT, ST)() As pair(Of FT, ST)
        Return pair(Of FT, ST).emplace_make_pair()
    End Function

    <Extension()> Public Function to_array(Of T)(ByVal i() As pair(Of T, T)) As T(,)
        If isemptyarray(i) Then
            Return Nothing
        Else
            Dim r(,) As T = Nothing
            ReDim r(array_size(i) - 1, 2 - 1)
            For j As Int32 = 0 To array_size(i) - 1
                If Not i(j) Is Nothing Then
                    r(j, 0) = i(j).first
                    r(j, 1) = i(j).second
                End If
            Next
            Return r
        End If
    End Function

    <Extension()> Public Function to_two_dimensional_array(Of T)(ByVal i() As pair(Of T, T)) As T()()
        If isemptyarray(i) Then
            Return Nothing
        Else
            Dim r()() As T = Nothing
            ReDim r(array_size(i) - 1)
            For j As Int32 = 0 To array_size(i) - 1
                ReDim r(j)(2 - 1)
                r(j)(0) = i(j).first
                r(j)(1) = i(j).second
            Next
            Return r
        End If
    End Function
End Module
