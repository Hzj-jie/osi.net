
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt

Public Class valuer_create_recursively_test
    Inherits [case]

    Private Class base_class
        Public Shared x As String
        Public s As String
    End Class

    Private Class inherit_class
        Inherits base_class
        Public Shared y As String
        Public t As String
    End Class

    Public Overrides Function run() As Boolean
        Dim v As valuer(Of String) = Nothing
        assert_true(valuer.create_recursively(Of inherit_class, String)("x", v))
        assert_true(v.valid())
        v.set("abc")
        assert_equal(v.get(), "abc")
        assert_equal(base_class.x, "abc")

        assert_true(valuer.create_recursively(Of inherit_class, String)("y", v))
        assert_true(v.valid())
        v.set("def")
        assert_equal(v.get(), "def")
        assert_equal(inherit_class.y, "def")

        Dim i As inherit_class = Nothing
        i = New inherit_class()
        assert_true(valuer.create_recursively(i, "s", v))
        assert_true(v.valid())
        v.set("abc")
        assert_equal(v.get(), "abc")
        assert_equal(i.s, "abc")

        assert_true(valuer.create_recursively(i, "t", v))
        assert_true(v.valid())
        v.set("def")
        assert_equal(v.get(), "def")
        assert_equal(i.t, "def")
        Return True
    End Function
End Class
