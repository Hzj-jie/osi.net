
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class resolver_test
    Private Shared ReadOnly multiple_thread_resolver As thread_safe_resolver(Of Object)
    Private Shared ReadOnly multiple_thread_resolver_object As Object

    Shared Sub New()
        Dim resolved As Boolean = False
        multiple_thread_resolver_object = New Object()
        multiple_thread_resolver = New thread_safe_resolver(Of Object)()
        multiple_thread_resolver.register(Function() As Object
                                              assert_false(resolved)
                                              resolved = True
                                              Return multiple_thread_resolver_object
                                          End Function)
    End Sub

    <test>
    Private Shared Sub single_thread_case()
        Dim r As resolver(Of Object) = Nothing
        r = New resolver(Of Object)()

        Dim i As Object = Nothing
        i = New Object()
        r.register(i)

        Dim o As Object = Nothing
        assert_true(r.resolve(o))
        assert_reference_equal(i, o)

        i = New Object()
        assert_not_reference_equal(i, o)
        r.register(i)

        assert_true(r.resolve(o))
        assert_reference_equal(i, o)

        i = New Object()
        assert_not_reference_equal(i, o)
        r.register(Function() As Object
                       Return i
                   End Function)
        assert_true(r.resolve(o))
        assert_reference_equal(i, o)
    End Sub

    <test>
    Private Shared Sub unregistered_case()
        Dim r As resolver(Of Object) = Nothing
        r = New resolver(Of Object)()

        assert_false(r.resolve(Nothing))
    End Sub

    <test>
    Private Shared Sub should_cache_result()
        Dim r As resolver(Of Object) = Nothing
        r = New resolver(Of Object)()

        Dim i As Object = Nothing
        i = New Object()
        Dim resolved As Boolean = False
        r.register(Function() As Object
                       assert_false(resolved)
                       resolved = True
                       Return i
                   End Function)
        For j As Int32 = 0 To 100
            Dim o As Object = Nothing
            assert_true(r.resolve(o))
            assert_reference_equal(i, o)
        Next
    End Sub

    <test>
    <repeat(100)>
    <multi_threading(10)>
    Private Shared Sub resolve_in_multiple_threads()
        Dim o As Object = Nothing
        assert_true(multiple_thread_resolver.resolve(o))
        assert_reference_equal(o, multiple_thread_resolver_object)
    End Sub

    Private Sub New()
    End Sub
End Class
