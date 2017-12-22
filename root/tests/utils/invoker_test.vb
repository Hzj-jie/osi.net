
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utils
Imports osi.root.utt

Public Class invoker_test
    Inherits [case]

    Private Shared Sub ss1(ByVal c As atomic_int)
        assert(Not c Is Nothing)
        c.increment()
    End Sub

    Private Shared Function sf1(ByVal c As atomic_int) As Int32
        assert(Not c Is Nothing)
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
        ds = New invoker(Of Action(Of atomic_int))(GetType(invoker_test),
                                                   Reflection.BindingFlags.NonPublic Or
                                                   Reflection.BindingFlags.Static,
                                                   "ss1")
        assert_true(ds.valid())
        assert_true(ds.static())
        assert_true(ds.pre_binding())
        assert_false(ds.post_binding())
        If assert_true(ds.pre_bind(Nothing)) Then
            ds.get()(c)
        End If
        assert_false(ds.post_bind(Me, Nothing))

        ds = New invoker(Of Action(Of atomic_int))(Me,
                                                   Reflection.BindingFlags.NonPublic Or
                                                   Reflection.BindingFlags.Instance,
                                                   "s1")
        assert_true(ds.valid())
        assert_false(ds.static())
        assert_true(ds.pre_binding())
        assert_false(ds.post_binding())
        If assert_true(ds.pre_bind(Nothing)) Then
            ds.get()(c)
        End If
        assert_false(ds.post_bind(Me, Nothing))

        Dim df As invoker(Of Func(Of atomic_int, Int32)) = Nothing
        df = New invoker(Of Func(Of atomic_int, Int32))(GetType(invoker_test),
                                                        Reflection.BindingFlags.NonPublic Or
                                                        Reflection.BindingFlags.Static,
                                                        "sf1")
        assert_true(df.valid())
        assert_true(df.static())
        assert_true(df.pre_binding())
        assert_false(df.post_binding())
        If assert_true(df.pre_bind(Nothing)) Then
            assert_equal(df.get()(c), +c)
        End If
        assert_false(df.post_bind(Me, Nothing))

        df = New invoker(Of Func(Of atomic_int, Int32))(Me,
                                                        Reflection.BindingFlags.NonPublic Or
                                                        Reflection.BindingFlags.Instance,
                                                        "f1")
        assert_true(df.valid())
        assert_false(df.static())
        assert_true(df.pre_binding())
        assert_false(df.post_binding())
        If assert_true(df.pre_bind(Nothing)) Then
            assert_equal(df.get()(c), +c)
        End If
        assert_false(df.post_bind(Me, Nothing))

        assert_equal(+c, 4)

        df = New invoker(Of Func(Of atomic_int, Int32))(Me,
                                                        Reflection.BindingFlags.NonPublic Or
                                                        Reflection.BindingFlags.Instance,
                                                        "not_exist")
        assert_false(df.valid())
        assert_false(df.pre_binding())
        assert_false(df.post_binding())
        assert_false(df.pre_bind(Nothing))
        assert_false(df.post_bind(Me, Nothing))

        Return True
    End Function

    Private Function post_bind() As Boolean
        Dim c As atomic_int = Nothing
        c = New atomic_int()
        Dim ds As invoker(Of Action(Of atomic_int)) = Nothing
        ds = New invoker(Of Action(Of atomic_int))(Me.GetType(),
                                                   Reflection.BindingFlags.NonPublic Or
                                                   Reflection.BindingFlags.Instance,
                                                   "s1")
        assert_true(ds.valid())
        assert_false(ds.static())
        assert_false(ds.pre_binding())
        assert_true(ds.post_binding())
        assert_false(ds.pre_bind(Nothing))
        If ds.post_bind(Me, Nothing) Then
            ds(Me)(c)
        End If

        Dim df As invoker(Of Func(Of atomic_int, Int32)) = Nothing
        df = New invoker(Of Func(Of atomic_int, Int32))(Me.GetType(),
                                                        Reflection.BindingFlags.NonPublic Or
                                                        Reflection.BindingFlags.Instance,
                                                        "f1")
        assert_true(df.valid())
        assert_false(df.static())
        assert_false(df.pre_binding())
        assert_true(df.post_binding())
        assert_false(df.pre_bind(Nothing))
        If df.post_bind(Me, Nothing) Then
            assert_equal(df(Me)(c), +c)
        End If

        assert_equal(+c, 2)

        df = New invoker(Of Func(Of atomic_int, Int32))(Me.GetType(),
                                                        Reflection.BindingFlags.NonPublic Or
                                                        Reflection.BindingFlags.Instance,
                                                        "not_exist")
        assert_false(df.valid())
        assert_false(df.pre_binding())
        assert_false(df.post_binding())
        assert_false(df.pre_bind(Nothing))
        assert_false(df.post_bind(Me, Nothing))

        Return True
    End Function

    'suppress.invoker_error
    Public Overrides Function reserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function

    Public Overrides Function run() As Boolean
        Using scoped_atomic_bool(suppress.invoker_error)
            Return pre_bind() AndAlso
                   post_bind()
        End Using
    End Function
End Class
