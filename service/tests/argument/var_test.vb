
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument

<test>
Public Class var_test
    <test>
    Private Shared Sub cloneable()
        Dim v As var = Nothing
        v = New var("--f=second ~t -case some other values")
        assert_equal(v("f"), "second")
        assert_true(v.switch("t"))
        v.bind("first", "third")
        assert_false(v.value("f", String.Empty))  ' raw values have been removed
        assert_equal(v("first"), "second")
        assert_false(v.switch("t"))                 ' raw values have been removed.
        assert_true(v.switch("third"))
        assert_true(v.switch("c"))
        assert_true(v.switch("a"))
        assert_true(v.switch("s"))
        assert_true(v.switch("e"))
        assert_array_equal(+(v.other_values()), {"some", "other", "values"})

        Dim v2 As var = Nothing
        assert_true(copy(v2, v))
        assert_false(v2.value("f", String.Empty))   ' raw values have been removed.
        assert_equal(v2("first"), "second")
        assert_false(v.switch("t"))                 ' raw values have been removed.
        assert_true(v2.switch("third"))
        assert_true(v2.switch("c"))
        assert_true(v2.switch("a"))
        assert_true(v2.switch("s"))
        assert_true(v2.switch("e"))
        assert_array_equal(+(v2.other_values()), {"some", "other", "values"})
    End Sub

    Private Sub New()
    End Sub
End Class
