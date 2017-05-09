
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with weak_dispose_ptr.vbp ----------
'so change weak_dispose_ptr.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with dispose_ptr.vbp ----------
'so change dispose_ptr.vbp instead of this file




'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with disposer_imports.vbp ----------
'so change disposer_imports.vbp instead of this file


Imports osi.root.connector
Imports osi.root.lock
'finish disposer_imports.vbp --------

Public Class weak_dispose_ptr(Of T)
    Inherits weak_pointer(Of T)
    
    Private ReadOnly _disposer As Action(Of T)

#If 0 Then
    Public Sub New(ByVal p As Func(Of T),
                   Optional ByVal init As Action = Nothing,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        If Not init Is Nothing Then
            init()
        End If
        If Not p Is Nothing Then
            [set](p())
        End If
        If disposer Is Nothing Then
            assert(Not [default](Of T).disposer() Is Nothing)
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
        assert(Not Me._disposer Is Nothing)
    End Sub

    Public Sub New(Optional ByVal disposer As Action(Of T) = Nothing)
        Me.New(Nothing, disposer)
    End Sub

    Protected Sub dispose(ByVal c As T)
        _disposer(c)
    End Sub

    Private Sub do_dispose()

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with weak_dispose_ptr_do_dispose.vbp ----------
'so change weak_dispose_ptr_do_dispose.vbp instead of this file

		Dim v As T = Nothing
		If [get](v) Then
			dispose(v)
		End If
'finish weak_dispose_ptr_do_dispose.vbp --------
    End Sub


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with disposer_core.vbp ----------
'so change disposer_core.vbp instead of this file



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

    Public Sub queue_dispose()
        queue_in_managed_threadpool(AddressOf mark_and_dispose)
#If False Then
        GC.SuppressFinalize(Me)
#End If
    End Sub

#If False Then

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with disposer_dispose.vbp ----------
'so change disposer_dispose.vbp instead of this file


    Public Sub IDisposable_Dispose() Implements IDisposable.Dispose
        dispose()
    End Sub

    Protected NotOverridable Overrides Sub Finalize()
        dispose()
        MyBase.Finalize()
    End Sub
'finish disposer_dispose.vbp --------
#End If
'finish disposer_core.vbp --------
End Class
'finish dispose_ptr.vbp --------
'finish weak_dispose_ptr.vbp --------
