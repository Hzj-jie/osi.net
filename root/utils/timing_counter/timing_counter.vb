
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Public MustInherit Class timing_counter
    Implements IDisposable

    Private ReadOnly p As ref(Of Int64)
    Private ReadOnly s As Int64

    Public Sub New(ByVal p As ref(Of Int64))
        Me.p = p
        s = now()
    End Sub

    Protected MustOverride Function now() As Int64

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                eva(p, now() - s)
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
