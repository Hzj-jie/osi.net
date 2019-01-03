
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
        assertion.is_true(invoker.of(f).
                        with_object(New b()).
                        with_binding_flags(binding_flags.instance_public_method).
                        with_name("a").
                        build(f))
        assertion.is_true(invoker.of(f).
                        with_object(New c()).
                        with_binding_flags(binding_flags.instance_public_method).
                        with_name("a").
                        build(f))
    End Sub

    <test>
    Private Shared Sub with_object_with_type()
        Dim f As invoker(Of Func(Of Boolean)) = Nothing
        assertion.is_true(invoker.of(f).
                        with_object(New b()).
                        with_type(Of b)().
                        with_binding_flags(binding_flags.instance_public_method Or BindingFlags.DeclaredOnly).
                        with_name("a").
                        build(f))
        assertion.is_false(invoker.of(f).
                         with_object(New c()).
                         with_binding_flags(binding_flags.instance_public_method Or BindingFlags.DeclaredOnly).
                         with_name("a").
                         build(f))
    End Sub

    <test>
    Private Shared Sub without_object_with_not_resolved_delegate()
        Dim f As invoker = Nothing
        Dim r As Object = Nothing
        assertion.is_true(invoker.of(f).
                        with_type(Of b)().
                        with_binding_flags(binding_flags.instance_public_method).
                        with_name("a").
                        build(f))
        assertion.is_true(f.post_binding())
        assertion.is_false(f.pre_binding())
        assertion.is_true(f.post_alloc_invoke(r))
        assertion.is_false(direct_cast(Of Boolean)(r))

        assertion.is_true(invoker.of(f).
                        with_type(Of c)().
                        with_binding_flags(binding_flags.instance_public_method).
                        with_name("a").
                        build(f))
        assertion.is_true(f.post_binding())
        assertion.is_false(f.pre_binding())
        assertion.is_true(f.post_alloc_invoke(r))
        assertion.is_false(direct_cast(Of Boolean)(r))
    End Sub

    <test>
    Private Shared Sub with_object_with_not_resolved_delegate()
        Dim f As invoker = Nothing
        assertion.is_true(invoker.of(f).
                        with_binding_flags(binding_flags.instance_public_method).
                        with_name("a").
                        with_object(New b()).
                        build(f))
        If assertion.is_true(f.pre_binding()) Then
            assertion.is_false(direct_cast(Of Boolean)(f.pre_bind()({})))
        End If
        assertion.is_false(f.post_binding())
    End Sub

    <test>
    Private Shared Sub with_object_with_unmatch_signature()
        Dim f As invoker(Of Func(Of Int32)) = Nothing
        Dim r As Object = Nothing
        assertion.is_true(invoker.of(f).
                        with_type(Of b)().
                        with_binding_flags(binding_flags.instance_public_method).
                        with_name("a").
                        build(f))
        assertion.is_false(f.post_binding())
        assertion.is_false(f.pre_binding())
        assertion.is_true(f.invoke_only())
        assertion.is_true(f.post_alloc_invoke(r))
        assertion.is_false(direct_cast(Of Boolean)(r))
    End Sub

    Private Sub New()
    End Sub
End Class
