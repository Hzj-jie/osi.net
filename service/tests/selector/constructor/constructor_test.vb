
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
    Private Class test_class
        Public ReadOnly i As Int32

        Public Sub New(ByVal i As Int32)
            Me.i = i
        End Sub
    End Class

    Private Class test_class_wrapper
        Inherits test_class

        Public ReadOnly c As test_class

        Public Sub New(ByVal c As test_class, ByVal i As Int32)
            MyBase.New(i)
            Me.c = c
        End Sub
    End Class

    <test>
    Private Shared Sub prefer_type_constructor()
        Const type As String = "prefer_type_constructor_type"
        assert_true(constructor.register(type,
                                         Function(ByVal v As var) As test_class
                                             Return New test_class(1)
                                         End Function))
        assert_true(constructor.register(Function(ByVal v As var) As test_class
                                             Return New test_class(2)
                                         End Function))
        Dim r As test_class = Nothing
        assert_true(constructor.resolve(New var(strcat("--type=", type)), r))
        assert_not_nothing(r)
        assert_equal(r.i, 1)
        assert_true(constructor(Of test_class).erase(type))
        assert_true(constructor(Of test_class).erase())
    End Sub

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
        assert_false(constructor.resolve(New var(strcat("--type=", type)), r))
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
        assert_false(constructor.resolve(New var(), r))
        assert_nothing(r)
        assert_true(constructor(Of test_return_false_when_constructor_failed2_class).erase())
    End Sub

    Private Class test_return_false_if_no_constructor_class
    End Class

    <test>
    Private Shared Sub return_false_if_no_constructor()
        assert_true(constructor(Of test_return_false_if_no_constructor_class).empty())
        Dim r As test_return_false_if_no_constructor_class = Nothing
        assert_false(constructor.resolve(New var(), r))
        assert_nothing(r)
    End Sub
End Class
