
Imports osi.root.constants

Public Module _choose
    Public Function choose(ByVal count As Int32, ByVal from As Int32) As Int32
        If from < count OrElse count < 0 OrElse from < 0 Then
            Return npos
        ElseIf count = 0 OrElse count = from Then
            Return 1
        Else
            If count > (from >> 1) Then
                count = from - count
            End If
            Dim r As Int32 = 0
            r = 1
            For i As Int32 = 0 To count - 1
                r *= from - i
            Next
            For i As Int32 = 2 To count
                r \= i
            Next
            Return r
        End If
    End Function

    Private Function choose(Of T)(ByVal d As Func(Of T(), Boolean),
                                  ByVal selected() As T,
                                  ByVal index As Int32,
                                  ByVal last As Int32,
                                  ByVal a() As T,
                                  ByVal count As Int32) As Boolean
        assert(Not d Is Nothing)
        assert(array_size(selected) = count)
        assert(index <= count)
        assert(last <= array_size(a))
        assert(Not isemptyarray(a))
        If index = count Then
            Return d(selected)
        Else
            For i As Int32 = last To array_size(a) - 1
                selected(index) = a(i)
                If Not choose(d, selected, index + 1, i + 1, a, count) Then
                    Return False
                End If
            Next
            Return True
        End If
    End Function

    Public Function choose(Of T)(ByVal d As Func(Of T(), Boolean),
                                 ByVal a() As T,
                                 ByVal count As Int32) As Boolean
        If d Is Nothing OrElse array_size(a) < count OrElse count < 0 Then
            Return False
        ElseIf count = 0 Then
            Return d(New T() {})
        ElseIf array_size(a) = count Then
            Return d(a)
        Else
            Dim selected() As T = Nothing
            ReDim selected(count - 1)
            Return choose(d, selected, 0, 0, a, count)
        End If
    End Function

    Public Function choose(Of T)(ByVal d As Action(Of T()),
                                 ByVal a() As T,
                                 ByVal count As Int32) As Boolean
        If d Is Nothing Then
            Return False
        Else
            Return choose(Function(s() As T) As Boolean
                              d(s)
                              Return True
                          End Function,
                          a,
                          count)
        End If
    End Function
End Module
