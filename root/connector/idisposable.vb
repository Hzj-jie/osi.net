
Imports System.Runtime.CompilerServices

Public Class empty_idisposable
    Implements IDisposable

    Public Sub Dispose() Implements IDisposable.Dispose
    End Sub
End Class

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
