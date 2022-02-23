
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with dispose_ptr.vbp ----------
'so change dispose_ptr.vbp instead of this file




'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with disposer_imports.vbp ----------
'so change disposer_imports.vbp instead of this file


Imports osi.root.connector
Imports osi.root.lock
'finish disposer_imports.vbp --------

Public Class dispose_ptr(Of T)
    Inherits ref(Of T)

    Private ReadOnly _disposer As Action(Of T)

#If 0 Then
    Public Sub New(ByVal p As Func(Of T),
                   Optional ByVal init As Action = Nothing,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        If init IsNot Nothing Then
            init()
        End If
        If p IsNot Nothing Then
            [set](p())
        End If
        If disposer Is Nothing Then
            assert([default](Of T).disposer() IsNot Nothing)
            Me._disposer = [default](Of T).disposer()
        Else
            Me._disposer = disposer
        End If
    End Sub

    Public Sub New(ByVal p As T,
                   Optional ByVal init As Action = Nothing,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        Me.New(Function() p, init, disposer)
    End Sub

    Public Sub New(Optional ByVal init As Action = Nothing,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        Me.New(DirectCast(Nothing, Func(Of T)), init, disposer)
    End Sub
#End If
    
    Public Sub New(ByVal p As T,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        [set](p)
        If disposer Is Nothing Then
            Me._disposer = disposable(Of T).D()
        Else
            Me._disposer = disposer
        End If
        assert(Me._disposer IsNot Nothing)
    End Sub

    Public Sub New(Optional ByVal disposer As Action(Of T) = Nothing)
        Me.New(Nothing, disposer)
    End Sub

    Protected Sub dispose(ByVal c As T)
        _disposer(c)
    End Sub

    Private Sub do_dispose()

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with dispose_ptr_do_dispose.vbp ----------
'so change dispose_ptr_do_dispose.vbp instead of this file


        If Not empty() Then
            dispose([get]())
        End If
'finish dispose_ptr_do_dispose.vbp --------
    End Sub


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with disposer_core.vbp ----------
'so change disposer_core.vbp instead of this file



    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")>
    Public Event dispose_exception(ByVal ex As Exception)
    Private exp As singleentry

    Private sub mark_and_dispose()
        If exp.mark_in_use() Then
            Try
                do_dispose()
            Catch ex As Exception
                If event_attached(dispose_exceptionEvent) Then
                    RaiseEvent dispose_exception(ex)
                Else
                    log_unhandled_exception(ex)
                End If
            End Try
        End If
        GC.KeepAlive(Me)
    End Sub

    Public Function disposed() As Boolean
        Return exp.in_use()
    End Function

    Public Sub dispose()
        mark_and_dispose()
#If False Then
        GC.SuppressFinalize(Me)
#End If
    End Sub


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with disposer_dispose.vbp ----------
'so change disposer_dispose.vbp instead of this file



#If False Then
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
    Public Sub IDisposable_Dispose() Implements IDisposable.Dispose
        dispose()
    End Sub
#End If

#If False Then
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
    Protected NotOverridable Overrides Sub Finalize()
        dispose()
        GC.KeepAlive(Me)
        MyBase.Finalize()
    End Sub
#End If
'finish disposer_dispose.vbp --------
'finish disposer_core.vbp --------
End Class
'finish dispose_ptr.vbp --------
