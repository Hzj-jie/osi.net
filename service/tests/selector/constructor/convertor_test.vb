
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument
Imports osi.service.selector

<test>
Public NotInheritable Class convertor_test
    Private Class test_class
        Public ReadOnly s As Int32

        Public Sub New(ByVal s As Int32)
            Me.s = s
        End Sub
    End Class

    Private Class test_class_wrapper
        Inherits test_class

        Public ReadOnly t As test_class

        Public Sub New(ByVal t As test_class, ByVal s As Int32)
            MyBase.New(s)
            Me.t = t
        End Sub
    End Class

    Private Class test_class2
        Public ReadOnly t As test_class
        Public ReadOnly s As Int32

        Public Sub New(ByVal t As test_class, ByVal s As Int32)
            Me.t = t
            Me.s = s
        End Sub
    End Class

    Private Class test_class2_wrapper
        Inherits test_class2

        Public ReadOnly t2 As test_class2

        Public Sub New(ByVal t As test_class2, ByVal s As Int32)
            MyBase.New(assert_not_nothing_return(t).t, s)
            Me.t2 = t
        End Sub
    End Class

    <test>
    Private Shared Sub simple_case()
        Const type As String = "simple_case_type"
        assert_true(constructor.register(type,
                                         Function(ByVal v As var) As test_class
                                             Return New test_class(1)
                                         End Function))
        assert_true(convertor.register(type,
                                       Function(ByVal v As var, ByVal i As test_class) As test_class2
                                           Return New test_class2(i, 1)
                                       End Function,
                                       "test_class"))

        Dim r As test_class2 = Nothing
        assert_true(constructor.sync_resolve(New var(strcat("--type=", type, " --test_class.type=", type)), r))
        assert_not_nothing(r)
        assert_equal(r.s, 1)
        assert_not_nothing(r.t)
        assert_equal(r.t.s, 1)
        assert_true(constructor(Of test_class).erase(type))
        assert_true(constructor(Of test_class2).erase(type))
    End Sub

    <test>
    Private Shared Sub simple_no_type_case()
        assert_true(constructor.register(Function(ByVal v As var) As test_class
                                             Return New test_class(3)
                                         End Function))
        assert_true(convertor.register(Function(ByVal v As var, ByVal i As test_class) As test_class2
                                           Return New test_class2(i, 3)
                                       End Function,
                                       "test_class"))

        Dim r As test_class2 = Nothing
        assert_true(constructor.sync_resolve(New var(), r))
        assert_not_nothing(r)
        assert_equal(r.s, 3)
        assert_not_nothing(r.t)
        assert_equal(r.t.s, 3)
        assert_true(constructor(Of test_class).erase())
        assert_true(constructor(Of test_class2).erase())
    End Sub

    <test>
    Private Shared Sub wrapper_case()
        Const type As String = "wrapper_case_type"
        assert_true(constructor.register(type,
                                         Function(ByVal v As var) As test_class
                                             Return New test_class(2)
                                         End Function))
        assert_true(convertor.register(type,
                                       Function(ByVal v As var, ByVal i As test_class) As test_class2
                                           Return New test_class2(i, 2)
                                       End Function,
                                       "test_class"))
        assert_true(wrapper.register(type,
                                     Function(ByVal v As var, ByVal i As test_class) As test_class
                                         Return New test_class_wrapper(i, 2)
                                     End Function))
        assert_true(wrapper.register(type,
                                     Function(ByVal v As var, ByVal i As test_class2) As test_class2
                                         Return New test_class2_wrapper(i, 2)
                                     End Function))

        Using code_block
            Dim t As test_class = Nothing
            Dim tw As test_class_wrapper = Nothing
            Dim t2 As test_class2 = Nothing
            Dim t2w As test_class2_wrapper = Nothing
            assert_true(constructor.sync_resolve(New var(strcat("--type=", type, " --test_class.type=", type)), t2))
            assert_not_nothing(t2)
            assert_equal(t2.s, 2)
            assert_not_nothing(t2.t)
            assert_equal(t2.t.s, 2)
            assert_false(direct_cast(t2, t2w))
            assert_false(direct_cast(t2.t, tw))
        End Using

        Using code_block
            Dim t As test_class = Nothing
            Dim tw As test_class_wrapper = Nothing
            Dim t2 As test_class2 = Nothing
            Dim t2w As test_class2_wrapper = Nothing
            assert_true(constructor.sync_resolve(New var(strcat("--type=",
                                                                type,
                                                                " --test_class.type=",
                                                                type,
                                                                " --test_class.wrapper=",
                                                                type)),
                                                 t2))
            assert_not_nothing(t2)
            assert_equal(t2.s, 2)
            assert_not_nothing(t2.t)
            assert_equal(t2.t.s, 2)
            assert_false(direct_cast(t2, t2w))
            assert_true(direct_cast(t2.t, tw))
            assert_not_nothing(tw.t)
            assert_equal(tw.t.s, 2)
        End Using

        Using code_block
            Dim t As test_class = Nothing
            Dim tw As test_class_wrapper = Nothing
            Dim t2 As test_class2 = Nothing
            Dim t2w As test_class2_wrapper = Nothing
            assert_true(constructor.sync_resolve(Of test_class2, test_class2_wrapper) _
                                                (New var(strcat("--type=",
                                                                type,
                                                                " --test_class.type=",
                                                                type,
                                                                " --wrapper=",
                                                                type)),
                                                 t2w))
            assert_not_nothing(t2w)
            assert_equal(t2w.s, 2)
            assert_not_nothing(t2w.t2)
            assert_equal(t2w.t2.s, 2)
            assert_not_nothing(t2w.t)
            assert_equal(t2w.t.s, 2)
            assert_false(direct_cast(t2w.t, tw))
        End Using

        Using code_block
            Dim t As test_class = Nothing
            Dim tw As test_class_wrapper = Nothing
            Dim t2 As test_class2 = Nothing
            Dim t2w As test_class2_wrapper = Nothing
            assert_true(constructor.sync_resolve(Of test_class2, test_class2_wrapper) _
                                                (New var(strcat("--type=",
                                                                type,
                                                                " --test_class.type=",
                                                                type,
                                                                " --wrapper=",
                                                                type,
                                                                " --test_class.wrapper=",
                                                                type)),
                                                 t2w))
            assert_not_nothing(t2w)
            assert_equal(t2w.s, 2)
            assert_not_nothing(t2w.t2)
            assert_equal(t2w.t2.s, 2)
            assert_not_nothing(t2w.t)
            assert_equal(t2w.t.s, 2)
            assert_true(direct_cast(t2w.t, tw))
            assert_equal(tw.s, 2)
            assert_not_nothing(tw.t)
            assert_equal(tw.t.s, 2)
        End Using

        assert_true(constructor(Of test_class).erase(type))
        assert_true(constructor(Of test_class2).erase(type))
        assert_true(wrapper(Of test_class).erase(type))
        assert_true(wrapper(Of test_class2).erase(type))
    End Sub

    Private Sub New()
    End Sub
End Class
