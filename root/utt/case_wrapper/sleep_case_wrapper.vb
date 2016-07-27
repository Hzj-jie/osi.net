
Imports osi.root.connector
Imports osi.root.utils

Public Class sleep_case_wrapper
    Inherits case_wrapper

    Private ReadOnly ms As Int64
    Private ReadOnly d As Func(Of Int64)
    Private ReadOnly before As Boolean
    Private ReadOnly after As Boolean

    Private Sub New(ByVal c As [case],
                    ByVal ms As Int64,
                    ByVal d As Func(Of Int64),
                    ByVal before As Boolean,
                    ByVal after As Boolean)
        MyBase.New(c)
        Me.ms = ms
        Me.d = d
        Me.before = before
        Me.after = after
        assert(before OrElse after)
    End Sub

    Public Sub New(ByVal c As [case], ByVal ms As Int64)
        Me.New(c, ms, Nothing, False, True)
    End Sub

    Public Sub New(ByVal ms As Int64, ByVal c As [case])
        Me.New(c, ms, Nothing, True, False)
    End Sub

    Public Sub New(ByVal c As [case], ByVal ms As Func(Of Int64))
        Me.New(c, 0, ms, False, True)
    End Sub

    Public Sub New(ByVal ms As Func(Of Int64), ByVal c As [case])
        Me.New(c, 0, ms, True, False)
    End Sub

    Public Sub New(ByVal c As [case], ByVal ms As Int64, ByVal before_and_after As Boolean)
        Me.New(c, ms, Nothing, before_and_after, True)
    End Sub

    Public Sub New(ByVal ms As Int64, ByVal c As [case], ByVal before_and_after As Boolean)
        Me.New(c, ms, Nothing, True, before_and_after)
    End Sub

    Private Sub s()
        If d Is Nothing Then
            sleep(ms)
        Else
            sleep(d())
        End If
    End Sub

    Public NotOverridable Overrides Function run() As Boolean
        If before Then
            s()
        End If
        Dim r As Boolean = False
        r = MyBase.run()
        If after Then
            s()
        End If
        Return r
    End Function
End Class
