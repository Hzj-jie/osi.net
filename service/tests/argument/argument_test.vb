
Imports osi.root.utt
Imports osi.service.argument

Public Class argument_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim v As var = Nothing
        v = New var()
        v.parse("--a=bcd -ijk ~op --b=def --de=efg ghghgh")
        assert_true(v.bind("abc", "bcd", "def", "opq"))
        assert_true(v.switch("i"))
        assert_true(v.switch("j"))
        assert_true(v.switch("k"))
        assert_true(v.switch("opq"))
        assert_equal(v.value("abc"), "bcd")
        assert_equal(v.value("bcd"), "def")
        assert_equal(v.value("def"), "efg")
        If assert_equal(v.other_values().size(), CUInt(1)) Then
            assert_equal(v.other_values()(0), "ghghgh")
        End If
        Return True
    End Function
End Class
