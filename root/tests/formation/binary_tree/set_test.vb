
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with set_test.vbp ----------
'so change set_test.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with set_case.vbp ----------
'so change set_case.vbp instead of this file


#Const pair_return_insert = True

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.delegates

Friend Class set_case
    Inherits random_run_case

    Private ReadOnly s As [set](Of String) = Nothing
    Private ReadOnly keys As vector(Of String) = Nothing

    Private Shared Function rnd_key() As String
        Return guid_str()
    End Function

    Private Function validate() As Boolean
        Return Not keys Is Nothing
    End Function

    Public Sub New(ByVal validate As Boolean)
        s = New [set](Of String)()
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
                assertion.equal(keys.size(), s.size())
                Dim i As UInt32 = 0
                i = rnd_uint(uint32_0, keys.size())
                assertion.is_true(s.erase(keys(i)), "s.erase(", keys(i), ") returns false.")
                assertion.equal(s.find(keys(i)), s.end(), "s.find(", keys(i), ") <> s.end()")
                keys.erase(i)
                assertion.equal(keys.size(), s.size())
            Else
                assertion.is_false(s.erase(rnd_key()))
            End If
        Else
            s.erase(rnd_key())
        End If
    End Sub

    Private Sub find()
        If validate() Then
            If rnd_int(0, 2) = 0 Then
                confirm_not_empty()
                Dim i As UInt32
                i = rnd_uint(uint32_0, keys.size())
                assertion.not_equal(s.find(keys(i)), s.end(), "s.find(", keys(i), ") = s.end()")
            Else
                assertion.equal(s.find(rnd_key()), s.end())
            End If
        Else
            s.find(rnd_key())
        End If
    End Sub

    Private Sub insert()
        Dim k As String = Nothing
        k = rnd_key()
#If pair_return_insert Then
        Dim r As pair(Of [set](Of String).iterator, Boolean) = Nothing
        r = s.insert(k)
        If assertion.is_not_null(r) Then
            assertion.equal(r.second, s.find(k) <> s.end(), "s.insert(", k, ") <> s.find")
        End If
#Else
        assertion.equal(s.insert(k), s.find(k), "s.insert(", k, ") <> s.find")
#End If
        If validate() Then
            keys.push_back(k)
        End If
    End Sub

    Private Sub clone()
        Dim s2 As [set](Of String) = Nothing
        copy(s2, s)
        assertion.equal(object_compare(s, s2), object_compare_undetermined, "cannot clone to a new instance.")
        If validate() Then
            assertion.equal(s.size(), s2.size())
            Dim sz As Int64 = 0
            sz = s.size()
            Dim c As Int64 = 0
            Dim it As [set](Of String).iterator = Nothing

            Dim cmp As void(Of [set](Of String), [set](Of String)) =
                Sub(ByRef base, ByRef camp)
                    assert(Not base Is Nothing)
                    assert(Not camp Is Nothing)
                    c = 0
                    it = base.begin()
                    While it <> base.end()
                        assertion.not_equal(camp.find(+it), camp.end())
                        c += 1
                        it += 1
                    End While
                    assertion.equal(c, base.size())
                    assertion.equal(c, sz)
                End Sub
            cmp(s, s2)
            cmp(s2, s)

            assertion.equal(s.size(), s2.size())
        End If
    End Sub

    Private Sub clear()
        If validate() Then
            Dim it As [set](Of String).iterator = Nothing
            Dim c As Int64 = 0
            it = s.begin()
            While it <> s.end()
                assertion.not_equal(keys.find(+it), npos)
                c += 1
                it += 1
            End While
            assertion.equal(c, keys.size())
            assertion.equal(c, s.size())
        End If
        s.clear()
        assertion.equal(CUInt(s.size()), uint32_0, "s.size()<>0 after clear.")
        assertion.is_true(s.empty())
        If validate() Then
            keys.clear()
        End If
    End Sub
End Class

Public Class set_perf
    Inherits performance_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(repeat(New set_case(False), round()))
    End Sub

    Protected Overrides Function max_loops() As UInt64
        Return CULng(If(isdebugbuild(), 180000000000, 25000000000))
    End Function
End Class

Public Class set_test
    Inherits repeat_case_wrapper

    Private Shared Function round() As Int64
        Return (1000000 << (If(isreleasebuild(), 2, 0)))
    End Function

    Public Sub New()
        MyBase.New(New set_case(True), round())
    End Sub
End Class
'finish set_case.vbp --------
'finish set_test.vbp --------
