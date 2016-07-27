
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.formation

Friend Class mapheap_case
    Inherits random_run_case

    Private ReadOnly mh As mapheap(Of String, Int64)
    Private ReadOnly m As map(Of String, Int64)
    Private ReadOnly c As map(Of Int64, Int64)

    Public Sub New(ByVal validate As Boolean)
        mh = New mapheap(Of String, Int64)()
        If validate Then
            m = New map(Of String, Int64)()
            c = New map(Of Int64, Int64)()
        End If
        insert_call(0.33, AddressOf insert)
        insert_call(0.3, AddressOf accumulate)
        insert_call(0.1, AddressOf pop_front)
        insert_call(0.12, AddressOf find)
        insert_call(0.1, AddressOf [erase])
        insert_call(0.01, AddressOf clone)
        insert_call(0.04, AddressOf clear)
    End Sub

    Public Overrides Function finish() As Boolean
        mh.clear()
        If validation() Then
            m.clear()
            c.clear()
        End If
        Return MyBase.finish()
    End Function

    Private Function validation() As Boolean
        assert((m Is Nothing) = (c Is Nothing))
        Return Not m Is Nothing
    End Function

    Private Shared Function rnd_key() As String
        Return strleft(guid_str(), 4)
    End Function

    Private Shared Function key_value(ByVal k As String) As Int64
        Return signing(k)
    End Function

    Private Shared Function next_value(ByVal v As Int64) As Int64
        Return left_shift(v, 1)
    End Function

    Private Shared Function is_key_value(ByVal k As String, ByVal v As Int64) As Boolean
        Dim i As Int64 = 0
        i = key_value(k)
        Dim b As Int64 = 0
        b = i
        Do
            If i = v Then
                Return True
            Else
                i = next_value(i)
            End If
        Loop Until i = b
        Return False
    End Function

    Private Shared Sub rnd_key_value(ByRef k As String, ByRef v As Int64)
        k = rnd_key()
        v = key_value(k)
    End Sub

    Private Sub confirm_one_key()
        If mh.empty() Then
            insert()
        End If
        assert(Not mh.empty())
    End Sub

    Private Sub insert()
        Dim k As String = Nothing
        Dim v As Int64 = 0
        rnd_key_value(k, v)
        assert_true(mh.insert(k, v))
        If validation() Then
            Dim it As map(Of String, Int64).iterator = Nothing
            it = m.find(k)
            If it <> m.end() Then
                dec((+it).second)
            End If
            assert_true(m.insert(k, v) <> m.end())
            c(v) += 1
        End If
    End Sub

    Private Sub accumulate()
        Dim k As String = Nothing
        k = rnd_key()
        If validation() Then
            Dim it As mapheap(Of String, Int64).iterator = Nothing
            it = mh.find(k)
            Dim v As Int64 = 0
            If it = mh.end() Then
                assert_true(m.find(k) = m.end())
                v = key_value(k)
                assert_true(mh.accumulate(k, v))
                m(k) = v
                c(v) += 1
            Else
                Dim ov As Int64 = 0
                ov = (+it).first
                v = next_value(ov)
                assert_true(mh.accumulate(k, v - ov))
                m(k) = v
                dec(ov)
                c(v) += 1
            End If
        Else
            mh.accumulate(k, 1)
        End If
    End Sub

    Private Sub dec(ByVal v As Int64)
        c(v) -= 1
        assert_more_or_equal(c(v), 0)
        If c(v) = 0 Then
            assert(c.erase(v))
        End If
    End Sub

    Private Sub pop_front()
        confirm_one_key()
        Dim k As String = Nothing
        Dim v As Int64 = 0
        mh.pop_front(k, v)
        assert_not_nothing(k)
        If validation() Then
            assert_true(is_key_value(k, v))
            assert_true(c.find(v) <> c.end())
            Dim it As map(Of Int64, Int64).iterator = Nothing
            it = c.begin()
            Dim max As Int64 = min_int64
            While it <> c.end()
                If (+it).first > max Then
                    max = (+it).first
                End If
                it += 1
            End While
            assert_equal(max, v)
            assert_true(m.erase(k))
            dec(v)
        End If
    End Sub

    Private Sub find()
        Dim k As String = Nothing
        k = rnd_key()
        Dim it As mapheap(Of String, Int64).iterator = Nothing
        it = mh.find(k)
        If validation() Then
            assert_equal(it = mh.end(), m.find(k) = m.end())
        End If
    End Sub

    Private Sub [erase]()
        Dim k As String = Nothing
        k = rnd_key()
        Dim r As Boolean = False
        r = mh.erase(k)
        If validation() Then
            If r Then
                assert_not_equal(m.find(k), m.end())
                dec(m(k))
                assert_equal(r, m.erase(k))
            Else
                assert_equal(m.find(k), m.end())
            End If
        End If
    End Sub

    Private Sub foreach(ByVal i As mapheap(Of String, Int64))
        If validation() Then
            assert(Not i Is Nothing)
            assert_equal(m.size(), i.size())
            Dim it As map(Of String, Int64).iterator = Nothing
            it = m.begin()
            While it <> m.end()
                Dim it2 As mapheap(Of String, Int64).iterator = Nothing
                it2 = mh.find((+it).first)
                If assert_not_equal(it2, mh.end()) Then
                    assert_equal((+it2).first, (+it).second)
                End If
                it += 1
            End While
        End If
    End Sub

    Private Sub clear()
        foreach(mh)
        mh.clear()
        If validation() Then
            m.clear()
            c.clear()
            assert_equal(mh.size(), 0)
            assert_true(mh.empty())
        End If
    End Sub

    Private Sub clone()
        Dim o As mapheap(Of String, Int64) = Nothing
        copy(o, mh)
        assert_equal(object_compare(o, mh), object_compare_undetermined)
        foreach(o)
        foreach(mh)
    End Sub
End Class
