﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utils
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class invoker_test2
    Private Class b
        Public Function a() As Boolean
            Return False
        End Function
    End Class

    Private Class c
        Inherits b
    End Class

    <test>
    Private Shared Sub with_object_without_type()
        Dim f As invoker(Of Func(Of Boolean)) = Nothing
        assert_true(invoker.of(f).
                        with_object(New b()).
                        with_binding_flags(binding_flags.instance_public_method).
                        with_name("a").
                        build(f))
        assert_true(invoker.of(f).
                        with_object(New c()).
                        with_binding_flags(binding_flags.instance_public_method).
                        with_name("a").
                        build(f))
    End Sub

    <test>
    Private Shared Sub with_object_with_type()
        Dim f As invoker(Of Func(Of Boolean)) = Nothing
        assert_true(invoker.of(f).
                        with_object(New b()).
                        with_type(Of b)().
                        with_binding_flags(binding_flags.instance_public_method Or BindingFlags.DeclaredOnly).
                        with_name("a").
                        build(f))
        assert_false(invoker.of(f).
                         with_object(New c()).
                         with_binding_flags(binding_flags.instance_public_method Or BindingFlags.DeclaredOnly).
                         with_name("a").
                         build(f))
    End Sub

    <test>
    Private Shared Sub with_object_with_not_resolved_delegate()
        Dim f As invoker(Of not_resolved_type_delegate) = Nothing
        Dim r As Object = Nothing
        assert_true(invoker.of(f).
                        with_type(Of b)().
                        with_binding_flags(binding_flags.instance_public_method).
                        with_name("a").
                        build(f))
        assert_true(f.post_binding())
        assert_false(f.pre_binding())
        assert_true(f.post_alloc_invoke(r, Nothing))
        assert_false(direct_cast(Of Boolean)(r))

        assert_true(invoker.of(f).
                        with_type(Of c)().
                        with_binding_flags(binding_flags.instance_public_method).
                        with_name("a").
                        build(f))
        assert_true(f.post_binding())
        assert_false(f.pre_binding())
        assert_true(f.post_alloc_invoke(r, Nothing))
        assert_false(direct_cast(Of Boolean)(r))
    End Sub

    Private Sub New()
    End Sub
End Class