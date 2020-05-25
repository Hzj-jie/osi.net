
Option Explicit On
Option Infer Off
Option Strict On

'do not suggest to use these two classes, use disposer<> instead

Imports osi.root.connector

Public Class event_disposer(Of T)
    Inherits pointer(Of T)
    Implements IComparable(Of event_disposer(Of T)), IDisposable

    Public Event disposing()

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal i As T)
        MyBase.New(i)
    End Sub

    Public Sub New(ByVal i As pointer(Of T))
        MyBase.New(i)
    End Sub

    Public Overloads Function CompareTo(ByVal other As event_disposer(Of T)) As Int32 _
                                       Implements IComparable(Of event_disposer(Of T)).CompareTo
        Return MyBase.CompareTo(other)
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                RaiseEvent disposing()
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

Public Class weak_event_disposer(Of T)
    Inherits weak_pointer(Of T)
    Implements IComparable(Of event_disposer(Of T)), IDisposable

    Public Event disposing()

    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(ByVal i As T)
        MyBase.New(i)
    End Sub

    Public Sub New(ByVal i As weak_pointer(Of T))
        MyBase.New(i)
    End Sub

    Public Overloads Function CompareTo(ByVal other As event_disposer(Of T)) As Int32 _
                                       Implements IComparable(Of event_disposer(Of T)).CompareTo
        Return MyBase.CompareTo(other)
    End Function

#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                RaiseEvent disposing()
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
