
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.utils
Imports osi.service.cache.constants.mapheap_cache

Friend Class mapheap_cache(Of KEY_T As IComparable(Of KEY_T), VALUE_T)
    Implements islimcache2(Of KEY_T, VALUE_T)

    Private ReadOnly max_size As UInt64
    Private ReadOnly retire_ticks As UInt64
    Private ReadOnly update_ticks_when_refer As Boolean
    Private ReadOnly m As map(Of KEY_T, VALUE_T)
    Private ReadOnly mh As mapheap(Of KEY_T, reverse(Of UInt64))
    Private ReadOnly clock As tick_clock

    Public Sub New(Optional ByVal max_size As UInt64 = default_max_size,
                   Optional ByVal retire_ticks As UInt64 = default_retire_ticks,
                   Optional ByVal update_ticks_when_refer As Boolean = default_update_ticks_when_refer)
        assert(max_size > 0)
        Me.max_size = max_size
        Me.retire_ticks = retire_ticks
        Me.update_ticks_when_refer = update_ticks_when_refer
        m = New map(Of KEY_T, VALUE_T)()
        mh = New mapheap(Of KEY_T, reverse(Of UInt64))()
        clock = thread_static_resolver.resolve_or_default(default_tick_clock.instance)
        assert(Not clock Is Nothing)
    End Sub

    Private Function no_retire() As Boolean
        Return retire_ticks = no_retire_ticks
    End Function

    Private Function retired(ByVal d As reverse(Of UInt64)) As Boolean
        assert(Not d Is Nothing)
        Return Not no_retire() AndAlso clock.ticks() - (+d) >= retire_ticks
    End Function

    Private Function retired(ByVal i As mapheap(Of KEY_T, reverse(Of UInt64)).iterator) As Boolean
        assert(Not i.is_null())
        assert(i <> mh.end())
        Return retired((+i).first)
    End Function

    Private Sub update_refer_ticks(ByVal k As KEY_T)
        assert(mh.insert(k, reverse.[New](clock.ticks())))
    End Sub

    Public Sub clear() Implements islimcache2(Of KEY_T, VALUE_T).clear
        m.clear()
        mh.clear()
    End Sub

    Public Function [erase](ByVal key As KEY_T) As Boolean Implements islimcache2(Of KEY_T, VALUE_T).erase
        If m.erase(key) Then
            Return assert(mh.erase(key))
        Else
            Return False
        End If
    End Function

    Public Function foreach(ByVal d As _do(Of KEY_T, VALUE_T, Boolean, Boolean)) As Boolean _
                           Implements islimcache2(Of KEY_T, VALUE_T).foreach
        If d Is Nothing Then
            Return False
        Else
            Return m.foreach(Function(ByRef k As KEY_T, ByRef v As VALUE_T, ByRef c As Boolean) As Boolean
                                 If retired(mh.find(k)) Then
                                     assert([erase](k))
                                     c = True
                                     Return True
                                 Else
                                     Return d(k, v, c)
                                 End If
                             End Function)
        End If
    End Function

    Public Function [get](ByVal key As KEY_T, ByRef value As VALUE_T) As Boolean _
                         Implements islimcache2(Of KEY_T, VALUE_T).get
        Dim i As map(Of KEY_T, VALUE_T).iterator = Nothing
        i = m.find(key)
        If i = m.end() Then
            If isdebugmode() Then
                assert(mh.find(key) = mh.end())
            End If
            Return False
        Else
            Dim j As mapheap(Of KEY_T, reverse(Of UInt64)).iterator = Nothing
            j = mh.find(key)
            assert(j <> mh.end())
            If retired(j) Then
                assert([erase](key))
                Return False
            Else
                copy(value, (+i).second)
                If update_ticks_when_refer Then
                    update_refer_ticks(key)
                End If
                Return True
            End If
        End If
    End Function

    Public Sub [set](ByVal key As KEY_T, ByVal value As VALUE_T) Implements islimcache2(Of KEY_T, VALUE_T).set
        m(key) = value
        update_refer_ticks(key)
        If size() > max_size Then
            Dim oldname As KEY_T = Nothing
            mh.pop_front(oldname, Nothing)
            assert(Not oldname Is Nothing)
            assert(m.erase(oldname))
            assert(size() = max_size)
        End If
    End Sub

    Public Function size() As Int64 Implements islimcache2(Of KEY_T, VALUE_T).size
        If isdebugmode() Then
            assert(m.size() = mh.size())
        End If
        Return m.size()
    End Function

    Public Function empty() As Boolean Implements islimcache2(Of KEY_T, VALUE_T).empty
        If isdebugmode() Then
            assert(m.empty() = mh.empty())
        End If
        Return m.empty()
    End Function

    Public Function have(ByVal key As KEY_T) As Boolean Implements islimcache2(Of KEY_T, VALUE_T).have
        If isdebugmode() Then
            assert((m.find(key) = m.end()) = (mh.find(key) = mh.end()))
        End If
        Return m.find(key) <> m.end()
    End Function
End Class
