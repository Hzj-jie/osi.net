
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class assert_which
    Public Shared Function [of](ByVal i As Double) As double_assertion
        Return New double_assertion(i)
    End Function

    Public Shared Function [of](ByVal i As Decimal) As decimal_assertion
        Return New decimal_assertion(i)
    End Function

    Public Shared Function [of](ByVal i As Int32) As int32_assertion
        Return New int32_assertion(i)
    End Function

    Public Shared Function [of](ByVal i As UInt32) As uint32_assertion
        Return New uint32_assertion(i)
    End Function

    Public Shared Function [of](ByVal i As UInt64) As uint64_assertion
        Return New uint64_assertion(i)
    End Function

    Public Shared Function [of](ByVal i As Object) As object_assertion
        Return New object_assertion(i)
    End Function

    Public Shared Function [of](Of T)(ByVal i As T) As T_assertion(Of T)
        Return New T_assertion(Of T)(i)
    End Function

    Private Sub New()
    End Sub
End Class
