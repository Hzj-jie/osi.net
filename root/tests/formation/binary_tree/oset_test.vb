
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with oset_test.vbp ----------
'so change oset_test.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with set_case.vbp ----------
'so change set_case.vbp instead of this file


#Const pair_return_insert = True

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utils
Imports osi.root.delegates

Friend Class oset_case
    Inherits random_run_case

    Private ReadOnly s As [oset](Of String) = Nothing
    Private ReadOnly keys As vector(Of String) = Nothing

    Private Shared Function rnd_key() As String
        Return guid_str()
    End Function

    Private Function validate() As Boolean
        Return Not keys Is Nothing
    End Function

    Public Sub New(ByVal validate As Boolean)
        s = New [oset](Of String)()
        If validate Then
            keys = New vector(Of String)()
        End If
        insert_call(0.43, AddressOf insert)
        insert_call(0.25, AddressOf find)
        insert_call(0.26, AddressOf [erase])
        insert_call(0.01, AddressOf clone)
        insert_call(0.05, AddressOf clear)
    End Sub

    Public Overrides Function finish() As Boolean
        s.clear()
        If validate() Then
            keys.clear()
        End If
        Return MyBase.finish()
    End Function

    Private Sub confirm_not_empty()
        assert(validate())
        If keys.empty() Then
            insert()
        End If
        assert(Not keys.empty())
    End Sub

    Private Sub [erase]()
        If validate() Then
            If rnd_int(0, 2) = 0 Then
                confirm_not_empty()
                assert_equal(keys.size(), s.size())
                Dim i As Int32 = 0
                i = rnd_int(0, keys.size())
                assert_true(s.erase(keys(i)), "s.erase(", keys(i), ") returns false.")
                assert_equal(s.find(keys(i)), s.end(), "s.find(", keys(i), ") <> s.end()")
                keys.erase(i)
                assert_equal(keys.size(), s.size())
            Else
                assert_false(s.erase(rnd_key()))
            End If
        Else
            s.erase(rnd_key())
        End If
    End Sub

    Private Sub find()
        If validate() Then
            If rnd_int(0, 2) = 0 Then
                confirm_not_empty()
                Dim i As Int64
                i = rnd_int(0, keys.size())
                assert_not_equal(s.find(keys(i)), s.end(), "s.find(", keys(i), ") = s.end()")
            Else
                assert_equal(s.find(rnd_key()), s.end())
            End If
        Else
            s.find(rnd_key())
        End If
    End Sub

    Private Sub insert()
        Dim k As String = Nothing
        k = rnd_key()
#If pair_return_insert Then
		Dim r As pair(Of [oset](Of String).iterator, Boolean) = Nothing
		r = s.insert(k)
		If assert_not_nothing(r) Then
			assert_equal(r.second, s.find(k) <> s.end(), "s.insert(", k, ") <> s.find")
		End If
#Else
        assert_equal(s.insert(k), s.find(k), "s.insert(", k, ") <> s.find")
#End If
        If validate() Then
            keys.push_back(k)
        End If
    End Sub

    Private Sub clone()
        Dim s2 As [oset](Of String) = Nothing
        copy(s2, s)
        assert_equal(object_compare(s, s2), object_compare_undetermined, "cannot clone to a new instance.")
        If validate() Then
            assert_equal(s.size(), s2.size())
            Dim sz As Int64 = 0
            sz = s.size()
            Dim c As Int64 = 0
            Dim it As [oset](Of String).iterator = Nothing

            Dim cmp As void(Of [oset](Of String), [oset](Of String)) =
                Sub(ByRef base, ByRef camp)
                    assert(Not base Is Nothing)
                    assert(Not camp Is Nothing)
                    c = 0
                    it = base.begin()
                    While it <> base.end()
                        assert_not_equal(camp.find(+it), camp.end())
                        c += 1
                        it += 1
                    End While
                    assert_equal(c, base.size())
                    assert_equal(c, sz)
                End Sub
            cmp(s, s2)
            cmp(s2, s)

            assert_equal(s.size(), s2.size())
        End If
    End Sub

    Private Sub clear()
        If validate() Then
            Dim it As [oset](Of String).iterator = Nothing
            Dim c As Int64 = 0
            it = s.begin()
            While it <> s.end()
                assert_not_equal(keys.find(+it), npos)
                c += 1
                it += 1
            End While
            assert_equal(c, keys.size())
            assert_equal(c, s.size())
        End If
        s.clear()
        assert_equal(CUInt(s.size()), uint32_0, "s.size()<>0 after clear.")
        assert_true(s.empty())
        If validate() Then
            keys.clear()
        End If
    End Sub
End Class

Public Class oset_perf
    Inherits performance_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(repeat(New oset_case(False), round()))
    End Sub

    Protected Overrides Function max_loops() As UInt64
        Return If(isreleasebuild(), 180000000000, 25000000000)
    End Function
End Class

Public Class oset_test
    Inherits repeat_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(New oset_case(True), round())
    End Sub
End Class
'finish set_case.vbp --------
'finish oset_test.vbp --------
