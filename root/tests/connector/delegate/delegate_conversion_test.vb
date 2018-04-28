
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class delegate_conversion_test
    Private Interface i1
    End Interface

    Private Interface i2
    End Interface

    Private Class b
        Implements i1
    End Class

    Private Class c
        Inherits b
        Implements i2
    End Class

    Private Shared Function f(ByVal i As b) As b
        Return Nothing
    End Function

    <test>
    Private Shared Sub run()
        success(Of b, b)()
        success(Of b, i1)()
        success(Of c, b)()
        success(Of c, i1)()

        fail(Of i1, b)()
        fail(Of i2, b)()
        fail(Of b, c)()
        fail(Of b, i2)()
        fail(Of i1, c)()
        fail(Of i2, c)()
        fail(Of i1, i2)()
        fail(Of i2, i2)()
    End Sub

    Private Shared Sub success(Of IT, OT)()
        assert_not_nothing([Delegate].CreateDelegate(GetType(Func(Of IT, OT)), method_info()))
    End Sub

    Private Shared Sub fail(Of IT, OT)()
        Try
            assert_nothing([Delegate].CreateDelegate(GetType(Func(Of IT, OT)), method_info()))
            assert_not_reach()
        Catch
            Return
        End Try
    End Sub

    Private Shared Function method_info() As MethodInfo
        Return GetType(delegate_conversion_test).GetMethod("f", binding_flags.static_private_method)
    End Function

    Private Sub New()
    End Sub
End Class
