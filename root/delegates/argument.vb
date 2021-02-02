
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class argument(Of T)
    Private ReadOnly v As T

    ' Use reflection to create the element, the constructor accepts only object.
    Public Sub New(ByVal v As Object)
        ' A shortcut for most of the cases.
        If v Is Nothing Then
            Me.v = Nothing
        Else
            Me.v = DirectCast(v, T)
        End If
    End Sub

    Public Shared Operator +(ByVal this As argument(Of T)) As T
        If this Is Nothing Then
            Console.WriteLine("argument has not been initialized, " +
                              "did you forget to call global_init.execute(arguments) or " +
                              "access an argument before arguments?")
            Console.WriteLine(New Exception().StackTrace())
            Environment.Exit(0)
        End If

        Return this.v
    End Operator
End Class
