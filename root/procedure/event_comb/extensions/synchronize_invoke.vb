
Option Explicit On
Option Infer Off
Option Strict On

Imports System.ComponentModel
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _synchronize_invoke
    <Extension()> Public Function invoke(ByVal si As ISynchronizeInvoke,
                                         ByVal method As [Delegate],
                                         ByVal args() As Object,
                                         ByVal o As ref(Of Object)) As event_comb
        If si Is Nothing Then
            Return event_comb.failed()
        End If

        Return event_comb_async_operation.ctor(Function(ByVal i As AsyncCallback) As IAsyncResult
                                                   Return si.BeginInvoke(method, args)
                                               End Function,
                                               Function(ByVal r As IAsyncResult) As Object
                                                   Return si.EndInvoke(r)
                                               End Function,
                                               o)
    End Function
End Module
