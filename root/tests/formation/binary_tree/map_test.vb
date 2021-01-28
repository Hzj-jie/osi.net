
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map_test.vbp ----------
'so change map_test.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with map_case.vbp ----------
'so change map_case.vbp instead of this file



Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.delegates

Friend Class map_case
    Inherits random_run_case

    Private ReadOnly m As map(Of String, UInt32) = Nothing
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
        m = New map(Of String, UInt32)()
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
        assertion.equal(m.emplace(k, value(k)).first, m.find(k))
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
            assertion.equal(m(+it), value(+it))
            Dim nv As UInt32 = 0
            nv = value(rnd_key())
            m(+it) = nv
            assertion.equal(m(+it), nv)
            m(+it) = value(+it)
            assertion.equal(m(+it), value(+it))
        Else
            m(rnd_key()) = value(rnd_key())
        End If
    End Sub

    Private Sub [get]()
        If validate() Then
            confirm_not_empty()
            Dim it As [set](Of String).iterator = Nothing
            it = rnd_it()
            assertion.equal(m(+it), value(+it))
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
                assertion.equal(m(+it), value(+it))
                assertion.is_true(m.erase(+it))
                assert(v.erase(+it))
            Else
                assertion.is_false(m.erase(rnd_key()))
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
                assertion.equal(m(+it), value(+it))
                assertion.not_equal(m.find(+it), m.end())
            Else
                assertion.equal(m.find(rnd_key()), m.end())
            End If
        Else
            m.find(rnd_key())
        End If
    End Sub

    Private Sub clear()
        If validate() Then
            Dim it As map(Of String, UInt32).iterator = m.begin()
            Dim c As Int64 = 0
            While it <> m.end()
                assertion.not_equal(v.find((+it).first), v.end())
                assertion.equal((+it).second, value((+it).first))
                c += 1
                it += 1
            End While
            assertion.equal(c, v.size())
            assertion.equal(c, m.size())
        End If
        If rnd_bool() Then
            m.clear()
        Else
            Dim it As map(Of String, UInt32).iterator = m.begin()
            Dim c As Int64 = 0
            While it <> m.end()
                c += 1
                it = m.erase(it)
            End While
            assertion.equal(c, v.size())
        End If
        assertion.equal(CUInt(m.size()), uint32_0)
        assertion.is_true(m.empty())
        If validate() Then
            v.clear()
        End If
    End Sub

    Private Sub clone()
        Dim m2 As map(Of String, UInt32) = Nothing
        copy(m2, m)
        assertion.equal(object_compare(m, m2), object_compare_undetermined)
        If Not validate() Then
            Return
        End If
        assertion.equal(m.size(), m2.size())
        Dim sz As Int64 = 0
        sz = m.size()
        Dim c As Int64 = 0
        Dim it As map(Of String, UInt32).iterator = Nothing

        Dim cmp As void(Of map(Of String, UInt32), map(Of String, UInt32)) =
            Sub(ByRef base, ByRef camp)
                assert(Not base Is Nothing)
                assert(Not camp Is Nothing)
                c = 0
                it = base.begin()
                While it <> base.end()
                    assertion.not_equal(camp.find((+it).first), camp.end())
                    assertion.equal((+it).second, value((+it).first))
                    assertion.equal((+it).second, camp((+it).first))
                    c += 1
                    it += 1
                End While
                assertion.equal(c, base.size())
                assertion.equal(c, sz)
            End Sub
        cmp(m, m2)
        cmp(m2, m)

        assertion.equal(m.size(), m2.size())
        assertion.equal(m, m2)
    End Sub
End Class

Public NotInheritable Class map_test
    Inherits repeat_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(New map_case(True), round())
    End Sub
End Class

Public NotInheritable Class map_perf
    Inherits performance_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(repeat(New map_case(False), round()))
    End Sub

    Protected Overrides Function max_loops() As UInt64
        Return CULng(If(isreleasebuild(), 250000000000, 30000000000))
    End Function
End Class
'finish map_case.vbp --------
'finish map_test.vbp --------
