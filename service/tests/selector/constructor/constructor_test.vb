
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument
Imports osi.service.selector

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
        assert_true(constructor.register(type,
                                         Function(ByVal v As var) As test_prefer_type_constructor_class
                                             Return New test_prefer_type_constructor_class(1)
                                         End Function))
        assert_true(constructor.register(Function(ByVal v As var) As test_prefer_type_constructor_class
                                             Return New test_prefer_type_constructor_class(2)
                                         End Function))
        Dim r As test_prefer_type_constructor_class = Nothing
        assert_true(constructor.sync_resolve(New var(strcat("--type=", type)), r))
        assert_not_nothing(r)
        assert_equal(r.i, 1)
        assert_true(constructor(Of test_prefer_type_constructor_class).erase(type))
        assert_true(constructor(Of test_prefer_type_constructor_class).erase())
    End Sub

    Private Class test_class
    End Class

    <test>
    Private Shared Sub ignore_empty_type_or_allocator()
        assert_false(constructor(Of test_class).register(Nothing))
        assert_false(constructor(Of test_class).register(Nothing,
                                                         Function(ByVal v As var,
                                                                  ByVal o As pointer(Of test_class)) As event_comb
                                                             Return event_comb.failed()
                                                         End Function))
        assert_false(constructor(Of test_class).register(Nothing, Nothing))
    End Sub

    <test>
    Private Shared Sub return_false_when_constructor_failed()
        Const type As String = "return_false_when_constructor_failed_type"
        assert_true(constructor.register(type,
                                         Function(ByVal v As var, ByRef o As test_class) As Boolean
                                             Return False
                                         End Function))
        Dim r As test_class = Nothing
        assert_false(constructor.sync_resolve(New var(strcat("--type=", type)), r))
        assert_nothing(r)
        assert_true(constructor(Of test_class).erase(type))
    End Sub

    Private Class test_return_false_when_constructor_failed2_class
    End Class

    <test>
    Private Shared Sub return_false_when_constructor_failed2()
        assert_true(constructor.register(Function(ByVal v As var,
                                                  ByRef o As test_return_false_when_constructor_failed2_class) _
                                                 As Boolean
                                             Return False
                                         End Function))
        Dim r As test_return_false_when_constructor_failed2_class = Nothing
        assert_false(constructor.sync_resolve(New var(), r))
        assert_nothing(r)
        assert_true(constructor(Of test_return_false_when_constructor_failed2_class).erase())
    End Sub

    Private Class test_return_false_if_no_constructor_class
    End Class

    <test>
    Private Shared Sub return_false_if_no_constructor()
        assert_true(constructor(Of test_return_false_if_no_constructor_class).empty())
        Dim r As test_return_false_if_no_constructor_class = Nothing
        assert_false(constructor.sync_resolve(New var(), r))
        assert_nothing(r)
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
        assert_true(constructor.register(type,
                                         Function(ByVal v As var) As test_class
                                             Return instance
                                         End Function))
        assert_true(wrapper.register(type,
                                     Function(ByVal v As var, ByVal i As test_class) As test_class
                                         Return New test_class_wrapper(i)
                                     End Function))
        Dim r As test_class = Nothing
        Dim w As test_class_wrapper = Nothing
        assert_true(constructor.sync_resolve(New var(strcat("--type=", type, " --wrapper=", type)), r))
        assert_not_nothing(r)
        assert_true(direct_cast(r, w))
        assert_not_nothing(w)
        assert_reference_equal(w.c, instance)
        assert_true(constructor(Of test_class).erase(type))
        assert_true(wrapper(Of test_class).erase(type))
    End Sub
End Class
