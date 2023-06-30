
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class defer
    Private Structure deferrer
        Implements IDisposable

        Private ReadOnly a As Action

        Public Sub New(ByVal a As Action)
            assert(Not a Is Nothing)
            Me.a = a
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            a()
        End Sub
    End Structure

    ' This is an efficient implementation of util._disposer.defer. It does not use singleentry or any kind of race
    ' condition protection, as it should not be the common user scenario.
    Public Shared Function [to](ByVal a As Action) As IDisposable
        Return New deferrer(a)
    End Function

    Private Sub New()
    End Sub
End Class
