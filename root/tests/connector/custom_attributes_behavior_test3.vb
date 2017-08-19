
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.utt

Public Class custom_attributes_behavior_test3
    Inherits [case]

    Private Shared ReadOnly i As atomic_int

    Private Class test
        Inherits Attribute

        Public ReadOnly i As Int32

        Public Sub New()
            Try
                assert_nothing(custom_attributes_behavior_test3.i)
                custom_attributes_behavior_test3.i.increment()
            Catch
                Return
            Finally
                Me.i = +(custom_attributes_behavior_test3.i)
            End Try
            assert_true(False)
        End Sub
    End Class

    <test()>
    Private Class C
    End Class

    <test()>
    Private Class C2
    End Class

    <test()>
    Private Class C3
    End Class

    Public Overrides Function run() As Boolean
        assert_equal(+i, 0)
        Dim a As test = Nothing
        assert_true(GetType(C).custom_attribute(Of test)(a))
        assert_equal(a.i, 0)
        assert_true(GetType(C2).custom_attribute(Of test)(a))
        assert_equal(a.i, 0)
        assert_true(GetType(C3).custom_attribute(Of test)(a))
        assert_equal(a.i, 0)
        Return True
    End Function
End Class

' NullReferenceException may throw at runtime.
#If CALLSTACK Then
c, 15-11-25 22:09:14, caught unhandled exception, type System.NullReferenceException, source osi.tests.root.connector, message Object reference not set to an instance of an object.
, stacktrace    at osi.tests.root.connector.custom_attributes_behavior_test3.test..ctor() in C:\Users\Hzj_jie\Documents\Visual Studio 2013\Projects\gemini\osi\root\tests\c
onnector\custom_attributes_behavior_test3.vb:line 18
   at System.RuntimeTypeHandle.CreateCaInstance(RuntimeMethodHandle ctor)
   at System.Reflection.CustomAttribute.GetCustomAttributes(Module decoratedModule, Int32 decoratedMetadataToken, Int32 pcaCount, RuntimeType attributeFilterType, Boolean mustBeInh
eritable, IList derivedAttributes)
   at System.Reflection.CustomAttribute.GetCustomAttributes(RuntimeType type, RuntimeType caType, Boolean inherit)
   at osi.root.connector._custom_attributes.custom_attributes[AT](Type this, AT[]& o, Boolean inherit) in C:\Users\Hzj_jie\Documents\Visual Studio 2013\Projects\gemini\osi\root\con
nector\custom_attributes.vb:line 53
   at osi.root.connector._custom_attributes.custom_attribute[AT](Type this, AT& o, Boolean inherit) in C:\Users\Hzj_jie\Documents\Visual Studio 2013\Projects\gemini\osi\root\connec
tor\custom_attributes.vb:line 17
   at osi.tests.root.connector.custom_attributes_behavior_test3.run() in C:\Users\Hzj_jie\Documents\Visual Studio 2013\Projects\gemini\osi\root\tests\connector\custom_attributes_be
havior_test3.vb:line 38
   at osi.root.connector._delegate.do_[RT](Func`1 d, RT false_value) in C:\Users\Hzj_jie\Documents\Visual Studio 2013\Projects\gemini\osi\root\connector\delegate.vb:line 18, C:\Use
rs\Hzj_jie\Documents\Visual Studio 2013\Projects\gemini\osi\root\connector\unhandled_exception.vb(8):osi.root.connector.dll.osi.root.connector._unhandled_exception.log_unhandled_ex
ception
#End If
