
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public Class ms_timing_counter
    Inherits timing_counter

    Public Sub New(ByVal p As ref(Of Int64))
        MyBase.New(p)
    End Sub

    Protected Overrides Function now() As Int64
        Return DateTime.Now().milliseconds()
    End Function
End Class
