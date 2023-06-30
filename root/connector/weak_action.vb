
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _weak_action
    <Extension()> Public Function as_weak_action(ByVal a As Action) As Action
        If a Is Nothing Then
            Return Nothing
        End If
        Dim p As WeakReference = Nothing
        p = New WeakReference(a)
        Return Sub()
                   Dim v As Action = Nothing
                   v = direct_cast(Of Action)(p.Target())
                   If Not v Is Nothing Then
                       v()
                   End If
               End Sub
    End Function
End Module
