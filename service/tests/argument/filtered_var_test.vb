
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument

<test>
Public NotInheritable Class filtered_var_test
    <test>
    Private Shared Sub get_filtered_values()
        Dim v As var = Nothing
        v = New var("--a=b -cde ~def --filtered.a=d ~filtered.efg hij klm")
        assertion.is_true(v.switch("c"))
        assertion.is_true(v.switch("d"))
        assertion.is_true(v.switch("e"))
        assertion.is_true(v.switch("def"))
        assertion.is_true(v.switch("filtered.efg"))
        assertion.equal(v("a"), "b")
        assertion.equal(v("filtered.a"), "d")
        assertion.array_equal(+(v.other_values()), {"hij", "klm"})

        Dim f As var = Nothing
        f = filtered_var.[New](v, "filtered.")
        assertion.is_false(f.switch("c"))
        assertion.is_false(f.switch("d"))
        assertion.is_false(f.switch("e"))
        assertion.is_false(f.switch("def"))
        assertion.is_false(f.switch("filtered.efg"))
        assertion.is_false(f.value("filtered.a", ""))
        assertion.is_true(f.switch("efg"))
        assertion.equal(f("a"), "d")
        assertion.array_equal(+(f.other_values()), {"hij", "klm"})
    End Sub

    <test>
    Private Shared Sub cloneable()
        Dim v As var = Nothing
        v = New var("--name.f=second ~name.t -case some other values")

        Dim f As var = Nothing
        f = filtered_var.[New](v, "name.")

        Dim f2 As var = Nothing
        Dim f3 As var = Nothing
        assertion.is_true(copy(f2, f))
        assertion.equal(f2("f"), "second")
        assertion.is_true(f2.switch("t"))
        assertion.is_false(f2.switch("c"))
        assertion.is_false(f2.switch("a"))
        assertion.is_false(f2.switch("s"))
        assertion.is_false(f2.switch("e"))
        assertion.array_equal(+(f2.other_values()), {"some", "other", "values"})
    End Sub

    <test>
    Private Shared Sub comparable()
        Dim v As var = Nothing
        v = New var("--name.a=1 --name.b=2 ~name.c ~name.d some other values")

        Dim f As var = Nothing
        f = filtered_var.[new](v, "name.")
        Dim v2 As var = Nothing
        v2 = f.CloneT()

        Dim expected As var = Nothing
        expected = New var("--a=1 --b=2 ~c ~d some other values")

        assertion.equal(f, expected)
        assertion.equal(f, v2)
        assertion.equal(expected, v2)

        assertion.equal(v, v)
        assertion.equal(f, f)
        assertion.equal(v2, v2)
    End Sub

    Private Sub New()
    End Sub
End Class
