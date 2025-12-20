
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Public NotInheritable Class current_case
    Public Shared Function [with](ByVal c As [case]) As IDisposable
        Return instance_stack(Of [case], host).with(c)
    End Function

    ' Call from a different thread with
    ' Thread.post(forward_current_case(d))
    Public Shared Function forward(ByVal d As Action) As Action
        assert(Not d Is Nothing)
        Dim c As [case] = instance_stack(Of [case], host).current()
        assert(Not c Is Nothing)
        Return Sub()
                   Using instance_stack(Of [case], host).with(c)
                       d()
                   End Using
               End Sub
    End Function

    Public Shared Function is_null() As Boolean
        Return instance_stack(Of [case], host).empty()
    End Function

    Public Shared Function [of]() As [case]
        Dim r As [case] = Nothing
        If instance_stack(Of [case], host).back(r) Then
            Return r
        End If
        Return Nothing
    End Function

    Private Sub New()
    End Sub
End Class
