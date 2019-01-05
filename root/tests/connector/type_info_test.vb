
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
        assertion.is_true(type_info(Of finalizer_class).has_finalizer())
        assertion.is_false(type_info(Of no_finalizer_class).has_finalizer())
        finalizer_class.finalizer_called.set(0)
        Dim f As finalizer_class = Nothing
        f = New finalizer_class()
        assertion.is_not_null(type_info(Of finalizer_class).finalizer())
        type_info(Of finalizer_class).finalizer()(f)
        assertion.equal(+finalizer_class.finalizer_called, 1)
        type_info(Of finalizer_class).finalizer()(f)
        assertion.equal(+finalizer_class.finalizer_called, 2)
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
        assertion.is_not_null(info)
        assertion.equal(array_size_i(info.GetParameters()), 4)
        assertion.equal(info.GetParameters()(0).ParameterType(), GetType(UInt32))
        assertion.equal(info.GetParameters()(1).ParameterType(), GetType(Int32))
        assertion.equal(info.GetParameters()(2).ParameterType(), GetType(String))
        assertion.equal(info.GetParameters()(3).ParameterType(), GetType(Object))
        assertion.equal(info.GetParameters()(0).Name(), "i")
        assertion.equal(info.GetParameters()(1).Name(), "j")
        assertion.equal(info.GetParameters()(2).Name(), "k")
        assertion.equal(info.GetParameters()(3).Name(), "l")
        assertion.equal(info.GetParameters()(0).RawDefaultValue(), direct_cast(Of Object)(DBNull.Value))
        assertion.equal(info.GetParameters()(1).RawDefaultValue(), direct_cast(Of Object)(DBNull.Value))
        assertion.equal(info.GetParameters()(2).RawDefaultValue(), direct_cast(Of Object)("abc"))
        assertion.equal(info.GetParameters()(3).RawDefaultValue(), direct_cast(Of Object)(Nothing))
    End Sub

    Private Enum e1 As Int32
        a
    End Enum

    Private Enum e2 As SByte
        a
    End Enum

    <test>
    Private Shared Sub enum_case()
        assertion.is_true(type_info(Of e1).is_enum)
        assertion.is_false(type_info(Of Integer).is_enum)
        assertion.is_true(enum_def(Of e1).underlying_type() Is GetType(Int32))
        assertion.is_true(enum_def(Of e2).underlying_type() Is GetType(SByte))
    End Sub
End Class
