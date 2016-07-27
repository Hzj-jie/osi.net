
Imports System.Runtime.CompilerServices

Public Module _map
    <Extension()> Public Function null_or_empty(Of KT, VT)(ByVal this As map(Of KT, VT)) As Boolean
        Return this Is Nothing OrElse this.empty()
    End Function
End Module
