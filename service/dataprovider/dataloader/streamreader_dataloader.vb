
Imports System.IO
Imports System.Text
Imports osi.root.procedure
Imports osi.root.formation
Imports osi.root.delegates
Imports osi.root.threadpool
Imports osi.root.connector
Imports osi.root.utils

Public MustInherit Class streamreader_dataloader(Of T)
    Implements istreamdataloader(Of T)

    Private ReadOnly e As Encoding

    Protected Sub New(Optional ByVal e As Encoding = Nothing)
        Me.e = e
    End Sub

    Protected MustOverride Function load(ByVal s As StreamReader, ByRef result As T) As Boolean

    Public Function load(ByVal s As Stream,
                         ByVal result As ref(Of T)) As event_comb Implements istreamdataloader(Of T).load
        Dim suc As Boolean = False
        Return New event_comb(Function() As Boolean
                                  Return waitfor(Sub()
                                                     Dim r As StreamReader = Nothing
                                                     If e Is Nothing Then
                                                         r = New StreamReader(s)
                                                     Else
                                                         r = New StreamReader(s, e)
                                                     End If
                                                     Dim rst As T = Nothing
                                                     suc = load(r, rst) AndAlso eva(result, rst)
                                                     r.Close()
                                                     r.Dispose()
                                                 End Sub) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Return suc AndAlso
                                         goto_end()
                              End Function)
    End Function
End Class
