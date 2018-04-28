
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

' The typical base implementation of a module. It contains two steps, filtering and processing.
Public MustInherit Class filtered_module
    Implements module_handle.module

    Private ReadOnly filter As context_filter

    Protected Sub New(ByVal filter As context_filter)
        assert(Not filter Is Nothing)
        Me.filter = filter
    End Sub

    Protected MustOverride Sub process(ByVal context As server.context)

    Public Function context_received(ByVal context As server.context) As Boolean _
                                    Implements module_handle.module.context_received
        If filter.select(context) Then
            process(context)
            Return True
        End If
        Return False
    End Function
End Class
