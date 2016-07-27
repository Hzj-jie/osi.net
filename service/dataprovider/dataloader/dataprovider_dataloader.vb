
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.connector
Imports osi.root.utils

Public Class dataprovider_dataloader(Of T)
    Implements idataloader(Of T)

    Private ReadOnly loader As Func(Of idataprovider(), T)
    Private ReadOnly dps() As weak_pointer(Of idataprovider)

    Public Sub New(ByVal loader As Func(Of idataprovider(), T),
                   ByVal ParamArray dps() As idataprovider)
        assert(Not isemptyarray(dps))
        Me.loader = loader
        Me.dps = make_weak_pointers(dps)
    End Sub

    Public Sub New(ByVal ParamArray dps() As idataprovider)
        Me.New(Nothing, dps)
    End Sub

    Protected Overridable Function load(ByVal dps() As idataprovider) As T
        assert(Not loader Is Nothing)
        Return loader(dps)
    End Function

    Public Function load(ByVal localfile As String,
                         ByVal result As pointer(Of T)) As event_comb Implements idataloader(Of T).load
        Return New event_comb(Function() As Boolean
                                  Dim v() As idataprovider = Nothing
                                  ReDim v(array_size(dps) - 1)
                                  For i As Int32 = 0 To array_size(dps) - 1
                                      Dim p As idataprovider = Nothing
                                      p = (+(dps(i)))
                                      If p Is Nothing OrElse
                                         Not p.valid() Then
                                          Return False
                                      Else
                                          v(i) = p
                                      End If
                                  Next
                                  Return eva(result, load(v)) AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
