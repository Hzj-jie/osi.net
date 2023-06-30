
' TODO: Move to template?

Option Explicit On
Option Infer Off
Option Strict On

' Creating a template type protector by joining two types together.
Public Interface joint_type(Of T1, T2)
End Interface

Public Interface joint_type(Of T1, T2, T3)
End Interface

Public Interface joint_type(Of T1, T2, T3, T4)
End Interface

Public Interface joint_type(Of T1, T2, T3, T4, T5)
End Interface

Public Interface joint_type(Of T1, T2, T3, T4, T5, T6)
End Interface

Public Interface joint_type(Of T1, T2, T3, T4, T5, T6, T7)
End Interface

Public Interface joint_type(Of T1, T2, T3, T4, T5, T6, T7, T8)
End Interface

Public Interface joint_type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9)
End Interface

Public Interface joint_type(Of T1, T2, T3, T4, T5, T6, T7, T8, T9, T10)
End Interface

Public NotInheritable Class joint_type
    Public Shared Function [of](ByVal i As Type, ByVal j As Type) As Type
        If i Is Nothing OrElse j Is Nothing Then
            Return Nothing
        End If
        Return GetType(joint_type(Of ,)).MakeGenericType(i, j)
    End Function

    Public Shared Function [of](ByVal i As Type, ByVal j As Type, ByVal k As Type) As Type
        If i Is Nothing OrElse j Is Nothing OrElse k Is Nothing Then
            Return Nothing
        End If
        Return GetType(joint_type(Of ,,)).MakeGenericType(i, j, k)
    End Function

    Private Sub New()
    End Sub
End Class