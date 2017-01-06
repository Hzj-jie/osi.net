
Imports System.Runtime.CompilerServices

Public Module _map
    <Extension()> Public Function null_or_empty(Of KT, VT)(ByVal this As map(Of KT, VT)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function

    <Extension()> Public Function find(Of KT, VT) _
                                      (ByVal this As map(Of KT, VT), ByVal key As KT, ByRef o As VT) As Boolean
        If this Is Nothing Then
            Return False
        Else
            Dim it As map(Of KT, VT).iterator = Nothing
            it = this.find(key)
            If it = this.end() Then
                Return False
            Else
                o = (+it).second
                Return True
            End If
        End If
    End Function

    <Extension()> Public Function find_or_default(Of KT, VT) _
                                                 (ByVal this As map(Of KT, VT),
                                                  ByVal key As KT,
                                                  ByRef o As VT,
                                                  Optional ByVal default_value As VT = Nothing) As Boolean
        If find(this, key, o) Then
            Return True
        Else
            o = default_value
            Return False
        End If
    End Function
End Module
