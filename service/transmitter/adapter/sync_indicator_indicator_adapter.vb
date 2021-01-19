
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utils

Public Class sync_indicator_indicator_adapter
    Implements indicator

    Private ReadOnly i As sync_indicator

    Public Sub New(ByVal i As sync_indicator)
        Me.i = i
    End Sub

    Public Function indicate(ByVal pending As ref(Of Boolean)) As event_comb Implements indicator.indicate
        Return sync_async(Function() As Boolean
                              If i Is Nothing Then
                                  Return False
                              Else
                                  Dim r As Boolean = False
                                  Return i.indicate(r) AndAlso
                                         eva(pending, r)
                              End If
                          End Function)
    End Function
End Class

Public MustInherit Class never_fail_sync_indicator
    Implements sync_indicator

    Protected MustOverride Function indicate() As Boolean

    Public Function indicate(ByRef pending As Boolean) As Boolean Implements sync_indicator.indicate
        pending = indicate()
        Return True
    End Function
End Class
