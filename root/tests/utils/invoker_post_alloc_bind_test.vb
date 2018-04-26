﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class invoker_post_alloc_bind_test
    Private NotInheritable Class test_class
        Inherits cd_object(Of test_class)
        Public value As Int32 = 0

        Public Sub check()
            assert_equal(value, 0)
        End Sub
    End Class

    <test>
    Private Shared Sub allocate_new_object()
        Dim i As invoker(Of Action) = Nothing
        i = New invoker(Of Action)(GetType(test_class), binding_flags.instance_public_method, "check")
        assert(i.valid() AndAlso i.post_binding())
        For j As UInt32 = 0 To 10
            assert_equal(test_class.constructed(), j)
            i.post_allocate_bind()()
        Next
    End Sub

    Private Structure test_struct
        Public value As Int32

        Public Sub check()
            assert_equal(value, 0)
        End Sub
    End Structure

    ' <test> Cannot work, "obj is nothing" check cannot work with value-type casted nothing in object type.
    Private Shared Sub allocate_struct()
        Dim i As invoker(Of Action) = Nothing
        i = New invoker(Of Action)(GetType(test_struct), binding_flags.instance_public_method, "check")
        assert(i.valid() AndAlso i.post_binding())
        For j As UInt32 = 0 To 10
            i.post_allocate_bind()()
        Next
    End Sub

    Private NotInheritable Class test_class2
        Public Function increment(ByVal i As Int32) As Int32
            Return i + 1
        End Function
    End Class

    <test>
    Private Shared Sub forward_parameter_and_return()
        Dim i As invoker(Of Func(Of Int32, Int32)) = Nothing
        i = New invoker(Of Func(Of Int32, Int32))(GetType(test_class2),
                                                  binding_flags.instance_public_method,
                                                  "increment")
        assert(i.valid() AndAlso i.post_binding())
        For j As Int32 = 0 To 10
            assert_equal(i.post_allocate_bind()(j), j + 1)
        Next
    End Sub

    Private NotInheritable Class test_class3
        Public Sub New()
            Throw New Exception()
        End Sub

        Public Sub exec()
        End Sub
    End Class

    ' <test> not testable, post_allocate_bind()() asserts. Maybe use isolated_case_wrapper?
    Private Shared Sub forward_exceptions_from_constructor()
        Dim i As invoker(Of Action) = Nothing
        i = New invoker(Of Action)(GetType(test_class3), binding_flags.instance_public_method, "exec")
        assert(i.valid() AndAlso i.post_binding())
        assert_throw(Sub()
                         i.post_allocate_bind()
                     End Sub)
        assert_throw(Sub()
                         i.post_allocate_bind()()
                     End Sub)
    End Sub

    Private NotInheritable Class test_class4
        Public Sub exec()
            Throw New Exception()
        End Sub
    End Class

    <test>
    Private Shared Sub forward_exceptions_from_invoke()
        Dim i As invoker(Of Action) = Nothing
        i = New invoker(Of Action)(GetType(test_class4), binding_flags.instance_public_method, "exec")
        assert(i.valid() AndAlso i.post_binding())
        i.post_allocate_bind()
        assert_throw(Sub()
                         i.post_allocate_bind()()
                     End Sub)
    End Sub

    Private Interface test_int
        Sub exec()
    End Interface

    ' <test> not testable, post_allocate_bind()() asserts. Maybe use isolated_case_wrapper?
    Private Shared Sub not_allocatable()
        Dim i As invoker(Of Action) = Nothing
        i = New invoker(Of Action)(GetType(test_int), binding_flags.instance_public_method, "exec")
        assert(i.valid() AndAlso i.post_binding())
        assert_throw(Sub()
                         i.post_allocate_bind()
                     End Sub)
        assert_throw(Sub()
                         i.post_allocate_bind()()
                     End Sub)
    End Sub

    Private Sub New()
    End Sub
End Class
