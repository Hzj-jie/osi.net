
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.procedure

' The typical base implementation of a module. It contains two steps, filtering and execute.
Public MustInherit Class filtered_module
    Implements module_handle.module

    Private ReadOnly filter As context_filter

    Protected Sub New(ByVal filter As context_filter)
        assert(Not filter Is Nothing)
        Me.filter = filter
    End Sub

    Protected Sub New(ByVal mi As MemberInfo)
        Me.New(context_filter.[New](mi))
    End Sub

    Protected MustOverride Function execute(ByVal context As server.context) As event_comb

    Public Function context_received(ByVal context As server.context) As Boolean _
                                    Implements module_handle.module.context_received
        Return procedure_handle.process_context(context, AddressOf filter.select, AddressOf execute)
    End Function
End Class
