
Option Explicit On
Option Infer Off
Option Strict On

' A simple optional.
Public NotInheritable Class argument(Of T)
    Private ReadOnly defined As Boolean
    Private ReadOnly v As T

    ' Use reflection to create the element, the constructor accepts only object.
    Private Sub New(ByVal v As Object)
        ' A shortcut for most of the cases.
        If v Is Nothing Then
            Me.defined = False
            Me.v = Nothing
        Else
            Me.defined = True
            Me.v = DirectCast(v, T)
        End If
    End Sub

    Private Shared Sub assert_instance(ByVal this As argument(Of T))
        If this Is Nothing Then
            Console.WriteLine("argument has not been initialized, " +
                              "did you forget to call global_init.execute(arguments) or " +
                              "access an argument before arguments?")
            Console.WriteLine(New Exception().StackTrace())
            Environment.Exit(1)
        End If
    End Sub

    Public Shared Operator -(ByVal this As argument(Of T)) As Boolean
        assert_instance(this)
        Return this.defined
    End Operator

    Public Shared Operator +(ByVal this As argument(Of T)) As T
        assert_instance(this)
        Return this.v
    End Operator

    Public Shared Operator Or(ByVal this As argument(Of T), ByVal default_value As T) As T
        assert_instance(this)
        If this.defined Then
            Return this.v
        End If
        Return default_value
    End Operator
End Class
