
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class type_info_test
    Private Class finalizer_class
        Public Shared ReadOnly finalizer_called As atomic_int

        Shared Sub New()
            finalizer_called = New atomic_int()
        End Sub

        Protected Overrides Sub Finalize()
            finalizer_called.increment()
            MyBase.Finalize()
        End Sub
    End Class

    Private Class no_finalizer_class
    End Class

    <test>
    Private Shared Sub finalizer_case()
        assert_true(type_info(Of finalizer_class).has_finalizer())
        assert_false(type_info(Of no_finalizer_class).has_finalizer())
        finalizer_class.finalizer_called.set(0)
        Dim f As finalizer_class = Nothing
        f = New finalizer_class()
        assert_not_nothing(type_info(Of finalizer_class).finalizer())
        type_info(Of finalizer_class).finalizer()(f)
        assert_equal(+finalizer_class.finalizer_called, 1)
        type_info(Of finalizer_class).finalizer()(f)
        assert_equal(+finalizer_class.finalizer_called, 2)
    End Sub

    Private Class annotated_constructor_test_attribute
        Inherits Attribute
    End Class

    Private Class annotated_constructor_test_class
        <annotated_constructor_test_attribute>
        Public Sub New(ByVal i As UInt32,
                       ByVal j As Int32,
                       Optional ByVal k As String = "abc",
                       Optional ByVal l As Object = Nothing)
        End Sub
    End Class

    <test>
    Private Shared Sub annotated_constructor_case()
        Dim info As ConstructorInfo = Nothing
        info = type_info(Of annotated_constructor_test_class) _
                       .annotated_constructor_info(Of annotated_constructor_test_attribute)()
        assert_not_nothing(info)
        assert_equal(array_size_i(info.GetParameters()), 4)
        assert_equal(info.GetParameters()(0).ParameterType(), GetType(UInt32))
        assert_equal(info.GetParameters()(1).ParameterType(), GetType(Int32))
        assert_equal(info.GetParameters()(2).ParameterType(), GetType(String))
        assert_equal(info.GetParameters()(3).ParameterType(), GetType(Object))
        assert_equal(info.GetParameters()(0).Name(), "i")
        assert_equal(info.GetParameters()(1).Name(), "j")
        assert_equal(info.GetParameters()(2).Name(), "k")
        assert_equal(info.GetParameters()(3).Name(), "l")
        assert_equal(info.GetParameters()(0).RawDefaultValue(), direct_cast(Of Object)(DBNull.Value))
        assert_equal(info.GetParameters()(1).RawDefaultValue(), direct_cast(Of Object)(DBNull.Value))
        assert_equal(info.GetParameters()(2).RawDefaultValue(), direct_cast(Of Object)("abc"))
        assert_equal(info.GetParameters()(3).RawDefaultValue(), direct_cast(Of Object)(Nothing))
    End Sub
End Class
