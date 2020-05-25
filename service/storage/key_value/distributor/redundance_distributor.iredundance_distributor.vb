
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utils

Partial Public Class redundance_distributor
    Implements iredundance_distributor

    Public Function expired() As pointer(Of singleentry) Implements iredundance_distributor.expired
        Return exp
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal excluded_nodes() As Boolean) As event_comb _
                          Implements iredundance_distributor.modify
        assert(array_size(excluded_nodes) = container.size())
        Dim ecs As vector(Of event_comb) = Nothing
        Dim rs As vector(Of pointer(Of Boolean)) = Nothing
        Return New event_comb(Function() As Boolean
                                  ecs = New vector(Of event_comb)()
                                  rs = New vector(Of pointer(Of Boolean))()
                                  For i As Int32 = 0 To container.size() - 1
                                      If Not excluded_nodes(i) Then
                                          Dim ec As event_comb = Nothing
                                          Dim r As pointer(Of Boolean) = Nothing
                                          r = New pointer(Of Boolean)()
                                          ec = container(i).modify(key, value, ts, r)
                                          ecs.emplace_back(ec)
                                          rs.emplace_back(r)
                                      End If
                                  Next
                                  If ecs.empty() Then
                                      Return goto_end()
                                  Else
                                      Return waitfor(+ecs) AndAlso
                                             goto_next()
                                  End If
                              End Function,
                              Function() As Boolean
                                  assert(ecs.size() = rs.size())
                                  For i As Int32 = 0 To ecs.size() - 1
                                      If Not ecs(i).end_result() OrElse Not (+(rs(i))) Then
                                          Return False
                                      End If
                                  Next
                                  Return goto_end()
                              End Function)
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As pointer(Of Byte()),
                         ByVal ts As pointer(Of Int64),
                         ByVal nodes_result As pointer(Of Boolean())) As event_comb _
                        Implements iredundance_distributor.read
        Dim bs() As pointer(Of Byte()) = Nothing
        Dim tss() As pointer(Of Int64) = Nothing
        Dim ecs() As event_comb = Nothing
        Return New event_comb(Function() As Boolean
                                  ReDim bs(container.size() - 1)
                                  ReDim tss(container.size() - 1)
                                  ReDim ecs(container.size() - 1)
                                  For i As Int32 = 0 To container.size() - 1
                                      bs(i) = New pointer(Of Byte())()
                                      tss(i) = New pointer(Of Int64)()
                                      ecs(i) = container(i).read(key, bs(i), tss(i))
                                  Next
                                  Return waitfor(ecs) AndAlso
                                         goto_next()
                              End Function,
                              Function() As Boolean
                                  Dim nr() As Boolean = Nothing
                                  Dim br() As Byte = Nothing
                                  Dim bt As Int64 = 0
                                  Return compare(ecs, bs, tss, nr, br, bt) AndAlso
                                         eva(result, br) AndAlso
                                         eva(ts, bt) AndAlso
                                         eva(nodes_result, nr) AndAlso
                                         goto_end()
                              End Function)
    End Function

    Public Function wrappered() As key_locked_istrkeyvt Implements iredundance_distributor.wrappered
        Return wrap
    End Function
End Class
