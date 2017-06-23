
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with unordered_map_test.vbp ----------
'so change unordered_map_test.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map_case.vbp ----------
'so change map_case.vbp instead of this file


#Const first_with_brackets = False

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.delegates

Friend Class unordered_map_case
    Inherits random_run_case

    Private ReadOnly m As unordered_map(Of String, UInt32) = Nothing
    Private ReadOnly v As [set](Of String) = Nothing

    Private Function validate() As Boolean
        Return Not v Is Nothing
    End Function

    Private Shared Function rnd_key() As String
        Return guid_str()
    End Function

    Private Shared Function value(ByVal k As String) As UInt32
        Return signing(k)
    End Function

    Public Sub New(ByVal validate As Boolean)
        m = New unordered_map(Of String, UInt32)()
        If validate Then
            v = New [set](Of String)()
        End If
        insert_call(0.32, AddressOf insert)
        insert_call(0.15, AddressOf [set])
        insert_call(0.15, AddressOf [get])
        insert_call(0.2, AddressOf [erase])
        insert_call(0.11, AddressOf find)
        insert_call(0.01, AddressOf clone)
        insert_call(0.06, AddressOf clear)
    End Sub

    Public Overrides Function finish() As Boolean
        m.clear()
        If validate() Then
            v.clear()
        End If
        Return MyBase.finish()
    End Function

    Private Sub insert()
        Dim k As String = Nothing
        k = rnd_key()
        assert_equal(m.emplace(k, value(k)).first, m.find(k))
        If validate() Then
            v.emplace(k)
        End If
    End Sub

    Private Function rnd_it() As [set](Of String).iterator
        assert(validate())
        assert(Not v.empty())
        Dim it As [set](Of String).iterator = Nothing
        it = v.begin()
        For i As Int32 = 0 To rnd_int(0, CInt(v.size())) - 1
            it += 1
        Next
        assert(it <> v.end())
        Return it
    End Function

    Private Sub confirm_not_empty()
        assert(validate())
        If v.empty() Then
            insert()
        End If
        assert(Not v.empty())
    End Sub

    Private Sub [set]()
        If validate() Then
            confirm_not_empty()
            Dim it As [set](Of String).iterator = Nothing
            it = rnd_it()
            assert_equal(m(+it), value(+it))
            Dim nv As UInt32 = 0
            nv = value(rnd_key())
            m(+it) = nv
            assert_equal(m(+it), nv)
            m(+it) = value(+it)
            assert_equal(m(+it), value(+it))
        Else
            m(rnd_key()) = value(rnd_key())
        End If
    End Sub

    Private Sub [get]()
        If validate() Then
            confirm_not_empty()
            Dim it As [set](Of String).iterator = Nothing
            it = rnd_it()
            assert_equal(m(+it), value(+it))
        Else
            Dim v As UInt32 = 0
            v = m(rnd_key())
        End If
    End Sub

    Private Sub [erase]()
        If validate() Then
            If rnd_int(0, 2) = 0 Then
                confirm_not_empty()
                Dim it As [set](Of String).iterator = Nothing
                it = rnd_it()
                assert_equal(m(+it), value(+it))
                assert_true(m.erase(+it))
                assert(v.erase(+it))
            Else
                assert_false(m.erase(rnd_key()))
            End If
        Else
            m.erase(rnd_key())
        End If
    End Sub

    Private Sub find()
        If validate() Then
            If rnd_int(0, 2) = 0 Then
                confirm_not_empty()
                Dim it As [set](Of String).iterator = Nothing
                it = rnd_it()
                assert_equal(m(+it), value(+it))
                assert_not_equal(m.find(+it), m.end())
            Else
                assert_equal(m.find(rnd_key()), m.end())
            End If
        Else
            m.find(rnd_key())
        End If
    End Sub

    Private Sub clear()
        If validate() Then
            Dim it As unordered_map(Of String, UInt32).iterator = Nothing
            it = m.begin()
            Dim c As Int64 = 0
            While it <> m.end()
#If first_with_brackets Then
                assert_not_equal(v.find((+it).first()), v.end())
                assert_equal((+it).second, value((+it).first()))
#Else
                assert_not_equal(v.find((+it).first), v.end())
                assert_equal((+it).second, value((+it).first))
#End If
                c += 1
                it += 1
            End While
            assert_equal(c, v.size())
            assert_equal(c, m.size())
        End If
        m.clear()
        assert_equal(CUInt(m.size()), uint32_0)
        assert_true(m.empty())
        If validate() Then
            v.clear()
        End If
    End Sub

    Private Sub clone()
        Dim m2 As unordered_map(Of String, UInt32) = Nothing
        copy(m2, m)
        assert_equal(object_compare(m, m2), object_compare_undetermined)
        If validate() Then
            assert_equal(m.size(), m2.size())
            Dim sz As Int64 = 0
            sz = m.size()
            Dim c As Int64 = 0
            Dim it As unordered_map(Of String, UInt32).iterator = Nothing

            Dim cmp As void(Of unordered_map(Of String, UInt32), unordered_map(Of String, UInt32)) =
                Sub(ByRef base, ByRef camp)
                    assert(Not base Is Nothing)
                    assert(Not camp Is Nothing)
                    c = 0
                    it = base.begin()
                    While it <> base.end()
#If first_with_brackets Then
                        assert_not_equal(camp.find((+it).first()), camp.end())
                        assert_equal((+it).second, value((+it).first()))
                        assert_equal((+it).second, camp((+it).first()))
#Else
                        assert_not_equal(camp.find((+it).first), camp.end())
                        assert_equal((+it).second, value((+it).first))
                        assert_equal((+it).second, camp((+it).first))
#End If
                        c += 1
                        it += 1
                    End While
                    assert_equal(c, base.size())
                    assert_equal(c, sz)
                End Sub
            cmp(m, m2)
            cmp(m2, m)

            assert_equal(m.size(), m2.size())
        End If
    End Sub
End Class

Public Class unordered_map_test
    Inherits repeat_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(New unordered_map_case(True), round())
    End Sub
End Class

Public Class unordered_map_perf
    Inherits performance_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(repeat(New unordered_map_case(False), round()))
    End Sub

    Protected Overrides Function max_loops() As UInt64
        Return CULng(If(isreleasebuild(), 250000000000, 30000000000))
    End Function
End Class
'finish map_case.vbp --------
'finish unordered_map_test.vbp --------
