
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
        assert_true(v.switch("c"))
        assert_true(v.switch("d"))
        assert_true(v.switch("e"))
        assert_true(v.switch("def"))
        assert_true(v.switch("filtered.efg"))
        assert_equal(v("a"), "b")
        assert_equal(v("filtered.a"), "d")
        assert_array_equal(+(v.other_values()), {"hij", "klm"})

        Dim f As var = Nothing
        f = filtered_var.[New](v, "filtered.")
        assert_false(f.switch("c"))
        assert_false(f.switch("d"))
        assert_false(f.switch("e"))
        assert_false(f.switch("def"))
        assert_false(f.switch("filtered.efg"))
        assert_false(f.value("filtered.a", String.Empty))
        assert_true(f.switch("efg"))
        assert_equal(f("a"), "d")
        assert_array_equal(+(f.other_values()), {"hij", "klm"})
    End Sub

    <test>
    Private Shared Sub cloneable()
        Dim v As var = Nothing
        v = New var("--name.f=second ~name.t -case some other values")

        Dim f As var = Nothing
        f = filtered_var.[New](v, "name.")

        Dim f2 As var = Nothing
        Dim f3 As var = Nothing
        assert_true(copy(f2, f))
        assert_equal(f2("f"), "second")
        assert_true(f2.switch("t"))
        assert_false(f2.switch("c"))
        assert_false(f2.switch("a"))
        assert_false(f2.switch("s"))
        assert_false(f2.switch("e"))
        assert_array_equal(+(f2.other_values()), {"some", "other", "values"})
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

        assert_equal(f, expected)
        assert_equal(f, v2)
        assert_equal(expected, v2)

        assert_equal(v, v)
        assert_equal(f, f)
        assert_equal(v2, v2)
    End Sub

    Private Sub New()
    End Sub
End Class
