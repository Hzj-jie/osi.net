
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt

Public Class invoker_test
    Inherits [case]

    Private Shared Sub ss1(ByVal c As atomic_int)
        assert(c IsNot Nothing)
        c.increment()
    End Sub

    Private Shared Function sf1(ByVal c As atomic_int) As Int32
        assert(c IsNot Nothing)
        Return c.increment()
    End Function

    Private Sub s1(ByVal c As atomic_int)
        ss1(c)
    End Sub

    Private Function f1(ByVal c As atomic_int) As Int32
        Return sf1(c)
    End Function

    Private Function pre_bind() As Boolean
        Dim c As atomic_int = Nothing
        c = New atomic_int()
        Dim ds As invoker(Of Action(Of atomic_int)) = Nothing
        assertion.is_true(invoker.of(ds).
                        with_type(Of invoker_test).
                        with_binding_flags(binding_flags.static_private_method).
                        with_name("ss1").
                        build(ds))
        assertion.is_true(ds.static())
        assertion.is_true(ds.pre_binding())
        assertion.is_false(ds.post_binding())
        If assertion.is_true(ds.pre_bind(Nothing)) Then
            ds.pre_bind()(c)
        End If
        assertion.is_false(ds.post_bind(Me, Nothing))

        assertion.is_true(invoker.of(ds).
                        with_object(Me).
                        with_binding_flags(binding_flags.instance_private_method).
                        with_name("s1").
                        build(ds))
        assertion.is_false(ds.static())
        assertion.is_true(ds.pre_binding())
        assertion.is_false(ds.post_binding())
        If assertion.is_true(ds.pre_bind(Nothing)) Then
            ds.pre_bind()(c)
        End If
        assertion.is_false(ds.post_bind(Me, Nothing))

        Dim df As invoker(Of Func(Of atomic_int, Int32)) = Nothing
        assertion.is_true(invoker.of(df).
                        with_type(Of invoker_test).
                        with_binding_flags(binding_flags.static_private_method).
                        with_name("sf1").
                        build(df))
        assertion.is_true(df.static())
        assertion.is_true(df.pre_binding())
        assertion.is_false(df.post_binding())
        If assertion.is_true(df.pre_bind(Nothing)) Then
            assertion.equal(df.pre_bind()(c), +c)
        End If
        assertion.is_false(df.post_bind(Me, Nothing))

        assertion.is_true(invoker.of(df).
                        with_object(Me).
                        with_binding_flags(binding_flags.instance_private_method).
                        with_name("f1").
                        build(df))
        assertion.is_false(df.static())
        assertion.is_true(df.pre_binding())
        assertion.is_false(df.post_binding())
        If assertion.is_true(df.pre_bind(Nothing)) Then
            assertion.equal(df.pre_bind()(c), +c)
        End If
        assertion.is_false(df.post_bind(Me, Nothing))

        assertion.equal(+c, 4)

        assertion.is_false(invoker.of(df).
                         with_object(Me).
                         with_binding_flags(binding_flags.instance_private_method).
                         with_name("not_exist").
                         build(df))
        assertion.is_null(df)

        Return True
    End Function

    Private Function post_bind() As Boolean
        Dim c As atomic_int = Nothing
        c = New atomic_int()
        Dim ds As invoker(Of Action(Of atomic_int)) = Nothing
        assertion.is_true(invoker.of(ds).
                        with_type(Me.GetType()).
                        with_binding_flags(binding_flags.instance_private_method).
                        with_name("s1").
                        build(ds))
        assertion.is_false(ds.static())
        assertion.is_false(ds.pre_binding())
        assertion.is_true(ds.post_binding())
        assertion.is_false(ds.pre_bind(Nothing))
        If ds.post_bind(Me, Nothing) Then
            ds(Me)(c)
        End If

        Dim df As invoker(Of Func(Of atomic_int, Int32)) = Nothing
        assertion.is_true(invoker.of(df).
                        with_type(Me.GetType()).
                        with_binding_flags(binding_flags.instance_private_method).
                        with_name("f1").
                        build(df))
        assertion.is_false(df.static())
        assertion.is_false(df.pre_binding())
        assertion.is_true(df.post_binding())
        assertion.is_false(df.pre_bind(Nothing))
        If df.post_bind(Me, Nothing) Then
            assertion.equal(df(Me)(c), +c)
        End If

        assertion.equal(+c, 2)

        assertion.is_false(invoker.of(df).
                         with_type(Me.GetType()).
                         with_binding_flags(binding_flags.instance_private_method).
                         with_name("not_exist").
                         build(df))
        assertion.is_null(df)

        Return True
    End Function

    'suppress.invoker_error
    Public Overrides Function reserved_processors() As Int16
        Return CShort(Environment.ProcessorCount())
    End Function

    Public Overrides Function run() As Boolean
        Using scoped.atomic_bool(suppress.invoker_error)
            Return pre_bind() AndAlso
                   post_bind()
        End Using
    End Function
End Class
