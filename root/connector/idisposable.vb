
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public NotInheritable Class empty_idisposable
    Implements IDisposable

    Public Shared ReadOnly instance As empty_idisposable = New empty_idisposable()

    Public Sub Dispose() Implements IDisposable.Dispose
    End Sub

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class disposables
    Public Shared Function [of](ByVal ParamArray ds() As IDisposable) As IDisposable
        assert(Not ds.null_or_empty())
        Return defer.to(Sub()
                            For Each d As IDisposable In ds
                                d.Dispose()
                            Next
                        End Sub)
    End Function

    Private Sub New()
    End Sub
End Class

Public Module _idisposable
    <Extension()> Public Function not_null_and_dispose(ByVal i As IDisposable) As Boolean
        If i Is Nothing Then
            Return False
        End If
        Try
            i.Dispose()
        Catch ex As ObjectDisposedException
            ' Ignore.
        End Try
        Return True
    End Function
End Module
