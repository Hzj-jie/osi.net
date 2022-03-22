
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.service.constructor

' A scope implementation for bstyle and b2style
Public Class scope_b(Of T As scope_b(Of T))
    Inherits scope(Of T)

    Private ReadOnly incs As includes_t

    Protected Sub New(ByVal parent As T)
        MyBase.New(parent)
        If parent Is Nothing Then
            incs = New includes_t()
        End If
    End Sub
End Class
