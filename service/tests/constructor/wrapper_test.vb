
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes
Imports osi.service.argument
Imports osi.service.constructor

<test>
Partial Public NotInheritable Class wrapper_test
    Private Class test_class(Of PROTECTOR)
        Public ReadOnly t As test_class(Of PROTECTOR)

        Public Sub New()
            Me.New(Nothing)
        End Sub

        Public Sub New(ByVal t As test_class(Of PROTECTOR))
            Me.t = t
        End Sub

        Public Sub assert_wrap(ByVal count As Int32)
            Dim this As test_class(Of PROTECTOR) = Nothing
            this = Me
            While count > 0
                assert_not_nothing(this.t)
                this = this.t
                count -= 1
            End While
            assert_nothing(this.t)
        End Sub
    End Class

    Private Class test_class
        Inherits test_class(Of Object)

        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByVal t As test_class)
            MyBase.New(t)
        End Sub
    End Class

    Private Interface multiple_registers_protector
    End Interface

    <test>
    Private Shared Sub multiple_registers()
        Const wrapper_count As UInt32 = 10
        Const type As String = "multiple_register_wrapper"
        For i As UInt32 = 0 To wrapper_count - uint32_1
            assert_true(wrapper.register(type,
                                         Function(ByVal v As var,
                                                  ByVal j As test_class(Of multiple_registers_protector)) _
                                                 As test_class(Of multiple_registers_protector)
                                             Return New test_class(Of multiple_registers_protector)(j)
                                         End Function))
        Next
        Dim r As test_class(Of multiple_registers_protector) = Nothing
        assert_true(wrapper.wrap(New var("--wrapper=multiple_register_wrapper"),
                                 New test_class(Of multiple_registers_protector)(),
                                 r))
        r.assert_wrap(wrapper_count)
        assert_true(wrapper(Of test_class(Of multiple_registers_protector)).erase(type))
    End Sub

    <test>
    Private Shared Sub ignore_empty_type_or_wrapper()
        assert_false(wrapper(Of test_class).register(Nothing))
        assert_false(wrapper(Of test_class).register(Nothing,
                                                     Function(ByVal v As var,
                                                              ByVal j As test_class,
                                                              ByRef o As test_class) As Boolean
                                                         Return True
                                                     End Function))
        assert_false(wrapper(Of test_class).register(Nothing, Nothing, Nothing))
        assert_false(wrapper(Of test_class).erase(default_str))
        assert_false(wrapper(Of test_class).erase())
    End Sub

    <test>
    Private Shared Sub return_false_when_wrapper_failed()
        Const type As String = "return_false_when_wrapper_failed_wrapper"
        assert_true(wrapper.register(type,
                                     Function(ByVal v As var, ByVal j As test_class, ByRef o As test_class) As Boolean
                                         Return False
                                     End Function))
        Dim r As test_class = Nothing
        assert_false(wrapper.wrap(New var(strcat("--wrapper=", type)), New test_class(), r))
        assert_not_nothing(r)
        assert_true(wrapper(Of test_class).erase(type))
    End Sub

    Private Interface return_false_when_wrapper_failed2_protector
    End Interface

    <test>
    Private Shared Sub return_false_when_wrapper_failed2()
        assert_true(wrapper.register(Function(ByVal v As var,
                                              ByVal j As test_class(Of return_false_when_wrapper_failed2_protector),
                                              ByRef o As test_class(Of return_false_when_wrapper_failed2_protector)) _
                                             As Boolean
                                         Return False
                                     End Function))
        Dim r As test_class(Of return_false_when_wrapper_failed2_protector) = Nothing
        assert_false(wrapper.wrap(New var(), New test_class(Of return_false_when_wrapper_failed2_protector)(), r))
        assert_not_nothing(r)
        assert_true(wrapper(Of test_class(Of return_false_when_wrapper_failed2_protector)).erase())
    End Sub

    Private Interface return_null_if_wrapper_returns_null_protector
    End Interface

    <test>
    Private Shared Sub return_null_if_wrapper_returns_null()
        assert_true(wrapper.register(Function(ByVal v As var,
                                              ByVal j As test_class(Of return_null_if_wrapper_returns_null_protector),
                                              ByRef o As test_class(Of return_null_if_wrapper_returns_null_protector)) _
                                             As Boolean
                                         o = Nothing
                                         Return True
                                     End Function))
        Dim r As test_class(Of return_null_if_wrapper_returns_null_protector) = Nothing
        assert_true(wrapper.wrap(New var(), New test_class(Of return_null_if_wrapper_returns_null_protector)(), r))
        assert_nothing(r)
        assert_true(wrapper(Of test_class(Of return_null_if_wrapper_returns_null_protector)).erase())
    End Sub

    Private Interface return_null_if_wrapper_returns_null2_protector
    End Interface

    <test>
    Private Shared Sub return_null_if_wrapper_returns_null2()
        Const type As String = "return_null_if_wrapper_returns_null2_wrapper"
        assert_true(wrapper.register(
                type,
                Function(ByVal v As var,
                         ByVal j As test_class(Of return_null_if_wrapper_returns_null2_protector),
                         ByRef o As test_class(Of return_null_if_wrapper_returns_null2_protector)) _
                        As Boolean
                    o = Nothing
                    Return True
                End Function))
        Dim r As test_class(Of return_null_if_wrapper_returns_null2_protector) = Nothing
        assert_true(wrapper.wrap(New var(strcat("--wrapper=", type)),
                                 New test_class(Of return_null_if_wrapper_returns_null2_protector)(),
                                 r))
        assert_nothing(r)
        assert_true(wrapper(Of test_class(Of return_null_if_wrapper_returns_null2_protector)).erase(type))
    End Sub

    Private Class test_return_true_if_no_wrapper_class
    End Class

    <test>
    Private Shared Sub return_true_if_no_wrapper()
        assert_true(wrapper(Of test_return_true_if_no_wrapper_class).empty())
        Dim r As test_return_true_if_no_wrapper_class = Nothing
        assert_true(wrapper.wrap(New var(), [default](Of test_return_true_if_no_wrapper_class).null, r))
        assert_nothing(r)
        assert_true(wrapper.wrap(New var(), New test_return_true_if_no_wrapper_class(), r))
        assert_not_nothing(r)
    End Sub

    Private Sub New()
    End Sub
End Class
