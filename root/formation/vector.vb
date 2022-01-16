
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class vector(Of T)
    Implements ICloneable(Of vector(Of T)), ICloneable, IComparable(Of vector(Of T)), IComparable

    Private ReadOnly v As adaptive_array_t

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New()
        Me.New(New adaptive_array_t())
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub New(ByVal n As UInt32)
        Me.New(New adaptive_array_t(n))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub New(ByVal v As adaptive_array_t)
        Me.v = v
    End Sub

    Public Shared Function move(ByVal that As vector(Of T)) As vector(Of T)
        If that Is Nothing Then
            Return Nothing
        End If
        Return New vector(Of T)(adaptive_array_t.move(that.v))
    End Function

    Public Shared Function swap(ByVal this As vector(Of T), ByVal that As vector(Of T)) As Boolean
        If this Is Nothing OrElse that Is Nothing Then
            Return False
        End If
        Return assert(adaptive_array_t.swap(this.v, that.v))
    End Function

    Public Sub assign(ByVal n As UInt32, ByVal d As T)
        clear()
        resize(n, d)
    End Sub

    Public Sub assign(ByVal ParamArray vs() As T)
        resize(array_size(vs))
        assert(capacity() >= array_size(vs))
        Dim i As UInt32 = 0
        While i < array_size(vs)
            v.set(i, copy_no_error(vs(CInt(i))))
            i += uint32_1
        End While
    End Sub

    Default Public Property at(ByVal p As UInt32) As T
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Get
            assert(p < size())
            Return v.get(p)
        End Get
        <MethodImpl(method_impl_options.aggressive_inlining)>
        Set(ByVal value As T)
            assert(p < size())
            v.set(p, copy_no_error(value))
        End Set
    End Property

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function begin() As iterator
        If empty() Then
            Return [end]()
        End If
        Return New iterator(New ref(Me, 0))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function rbegin() As iterator
        If empty() Then
            Return [end]()
        End If
        Return New iterator(New ref(Me, size() - uint32_1))
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function [end]() As iterator
        Return iterator.end
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function back() As T
        assert(Not empty())
        Return v.back()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function capacity() As UInt32
        Return v.capacity()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub clear()
        v.clear()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function data() As T()
        Return v.data()
    End Function

    Public Sub emplace(ByVal pos As UInt32, ByVal d As T)
        assert(pos <= size())
        If pos = size() Then
            emplace_back(d)
        Else
            emplace_back(d)
            arrays.copy(data(), pos + uint32_1, data(), pos, size() - pos - uint32_1)
            v.set(pos, d)
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub emplace_back(ByVal d As T)
        v.push_back(d)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function empty() As Boolean
        Return v.size() = 0
    End Function

    'remove elements between [start, end)
    Public Sub [erase](ByVal start As UInt32, ByVal [end] As UInt32)
        assert(start <= [end])
        assert(start < size())
        If start < [end] Then
            If [end] < size() Then
                'remove elements between [start, end)
                arrays.copy(data(), start, data(), [end], size() - [end])
                resize(size() - ([end] - start))
            Else
                resize(start)
            End If
        End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub [erase](ByVal start As UInt32)
        [erase](start, start + uint32_1)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function front() As T
        assert(Not empty())
        Return v.get(0)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub insert(ByVal pos As UInt32, ByVal v As T)
        emplace(pos, copy_no_error(v))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function max_size() As UInt32
        Return v.max_size()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub pop_back()
        assert(Not empty())
        v.pop_back()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub push_back(ByVal d As T)
        v.push_back(copy_no_error(d))
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub reserve(ByVal n As UInt32)
        v.reserve(n)
    End Sub

    Public Sub resize(ByVal n As UInt32)
        v.resize(n)
    End Sub

    Public Sub resize(ByVal n As UInt32, ByVal d As T)
        Dim os As UInt32 = size()
        v.resize(n)
        For i As UInt32 = os To n - uint32_1
            v.set(i, copy_no_error(d))
        Next
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function shrink_to_fit() As Boolean
        Return v.shrink_to_fit()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function size() As UInt32
        Return v.size()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
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

        reserve(n + size())
        If need_copy Then
            For i As UInt32 = start To start + n - uint32_1
                Dim m As T = copy_no_error(vs(CInt(i)))
                emplace_back(m)
            Next
        Else
            For i As UInt32 = start To start + n - uint32_1
                Dim m As T = vs(CInt(i))
                emplace_back(m)
            Next
        End If
        Return True
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function emplace_back(ByVal vs() As T,
                                 Optional ByVal start As UInt32 = 0,
                                 Optional ByVal n As UInt32 = max_uint32) As Boolean
        Return push_back(vs, start, n, False)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function push_back(ByVal vs() As T,
                              Optional ByVal start As UInt32 = 0,
                              Optional ByVal n As UInt32 = max_uint32) As Boolean
        Return push_back(vs, start, n, True)
    End Function

    '.net implementations
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function Clone() As Object Implements ICloneable.Clone
        Return CloneT()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CloneT() As vector(Of T) Implements ICloneable(Of vector(Of T)).Clone
        Return New vector(Of T)(v.CloneT())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function CompareTo(ByVal obj As Object) As Int32 Implements IComparable.CompareTo
        Return CompareTo(cast(Of vector(Of T))(obj, False))
    End Function

    Public Function CompareTo(ByVal other As vector(Of T)) As Int32 Implements IComparable(Of vector(Of T)).CompareTo
        Dim c As Int32 = object_compare(Me, other)
        If c <> object_compare_undetermined Then
            Return c
        End If
        assert(Not other Is Nothing)
        Return adaptive_array_t.compare(v, other.v)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overrides Function ToString() As String
        Return str(character.blank)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Overrides Function GetHashCode() As Int32
        Return v.data().hash(size())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Operator +(ByVal this As vector(Of T)) As T()
        If this Is Nothing Then
            Return Nothing
        End If
        this.shrink_to_fit()
        Return this.data()
    End Operator

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub replace(ByVal i As UInt32, ByVal value As T)
        assert(i < size())
        If object_compare(v.get(i), value) <> 0 Then
            v.set(i, value)
        End If
    End Sub
End Class
