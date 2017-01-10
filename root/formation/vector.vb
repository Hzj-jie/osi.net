
Imports System.Text
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public Module vector_extension
    <Extension()> Public Sub renew(Of T)(ByRef v As vector(Of T))
        If v Is Nothing Then
            v = New vector(Of T)()
        Else
            v.clear()
        End If
    End Sub

    <Extension()> Public Function null_or_empty(Of T)(ByVal i As vector(Of T)) As Boolean
        Return i Is Nothing OrElse i.empty()
    End Function

    <Extension()> Public Function except(Of T As IComparable(Of T)) _
                                        (ByVal this As vector(Of T),
                                         ByVal that As vector(Of T)) As vector(Of T)
        If this.null_or_empty() Then
            Return Nothing
        ElseIf that.null_or_empty() Then
            Return copy(this)
        Else
            Dim s As [set](Of T) = Nothing
            s = New [set](Of T)()
            For i As UInt32 = 0 To that.size() - uint32_1
                s.insert(that(i))
            Next

            Dim rtn As vector(Of T) = Nothing
            rtn = New vector(Of T)()
            For i As UInt32 = 0 To this.size() - uint32_1
                If s.find(this(i)) = s.end() Then
                    rtn.push_back(this(i))
                End If
            Next

            Return rtn
        End If
    End Function

    <Extension()> Public Function modget(Of T)(ByVal v As vector(Of T), ByVal i As Int64) As T
        If v.null_or_empty() Then
            Return Nothing
        Else
            Return v(i Mod v.size())
        End If
    End Function

    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As Int32, ByRef o As T) As Boolean
        If available_index(v, i) Then
            o = v(i)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As UInt32, ByRef o As T) As Boolean
        If available_index(v, i) Then
            o = v(i)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As Int64, ByRef o As T) As Boolean
        If available_index(v, i) Then
            o = v(i)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function take(Of T)(ByVal v As vector(Of T), ByVal i As UInt64, ByRef o As T) As Boolean
        If available_index(v, i) Then
            o = v(i)
            Return True
        Else
            Return False
        End If
    End Function

    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As Int32) As Boolean
        Return Not v Is Nothing AndAlso i >= 0 AndAlso i < v.size()
    End Function

    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As UInt32) As Boolean
        Return Not v Is Nothing AndAlso i < v.size()
    End Function

    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As Int64) As Boolean
        Return Not v Is Nothing AndAlso i >= 0 AndAlso i < v.size()
    End Function

    <Extension()> Public Function available_index(Of T)(ByVal v As vector(Of T), ByVal i As UInt64) As Boolean
        Return Not v Is Nothing AndAlso i < v.size()
    End Function

    <Extension()> Public Function [erase](Of T)(ByVal v As vector(Of T), ByVal i As Int32) As Boolean
        If i < 0 Then
            Return False
        Else
            v.[erase](CUInt(i))
            Return True
        End If
    End Function

    <Extension()> Public Function [erase](Of T)(ByVal v As vector(Of T), ByVal d As T) As Boolean
        Return [erase](v, v.find(d))
    End Function

    <Extension()> Public Function push_back(Of T)(ByVal d As vector(Of T), ByVal s As vector(Of T)) As Boolean
        If d Is Nothing OrElse s.null_or_empty() Then
            Return False
        Else
            Return assert(d.push_back(s.data(), 0, s.size()))
        End If
    End Function

    <Extension()> Public Function emplace_back(Of T)(ByVal d As vector(Of T), ByVal s As vector(Of T)) As Boolean
        If d Is Nothing OrElse s.null_or_empty() Then
            Return False
        Else
            Return assert(d.emplace_back(s.data(), 0, s.size()))
        End If
    End Function

    <Extension()> Public Sub fill(Of T)(ByVal v As vector(Of t), ByRef d() As T)
        If Not v Is Nothing Then
            If v.empty() Then
                ReDim d(-1)
            Else
                If array_size(d) <> v.size() Then
                    ReDim d(v.size() - uint32_1)
                End If
                For i As UInt32 = 0 To v.size() - uint32_1
                    copy(d(i), v(i))
                Next
            End If
        End If
    End Sub

    <Extension()> Public Function find(Of T)(ByVal v As vector(Of T), ByVal k As T) As Int32
        If v.null_or_empty() Then
            Return npos
        Else
            For i As UInt32 = 0 To v.size() - uint32_1
                If compare(k, v(i)) = 0 Then
                    Return i
                End If
            Next
            Return npos
        End If
    End Function

    <Extension()> Public Function ToString(Of T)(ByVal v As vector(Of T), ByVal separator As String) As String
        If v Is Nothing Then
            Return Nothing
        ElseIf v.empty() Then
            Return String.Empty
        Else
            Dim r As StringBuilder = Nothing
            r = New StringBuilder()
            For i As UInt32 = 0 To v.size() - uint32_1
                If i > 0 Then
                    r.Append(separator)
                End If
                r.Append(v(i))
            Next
            Return Convert.ToString(r)
        End If
    End Function

    <Extension()> Public Function str(Of T)(ByVal v As vector(Of T), ByVal separator As String) As String
        Return ToString(v, separator)
    End Function

    <Extension()> Public Function str(Of T)(ByVal v As vector(Of T)) As String
        Return ToString(v, Nothing)
    End Function

    <Extension()> Public Function from(Of T)(ByRef v As vector(Of T), ByVal ParamArray vs() As T) As Boolean
        v.renew()
        Return v.push_back(vs)
    End Function
End Module

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
        v.resize(array_size(vs) - 1)
        assert(v.capacity() >= array_size(vs))
        For i As Int32 = 0 To array_size(vs) - 1
            copy(v(i), vs(i))
        Next
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
            memmove(v.data(), pos + 1, v.data(), pos, v.size() - pos - 1)
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
        [erase](start, start + 1)
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
        For i As UInt32 = os To n - 1
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
        If array_size(vs) <= start Then
            Return False
        ElseIf n = max_uint32 Then
            n = array_size(vs) - start
        ElseIf n = 0 Then
            Return True
        ElseIf start + n > array_size(vs) Then
            Return False
        End If

        v.reserve(n + size())
        For i As UInt32 = start To start + n - uint32_1
            Dim m As T = Nothing
            If need_copy Then
                copy(m, vs(i))
            Else
                m = vs(i)
            End If
            v.push_back(m)
        Next
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
        Return New vector(Of T)(v.clone())
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
        Return Me.ToString(character.tab)
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
