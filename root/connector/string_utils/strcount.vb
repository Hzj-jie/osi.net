
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _strcount
    <Extension()> Public Function strchrcount(ByVal s As String, ByVal c As Char) As Int64
        Dim i As Int64 = 0
        Dim count As Int64 = 0
        count = -1
        i = -1
        Do
            count += 1
            i = s.IndexOf(c, Convert.ToInt32(i + 1))
        Loop Until i = npos

        Return count
    End Function
End Module
