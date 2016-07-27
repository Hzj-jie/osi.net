
Imports osi.root.utt
Imports osi.service.http

Public Class client_rr_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assert_equal(generate_url("abc", 80, ""), "http://abc")
        assert_equal(generate_url("abc", 80, Nothing), "http://abc")
        assert_equal(generate_url("abc", 81, ""), "http://abc:81")
        assert_equal(generate_url("abc", 81, Nothing), "http://abc:81")
        assert_equal(generate_url("abc", 80, "abc"), "http://abc/abc")
        assert_equal(generate_url("abc", 81, "abc"), "http://abc:81/abc")
        Return True
    End Function
End Class
