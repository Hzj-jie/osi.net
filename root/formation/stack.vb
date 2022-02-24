
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public NotInheritable Class stack(Of T)
    Implements ICloneable, ICloneable(Of stack(Of T)), IComparable, IComparable(Of stack(Of T))

    Private ReadOnly d As vector(Of T) = Nothing

    Public Sub clear()
        d.clear()
    End Sub

    Public Function empty() As Boolean
        Return d.empty()
    End Function

    Public Function size() As UInt32
        Return d.size()
    End Function

    Public Function back() As T
        Return d.back()
    End Function

    Public Sub push(ByVal v As T)
        d.push_back(v)
    End Sub

    Public Sub emplace(ByVal v As T)
        d.emplace_back(v)
    End Sub

    Public Sub pop()
        d.pop_back()
    End Sub

    Public Sub New()
        d = New vector(Of T)()
    End Sub

    Public Sub New(ByVal that As stack(Of T))
        d = copy_no_error(that.d)
    End Sub

    Public Sub New(ByVal that As vector(Of T))
        d = copy_no_error(that)
    End Sub

    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As stack(Of T) Implements ICloneable(Of stack(Of T)).Clone
        Return New stack(Of T)(d)
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of stack(Of T))(obj, False))
    End Function

    Public Function CompareTo(ByVal other As stack(Of T)) As Int32 Implements IComparable(Of stack(Of T)).CompareTo
        Dim cmp As Int32 = 0
        cmp = object_compare(Me, other)
        If cmp <> 0 Then
            Return cmp
        End If
        assert(Not other Is Nothing)
        Return d.CompareTo(other.d)
    End Function

    Public Overrides Function ToString() As String
        Return Convert.ToString(d)
    End Function
End Class
