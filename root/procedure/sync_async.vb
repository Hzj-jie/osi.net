
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.utils

Public Module _sync_async
    Public Function sync_async(ByVal d As Func(Of Boolean), ByRef ec As event_comb) As Boolean
        If d Is Nothing Then
            Return False
        Else
            ec = New event_comb(Function() As Boolean
                                    Return d() AndAlso
                                           goto_end()
                                End Function)
            Return True
        End If
    End Function

    Public Function sync_async(Of T)(ByVal d As _do(Of T, Boolean),
                                     ByVal p As pointer(Of T),
                                     ByRef ec As event_comb,
                                     Optional ByVal default_value As T = Nothing) As Boolean
        If d Is Nothing OrElse p Is Nothing Then
            Return False
        Else
            Return sync_async(Function() As Boolean
                                  Dim r As T = default_value
                                  Return d(r) AndAlso
                                         eva(p, r)
                              End Function,
                              ec)
        End If
    End Function

    Public Function sync_async(ByVal d As Action, ByRef ec As event_comb) As Boolean
        If d Is Nothing Then
            Return False
        Else
            Return sync_async(Function() As Boolean
                                  d()
                                  Return True
                              End Function,
                              ec)
        End If
    End Function

    Public Function sync_async(Of T)(ByVal d As Func(Of T),
                                     ByVal p As pointer(Of T),
                                     ByRef ec As event_comb,
                                     Optional ByVal default_value As T = Nothing) As Boolean
        If d Is Nothing Then
            Return False
        Else
            Return sync_async(Function(ByRef r As T) As Boolean
                                  r = d()
                                  Return True
                              End Function,
                              p,
                              ec,
                              default_value)
        End If
    End Function

    Public Function sync_async(Of T)(ByVal d As void(Of T),
                                     ByVal p As pointer(Of T),
                                     ByRef ec As event_comb,
                                     Optional ByVal default_value As T = Nothing) As Boolean
        If d Is Nothing Then
            Return False
        Else
            Return sync_async(Function(ByRef r As T) As Boolean
                                  d(r)
                                  Return True
                              End Function,
                              p,
                              ec,
                              default_value)
        End If
    End Function

    Private Function sync_async(ByVal c As _do(Of event_comb, Boolean)) As event_comb
        Dim r As event_comb = Nothing
        assert(c(r))
        Return r
    End Function

    Public Function sync_async(ByVal d As Func(Of Boolean)) As event_comb
        Return sync_async(Function(ByRef r) sync_async(d, r))
    End Function

    Public Function sync_async(Of T)(ByVal d As _do(Of T, Boolean),
                                     ByVal p As pointer(Of T),
                                     Optional ByVal default_value As T = Nothing) As event_comb
        Return sync_async(Function(ByRef r) sync_async(d, p, r, default_value))
    End Function

    Public Function sync_async(ByVal d As Action) As event_comb
        Return sync_async(Function(ByRef r) sync_async(d, r))
    End Function

    Public Function sync_async(Of T)(ByVal d As Func(Of T),
                                     ByVal p As pointer(Of T),
                                     Optional ByVal default_value As T = Nothing) As event_comb
        Return sync_async(Function(ByRef r) sync_async(d, p, r, default_value))
    End Function

    Public Function sync_async(Of T)(ByVal d As void(Of T),
                                     ByVal p As pointer(Of T),
                                     Optional ByVal default_value As T = Nothing) As event_comb
        Return sync_async(Function(ByRef r) sync_async(d, p, r, default_value))
    End Function
End Module
