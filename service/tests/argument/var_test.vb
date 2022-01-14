
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
        assertion.equal(v("f"), "second")
        assertion.is_true(v.switch("t"))
        v.bind("first", "third")
        assertion.is_false(v.value("f", ""))  ' raw values have been removed
        assertion.equal(v("first"), "second")
        assertion.is_false(v.switch("t"))                 ' raw values have been removed.
        assertion.is_true(v.switch("third"))
        assertion.is_true(v.switch("c"))
        assertion.is_true(v.switch("a"))
        assertion.is_true(v.switch("s"))
        assertion.is_true(v.switch("e"))
        assertion.array_equal(+(v.other_values()), {"some", "other", "values"})

        Dim v2 As var = Nothing
        assertion.is_true(copy(v2, v))
        assertion.is_false(v2.value("f", ""))   ' raw values have been removed.
        assertion.equal(v2("first"), "second")
        assertion.is_false(v.switch("t"))                 ' raw values have been removed.
        assertion.is_true(v2.switch("third"))
        assertion.is_true(v2.switch("c"))
        assertion.is_true(v2.switch("a"))
        assertion.is_true(v2.switch("s"))
        assertion.is_true(v2.switch("e"))
        assertion.array_equal(+(v2.other_values()), {"some", "other", "values"})
    End Sub

    Private Sub New()
    End Sub
End Class
