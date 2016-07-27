
Imports System.Runtime.CompilerServices
Imports System.Collections

Public Module _collections
    <Extension()> Public Function null_or_empty(ByVal i As ICollection) As Boolean
        Return i Is Nothing OrElse i.Count() = 0
    End Function
End Module
