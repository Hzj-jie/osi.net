
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class vector(Of T)
    Implements ICloneable(Of vector(Of T)), ICloneable, IComparable(Of vector(Of T)), IComparable

    Private ReadOnly v As adaptive_array_t

    Public Sub New()
        v = New adaptive_array_t()
    End Sub

    Public Sub New(ByVal n As UInt32)
        v = New adaptive_array_t(n)
    End Sub

    Private Sub New(ByVal v As adaptive_array_t)
        Me.v = v
    End Sub

    Public Shared Function move(ByVal that As vector(Of T)) As vector(Of T)
        If that Is Nothing Then
            Return Nothing
        Else
            Return New vector(Of T)(adaptive_array_t.move(that.v))
        End If
    End Function

    Public Shared Function swap(ByVal this As vector(Of T), ByVal that As vector(Of T)) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        Else
            Return assert(adaptive_array_t.swap(this.v, that.v))
        End If
    End Function

    Public Sub assign(ByVal n As UInt32, ByVal d As T)
        v.clear()
        resize(n, d)
    End Sub

    Public Sub assign(ByVal ParamArray vs() As T)
        v.resize(array_size(vs))
        assert(v.capacity() >= array_size(vs))
        Dim i As UInt32 = 0
        While i < array_size(vs)
            copy(v(i), vs(CInt(i)))
            i += uint32_1
        End While
    End Sub

    Default Public Property at(ByVal p As UInt32) As T
        Get
            assert(p < v.size())
            Return v(p)
        End Get
        Set(ByVal value As T)
            assert(p < v.size())
            If object_compare(v(p), value) <> 0 Then
                copy(v(p), value)
            End If
        End Set
    End Property

    Public Function back() As T
        assert(Not v.empty())
        Return v.back()
    End Function

    Public Function capacity() As UInt32
        Return v.capacity()
    End Function

    Public Sub clear()
        v.clear()
    End Sub

    Public Function data() As T()
        Return v.data()
    End Function

    Public Sub emplace(ByVal pos As UInt32, ByVal d As T)
        assert(pos <= v.size())
        If pos = v.size() Then
            v.push_back(d)
        Else
            v.push_back(d)
            memmove(v.data(), pos + uint32_1, v.data(), pos, v.size() - pos - uint32_1)
            v(pos) = d
        End If
    End Sub

    Public Sub emplace_back(ByVal d As T)
        v.push_back(d)
    End Sub

    Public Function empty() As Boolean
        Return v.size() = 0
    End Function

    'remove elements between [start, end)
    Public Sub [erase](ByVal start As UInt32, ByVal [end] As UInt32)
        assert(start <= [end])
        assert(start < v.size())
        If start < [end] Then
            If [end] < size() Then
                'remove elements between [start, end)
                memmove(v.data(), start, v.data(), [end], v.size() - [end])
                v.resize(v.size() - ([end] - start))
            Else
                v.resize(start)
            End If
        End If
    End Sub

    Public Sub [erase](ByVal start As UInt32)
        [erase](start, start + uint32_1)
    End Sub

    Public Function front() As T
        assert(Not v.empty())
        Return v(0)
    End Function

    Public Sub insert(ByVal pos As UInt32, ByVal v As T)
        emplace(pos, copy_no_error(v))
    End Sub

    Public Function max_size() As UInt32
        Return v.max_size()
    End Function

    Public Sub pop_back()
        assert(Not v.empty())
        v.pop_back()
    End Sub

    Public Sub push_back(ByVal d As T)
        v.push_back(copy_no_error(d))
    End Sub

    Public Sub reserve(ByVal n As UInt32)
        v.reserve(n)
    End Sub

    Public Sub resize(ByVal n As UInt32)
        v.resize(n)
    End Sub

    Public Sub resize(ByVal n As UInt32, ByVal d As T)
        Dim os As UInt32 = 0
        os = v.size()
        v.resize(n)
        For i As UInt32 = os To n - uint32_1
            copy(v(i), d)
        Next
    End Sub

    Public Sub shrink_to_fit()
        v.shrink_to_fit()
    End Sub

    Public Function size() As UInt32
        Return v.size()
    End Function

    Public Sub swap(ByVal other As vector(Of T))
        assert(Not other Is Nothing)
        assert(adaptive_array_t.swap(v, other.v))
    End Sub

    'non-standard functions

    Private Function push_back(ByVal vs() As T,
                               ByVal start As UInt32,
                               ByVal n As UInt32,
                               ByVal need_copy As Boolean) As Boolean
        If n = max_uint32 Then
            n = array_size(vs) - start
        End If
        If n = 0 Then
            Return True
        End If
        If array_size(vs) <= start Then
            Return False
        End If
        If start + n > array_size(vs) Then
            Return False
        End If

        v.reserve(n + size())
        If need_copy Then
            For i As UInt32 = start To start + n - uint32_1
                Dim m As T = Nothing
                copy(m, vs(CInt(i)))
                v.push_back(m)
            Next
        Else
            For i As UInt32 = start To start + n - uint32_1
                Dim m As T = Nothing
                m = vs(CInt(i))
                v.push_back(m)
            Next
        End If
        Return True
    End Function

    Public Function emplace_back(ByVal vs() As T,
                                 Optional ByVal start As UInt32 = 0,
                                 Optional ByVal n As UInt32 = max_uint32) As Boolean
        Return push_back(vs, start, n, False)
    End Function

    Public Function push_back(ByVal vs() As T,
                              Optional ByVal start As UInt32 = 0,
                              Optional ByVal n As UInt32 = max_uint32) As Boolean
        Return push_back(vs, start, n, True)
    End Function

    '.net implementations
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    Public Function CloneT() As vector(Of T) Implements ICloneable(Of vector(Of T)).Clone
        Return New vector(Of T)(v.CloneT())
    End Function

    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of vector(Of T))(obj, False))
    End Function

    Public Function CompareTo(ByVal other As vector(Of T)) As Int32 Implements IComparable(Of vector(Of T)).CompareTo
        Dim c As Int32 = 0
        c = object_compare(Me, other)
        If c = object_compare_undetermined Then
            assert(Not other Is Nothing)
            Return adaptive_array_t.compare(v, other.v)
        Else
            Return c
        End If
    End Function

    Public Overrides Function ToString() As String
        Return str(character.tab)
    End Function

    Public Shared Operator +(ByVal this As vector(Of T)) As T()
        If this Is Nothing Then
            Return Nothing
        Else
            this.shrink_to_fit()
            Return this.data()
        End If
    End Operator
End Class
