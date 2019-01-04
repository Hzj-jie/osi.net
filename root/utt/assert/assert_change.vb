
Option Explicit On
Option Infer Off
Option Strict On

' TODO: Move into check
Public Class assert_equal(Of T)
    Inherits assert_change(Of T)

    Public Sub New(ByVal f As Func(Of T), ByVal ParamArray msg() As Object)
        MyBase.New(f, msg)
    End Sub

    Protected Overrides Sub assert_result(b As T, e As T, ByVal ParamArray msg() As Object)
        assertion.equal(b, e, msg)
    End Sub
End Class

Public Class assert_more(Of T)
    Inherits assert_change(Of T)

    Public Sub New(ByVal f As Func(Of T), ByVal ParamArray msg() As Object)
        MyBase.New(f, msg)
    End Sub

    Protected Overrides Sub assert_result(b As T, e As T, ByVal ParamArray msg() As Object)
        assertion.more(b, e, msg)
    End Sub
End Class

Public Class assert_less(Of T)
    Inherits assert_change(Of T)

    Public Sub New(ByVal f As Func(Of T), ByVal ParamArray msg() As Object)
        MyBase.New(f, msg)
    End Sub

    Protected Overrides Sub assert_result(b As T, e As T, ByVal ParamArray msg() As Object)
        assertion.less(b, e, msg)
    End Sub
End Class

Public MustInherit Class assert_change(Of T)
    Implements IDisposable

    Private ReadOnly f As Func(Of T)
    Private ReadOnly b As T
    Private ReadOnly msg() As Object

    Protected Sub New(ByVal f As Func(Of T), ByVal ParamArray msg() As Object)
        Me.f = f
        Me.b = f()
        Me.msg = msg
    End Sub

    Protected MustOverride Sub assert_result(ByVal b As T, ByVal e As T, ByVal ParamArray msg() As Object)

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                assert_result(b, f(), msg)
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
