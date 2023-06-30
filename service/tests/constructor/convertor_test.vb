
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument
Imports osi.service.constructor
Imports c = osi.service.constructor

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
            MyBase.New(assert_which.of(t).is_not_null().t, s)
            Me.t2 = t
        End Sub
    End Class

    <test>
    Private Shared Sub simple_case()
        Const type As String = "simple_case_type"
        assertion.is_true(c.constructor.register(type,
                                           Function(ByVal v As var) As test_class
                                               Return New test_class(1)
                                           End Function))
        assertion.is_true(c.convertor.register(type,
                                         Function(ByVal v As var, ByVal i As test_class) As test_class2
                                             Return New test_class2(i, 1)
                                         End Function,
                                         "test_class"))

        Dim r As test_class2 = Nothing
        assertion.is_true(c.constructor.sync_resolve(New var(strcat("--type=", type, " --test_class.type=", type)), r))
        assertion.is_not_null(r)
        assertion.equal(r.s, 1)
        assertion.is_not_null(r.t)
        assertion.equal(r.t.s, 1)
        assertion.is_true(c.constructor(Of test_class).erase(type))
        assertion.is_true(c.constructor(Of test_class2).erase(type))
    End Sub

    <test>
    Private Shared Sub simple_no_type_case()
        assertion.is_true(c.constructor.register(Function(ByVal v As var) As test_class
                                               Return New test_class(3)
                                           End Function))
        assertion.is_true(c.convertor.register(Function(ByVal v As var, ByVal i As test_class) As test_class2
                                             Return New test_class2(i, 3)
                                         End Function,
                                         "test_class"))

        Dim r As test_class2 = Nothing
        assertion.is_true(c.constructor.sync_resolve(New var(), r))
        assertion.is_not_null(r)
        assertion.equal(r.s, 3)
        assertion.is_not_null(r.t)
        assertion.equal(r.t.s, 3)
        assertion.is_true(c.constructor(Of test_class).erase())
        assertion.is_true(c.constructor(Of test_class2).erase())
    End Sub

    <test>
    Private Shared Sub wrapper_case()
        Const type As String = "wrapper_case_type"
        assertion.is_true(c.constructor.register(type,
                                           Function(ByVal v As var) As test_class
                                               Return New test_class(2)
                                           End Function))
        assertion.is_true(c.convertor.register(type,
                                         Function(ByVal v As var, ByVal i As test_class) As test_class2
                                             Return New test_class2(i, 2)
                                         End Function,
                                       "test_class"))
        assertion.is_true(wrapper.register(type,
                                     Function(ByVal v As var, ByVal i As test_class) As test_class
                                         Return New test_class_wrapper(i, 2)
                                     End Function))
        assertion.is_true(wrapper.register(type,
                                     Function(ByVal v As var, ByVal i As test_class2) As test_class2
                                         Return New test_class2_wrapper(i, 2)
                                     End Function))

        Using code_block
            Dim t As test_class = Nothing
            Dim tw As test_class_wrapper = Nothing
            Dim t2 As test_class2 = Nothing
            Dim t2w As test_class2_wrapper = Nothing
            assertion.is_true(c.constructor.sync_resolve(New var(strcat("--type=", type, " --test_class.type=", type)), t2))
            assertion.is_not_null(t2)
            assertion.equal(t2.s, 2)
            assertion.is_not_null(t2.t)
            assertion.equal(t2.t.s, 2)
            assertion.is_false(direct_cast(t2, t2w))
            assertion.is_false(direct_cast(t2.t, tw))
        End Using

        Using code_block
            Dim t As test_class = Nothing
            Dim tw As test_class_wrapper = Nothing
            Dim t2 As test_class2 = Nothing
            Dim t2w As test_class2_wrapper = Nothing
            assertion.is_true(c.constructor.sync_resolve(New var(strcat("--type=",
                                                                  type,
                                                                  " --test_class.type=",
                                                                  type,
                                                                  " --test_class.wrapper=",
                                                                  type)),
                                                   t2))
            assertion.is_not_null(t2)
            assertion.equal(t2.s, 2)
            assertion.is_not_null(t2.t)
            assertion.equal(t2.t.s, 2)
            assertion.is_false(direct_cast(t2, t2w))
            assertion.is_true(direct_cast(t2.t, tw))
            assertion.is_not_null(tw.t)
            assertion.equal(tw.t.s, 2)
        End Using

        Using code_block
            Dim t As test_class = Nothing
            Dim tw As test_class_wrapper = Nothing
            Dim t2 As test_class2 = Nothing
            Dim t2w As test_class2_wrapper = Nothing
            assertion.is_true(c.constructor.sync_resolve(Of test_class2, test_class2_wrapper) _
                                                  (New var(strcat("--type=",
                                                                  type,
                                                                  " --test_class.type=",
                                                                  type,
                                                                  " --wrapper=",
                                                                  type)),
                                                   t2w))
            assertion.is_not_null(t2w)
            assertion.equal(t2w.s, 2)
            assertion.is_not_null(t2w.t2)
            assertion.equal(t2w.t2.s, 2)
            assertion.is_not_null(t2w.t)
            assertion.equal(t2w.t.s, 2)
            assertion.is_false(direct_cast(t2w.t, tw))
        End Using

        Using code_block
            Dim t As test_class = Nothing
            Dim tw As test_class_wrapper = Nothing
            Dim t2 As test_class2 = Nothing
            Dim t2w As test_class2_wrapper = Nothing
            assertion.is_true(c.constructor.sync_resolve(Of test_class2, test_class2_wrapper) _
                                                  (New var(strcat("--type=",
                                                                  type,
                                                                  " --test_class.type=",
                                                                  type,
                                                                  " --wrapper=",
                                                                  type,
                                                                  " --test_class.wrapper=",
                                                                  type)),
                                                   t2w))
            assertion.is_not_null(t2w)
            assertion.equal(t2w.s, 2)
            assertion.is_not_null(t2w.t2)
            assertion.equal(t2w.t2.s, 2)
            assertion.is_not_null(t2w.t)
            assertion.equal(t2w.t.s, 2)
            assertion.is_true(direct_cast(t2w.t, tw))
            assertion.equal(tw.s, 2)
            assertion.is_not_null(tw.t)
            assertion.equal(tw.t.s, 2)
        End Using

        assertion.is_true(c.constructor(Of test_class).erase(type))
        assertion.is_true(c.constructor(Of test_class2).erase(type))
        assertion.is_true(wrapper(Of test_class).erase(type))
        assertion.is_true(wrapper(Of test_class2).erase(type))
    End Sub

    Private Sub New()
    End Sub
End Class
