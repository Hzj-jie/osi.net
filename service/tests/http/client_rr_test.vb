
Imports osi.root.utt
Imports osi.service.http

Public Class client_rr_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assertion.equal(generate_url("abc", 80, ""), "http://abc")
        assertion.equal(generate_url("abc", 80, Nothing), "http://abc")
        assertion.equal(generate_url("abc", 81, ""), "http://abc:81")
        assertion.equal(generate_url("abc", 81, Nothing), "http://abc:81")
        assertion.equal(generate_url("abc", 80, "abc"), "http://abc/abc")
        assertion.equal(generate_url("abc", 81, "abc"), "http://abc:81/abc")
        Return True
    End Function
End Class
