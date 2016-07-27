
Imports System.Runtime.CompilerServices

Public Module _idisposable
    <Extension()> Public Function not_null_and_dispose(ByVal i As IDisposable) As Boolean
        If i Is Nothing Then
            Return False
        Else
            i.Dispose()
            Return True
        End If
    End Function
End Module
