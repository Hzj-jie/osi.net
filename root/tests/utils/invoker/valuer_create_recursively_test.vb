
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
        assertion.is_true(valuer.create_recursively(Of inherit_class, String)("x", v))
        assertion.is_true(v.valid())
        v.set("abc")
        assertion.equal(v.get(), "abc")
        assertion.equal(base_class.x, "abc")

        assertion.is_true(valuer.create_recursively(Of inherit_class, String)("y", v))
        assertion.is_true(v.valid())
        v.set("def")
        assertion.equal(v.get(), "def")
        assertion.equal(inherit_class.y, "def")

        Dim i As inherit_class = Nothing
        i = New inherit_class()
        assertion.is_true(valuer.create_recursively(i, "s", v))
        assertion.is_true(v.valid())
        v.set("abc")
        assertion.equal(v.get(), "abc")
        assertion.equal(i.s, "abc")

        assertion.is_true(valuer.create_recursively(i, "t", v))
        assertion.is_true(v.valid())
        v.set("def")
        assertion.equal(v.get(), "def")
        assertion.equal(i.t, "def")
        Return True
    End Function
End Class
