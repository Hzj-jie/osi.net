
Public Module _goto
    Public Function [goto](ByVal [step] As Int64) As Boolean
        Return event_comb.goto([step])
    End Function

    Public Function goback() As Boolean
        Return event_comb.goback()
    End Function

    Public Function goto_prev() As Boolean
        Return event_comb.goto_prev()
    End Function

    Public Function goto_end() As Boolean
        Return event_comb.goto_end()
    End Function

    Public Function goto_next() As Boolean
        Return event_comb.goto_next()
    End Function

    Public Function goto_last() As Boolean
        Return event_comb.goto_last()
    End Function

    Public Function goto_begin() As Boolean
        Return event_comb.goto_begin()
    End Function
End Module
