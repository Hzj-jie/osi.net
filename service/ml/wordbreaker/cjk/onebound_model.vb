
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.service.ml.onebound(Of String)

Public NotInheritable Class onebound_model
    Private Shared input As argument(Of String)
    Private Shared lower_bound As argument(Of Double)
    Private Shared ratio As argument(Of Double)

    Public Shared Function pure_words() As unordered_map(Of String, Double)
        Dim m As unordered_map(Of String, Double) =
                model.load(+input).
                      filter(lower_bound Or 0).
                      flat_map().
                      map(Function(ByVal p As first_const_pair(Of const_pair(Of String, String), Double)) _
                              As first_const_pair(Of String, Double)
                              Return first_const_pair.emplace_of(strcat(p.first.first, p.first.second), p.second)
                          End Function).
                      collect_to(Of unordered_map(Of String, Double))()
        Dim it As unordered_map(Of String, Double).iterator = m.begin()
        While it <> m.end()
            Dim s As String = (+it).first
            If s.strlen() > 2 Then
                Dim comparison As Action(Of unordered_map(Of String, Double).iterator) =
                        Sub(ByVal it2 As unordered_map(Of String, Double).iterator)
                            If it2 = m.end() Then
                                Return
                            End If
                            If (+it2).second > (+it).second * (ratio Or 1) Then
                                it.value().second = 0
                            ElseIf (+it).second > (+it2).second * (ratio Or 1) Then
                                it2.value().second = 0
                            End If
                        End Sub
                comparison(m.find(s.strleft(s.strlen() - uint32_1)))
                comparison(m.find(s.strmid(uint32_1)))
            End If
            it += 1
        End While

        it = m.begin()
        While it <> m.end()
            If (+it).second = 0 Then
                it = m.erase(it)
            Else
                it += 1
            End If
        End While

        Return m
    End Function

    Private Sub New()
    End Sub
End Class
