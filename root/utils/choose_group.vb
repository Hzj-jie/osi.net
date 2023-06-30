
Imports osi.root.connector
Imports osi.root.formation

Public Module _choose_group
    Public Function choose_group(Of T)(ByVal a() As T, ByVal count As Int32) As vector(Of T())
        Dim r As vector(Of T()) = Nothing
        r = New vector(Of T())()
        If choose(Sub(s() As T)
                      r.emplace_back(s)
                  End Sub,
                  a,
                  count) Then
            Return r
        Else
            Return Nothing
        End If
    End Function
End Module
