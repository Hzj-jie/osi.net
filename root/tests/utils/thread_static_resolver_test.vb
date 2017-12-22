
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

        thread_static_resolver(Of Object).unregister()
    End Sub

    Private Sub New()
    End Sub
End Class
