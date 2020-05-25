
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument
Imports osi.service.constructor
Imports c = osi.service.constructor

<test>
Partial Public Class constructor_test
    Private Class test_prefer_type_constructor_class
        Public ReadOnly i As Int32

        Public Sub New(ByVal i As Int32)
            Me.i = i
        End Sub
    End Class

    <test>
    Private Shared Sub prefer_type_constructor()
        Const type As String = "prefer_type_constructor_type"
        assertion.is_true(c.constructor.register(type,
                                           Function(ByVal v As var) As test_prefer_type_constructor_class
                                               Return New test_prefer_type_constructor_class(1)
                                           End Function))
        assertion.is_true(c.constructor.register(Function(ByVal v As var) As test_prefer_type_constructor_class
                                               Return New test_prefer_type_constructor_class(2)
                                           End Function))
        Dim r As test_prefer_type_constructor_class = Nothing
        assertion.is_true(c.constructor.sync_resolve(New var(strcat("--type=", type)), r))
        assertion.is_not_null(r)
        assertion.equal(r.i, 1)
        assertion.is_true(c.constructor(Of test_prefer_type_constructor_class).erase(type))
        assertion.is_true(c.constructor(Of test_prefer_type_constructor_class).erase())
    End Sub

    Private Class test_class
    End Class

    <test>
    Private Shared Sub ignore_empty_type_or_allocator()
        assertion.is_false(c.constructor(Of test_class).register(Nothing))
        assertion.is_false(c.constructor(Of test_class).register(Nothing,
                                                           Function(ByVal v As var,
                                                                  ByVal o As ref(Of test_class)) As event_comb
                                                               Return event_comb.failed()
                                                           End Function))
        assertion.is_false(c.constructor(Of test_class).register(Nothing, Nothing))
    End Sub

    <test>
    Private Shared Sub return_false_when_constructor_failed()
        Const type As String = "return_false_when_constructor_failed_type"
        assertion.is_true(c.constructor.register(type,
                                           Function(ByVal v As var, ByRef o As test_class) As Boolean
                                               Return False
                                           End Function))
        Dim r As test_class = Nothing
        assertion.is_false(c.constructor.sync_resolve(New var(strcat("--type=", type)), r))
        assertion.is_null(r)
        assertion.is_true(c.constructor(Of test_class).erase(type))
    End Sub

    Private Class test_return_false_when_constructor_failed2_class
    End Class

    <test>
    Private Shared Sub return_false_when_constructor_failed2()
        assertion.is_true(c.constructor.register(Function(ByVal v As var,
                                                    ByRef o As test_return_false_when_constructor_failed2_class) _
                                                   As Boolean
                                               Return False
                                           End Function))
        Dim r As test_return_false_when_constructor_failed2_class = Nothing
        assertion.is_false(c.constructor.sync_resolve(New var(), r))
        assertion.is_null(r)
        assertion.is_true(c.constructor(Of test_return_false_when_constructor_failed2_class).erase())
    End Sub

    Private Class test_return_false_if_no_constructor_class
    End Class

    <test>
    Private Shared Sub return_false_if_no_constructor()
        assertion.is_true(c.constructor(Of test_return_false_if_no_constructor_class).empty())
        Dim r As test_return_false_if_no_constructor_class = Nothing
        assertion.is_false(c.constructor.sync_resolve(New var(), r))
        assertion.is_null(r)
    End Sub

    Private Class test_class_wrapper
        Inherits test_class

        Public ReadOnly c As test_class

        Public Sub New(ByVal c As test_class)
            Me.c = c
        End Sub
    End Class

    <test>
    Private Shared Sub return_value_is_wrapped()
        Const type As String = "return_type_is_wrapped_type"
        Dim instance As test_class = Nothing
        instance = New test_class()
        assertion.is_true(c.constructor.register(type,
                                           Function(ByVal v As var) As test_class
                                               Return instance
                                           End Function))
        assertion.is_true(wrapper.register(type,
                                     Function(ByVal v As var, ByVal i As test_class) As test_class
                                         Return New test_class_wrapper(i)
                                     End Function))
        Dim r As test_class = Nothing
        Dim w As test_class_wrapper = Nothing
        assertion.is_true(c.constructor.sync_resolve(New var(strcat("--type=", type, " --wrapper=", type)), r))
        assertion.is_not_null(r)
        assertion.is_true(direct_cast(r, w))
        assertion.is_not_null(w)
        assertion.reference_equal(w.c, instance)
        assertion.is_true(c.constructor(Of test_class).erase(type))
        assertion.is_true(wrapper(Of test_class).erase(type))
    End Sub
End Class
