
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class thread_static_resolver_test
    <test>
    <multi_threading(10)>
    <repeat(100)>
    Private Shared Sub run()
        Dim i As Object = Nothing
        i = New Object()
        If rnd_bool() Then
            thread_static_resolver(Of Object).register(i)
        Else
            thread_static_resolver(Of Object).register(Function() As Object
                                                           Return i
                                                       End Function)
        End If
        Dim o As Object = Nothing
        assert_true(thread_static_resolver(Of Object).resolve(o))
        assert_reference_equal(i, o)
        thread_static_resolver(Of Object).unregister()
    End Sub

    <test>
    <repeat(100)>
    Private Shared Sub single_thread_case()
        Dim i As Object = Nothing
        i = New Object()
        If rnd_bool() Then
            thread_static_resolver(Of Object).register(i)
        Else
            thread_static_resolver(Of Object).register(Function() As Object
                                                           Return i
                                                       End Function)
        End If
        Dim o As Object = Nothing
        assert_true(thread_static_resolver.resolve(o))
        assert_reference_equal(i, o)
        thread_static_resolver(Of Object).unregister()
    End Sub

    <test>
    Private Shared Sub scoped_register_case()
        Dim i As Object = Nothing
        i = New Object()
        Using thread_static_resolver(Of Object).scoped_register(i)
            Dim o As Object = Nothing
            assert_true(thread_static_resolver.resolve(o))
            assert_reference_equal(i, o)
        End Using
        assert_false(thread_static_resolver(Of Object).resolve(Nothing))

        Using thread_static_resolver(Of Object).scoped_register(Function() As Object
                                                                    Return i
                                                                End Function)
            Dim o As Object = Nothing
            assert_true(thread_static_resolver.resolve(o))
            assert_reference_equal(i, o)
        End Using
        assert_false(thread_static_resolver(Of Object).resolve(Nothing))
    End Sub

    <test>
    Private Shared Sub resolve_rt_case()
        Dim i As Int32 = 0
        i = rnd_int()
        Dim o As Int32 = 0
        Using thread_static_resolver(Of Object).scoped_register(i)
            assert_true(thread_static_resolver(Of Object).resolve(o))
            assert_equal(i, o)
        End Using
        assert_false(thread_static_resolver(Of Object).resolve(o))

        Using thread_static_resolver(Of Object).scoped_register(New Int64())
            assert_false(thread_static_resolver(Of Object).resolve(o))
        End Using
    End Sub

    <test>
    Private Shared Sub resolve_shortcuts_case()
        Dim i As Object = Nothing
        i = New Object()
        assert_nothing(thread_static_resolver(Of Object).resolve_or_null())
        assert_reference_equal(thread_static_resolver(Of Object).resolve_or_default(i), i)
        Using thread_static_resolver(Of Object).scoped_register(i)
            assert_reference_equal(thread_static_resolver(Of Object).resolve_or_null(), i)
            assert_reference_equal(thread_static_resolver(Of Object).resolve_or_default(New Object()), i)
        End Using

        Using thread_static_resolver(Of Object).scoped_register(100)
            assert_equal(thread_static_resolver(Of Object).resolve_or_null(), 100)
            assert_equal(thread_static_resolver(Of Object).resolve_or_default(New Object()), 100)

            assert_equal(thread_static_resolver(Of Object).resolve_or_null(Of Int32)(), 100)
            assert_equal(thread_static_resolver(Of Object).resolve_or_default(Of Int32)(200), 100)
        End Using

        Using thread_static_resolver(Of Object).scoped_register(1.0)
            assert_equal(thread_static_resolver(Of Object).resolve_or_null(), 1.0)
            assert_equal(thread_static_resolver(Of Object).resolve_or_default(New Object()), 1.0)

            assert_equal(thread_static_resolver(Of Object).resolve_or_null(Of Int32)(), 0)
            assert_equal(thread_static_resolver(Of Object).resolve_or_default(Of Int32)(200), 200)
        End Using
    End Sub

    Private Sub New()
    End Sub
End Class
