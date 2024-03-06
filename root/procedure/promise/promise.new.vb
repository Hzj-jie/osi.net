
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public NotInheritable Class promise
    Public Sub New(ByVal executor As Action(Of Action(Of Object)))
        Me.New(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                   assert(Not executor Is Nothing)
                   executor(resolve)
               End Sub)
    End Sub

    Public Sub New(ByVal executor As Func(Of Object))
        Me.New(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                   assert(Not executor Is Nothing)
                   resolve(executor())
               End Sub)
    End Sub
End Class
