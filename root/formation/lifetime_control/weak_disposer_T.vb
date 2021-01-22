
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with weak_disposer_T.vbp ----------
'so change weak_disposer_T.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with disposer_T.vbp ----------
'so change disposer_T.vbp instead of this file



Public Module _weak_disposer
#If 0 Then
    Public Function make_weak_disposer(Of T) _
                        (ByVal p As Func(Of T),
                         Optional ByVal init As Action = Nothing,
                         Optional ByVal disposer As Action(Of T) = Nothing) _
                        As weak_disposer(Of T)
        Return New weak_disposer(Of T)(p, init, disposer)
    End Function

    Public Function make_weak_disposer(Of T) _
                        (ByVal p As T,
                         Optional ByVal init As Action = Nothing,
                         Optional ByVal disposer As Action(Of T) = Nothing) _
                        As weak_disposer(Of T)
        Return New weak_disposer(Of T)(p, init, disposer)
    End Function
#End If

    Public Function make_weak_disposer(Of T) _
                        (ByVal p As T,
                         Optional ByVal disposer As Action(Of T) = Nothing) _
                        As weak_disposer(Of T)
        Return New weak_disposer(Of T)(p, disposer)
    End Function
End Module

<Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
Public Class weak_disposer(Of T)
    Inherits weak_dispose_ptr(Of T)
    Implements IDisposable

#If 0 Then
    Public Sub New(ByVal p As Func(Of T),
                   Optional ByVal init As Action = Nothing,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        MyBase.New(p, init, disposer)
    End Sub

    Public Sub New(ByVal p As T,
                   Optional ByVal init As Action = Nothing,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        MyBase.New(p, init, disposer)
    End Sub

    Public Sub New(Optional ByVal init As Action = Nothing,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        MyBase.New(init, disposer)
    End Sub
#End If

    Public Sub New(ByVal p As T,
                   Optional ByVal disposer As Action(Of T) = Nothing)
        MyBase.New(p, disposer)
    End Sub

    Public Sub New(Optional ByVal disposer As Action(Of T) = Nothing)
        MyBase.New(disposer)
    End Sub

    Public Overloads Sub dispose()
        MyBase.dispose()
        GC.SuppressFinalize(Me)
    End Sub


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with disposer_dispose.vbp ----------
'so change disposer_dispose.vbp instead of this file



#If True Then
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
    Public Sub IDisposable_Dispose() Implements IDisposable.Dispose
        dispose()
    End Sub
#End If

#If True Then
    <Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")>
    Protected NotOverridable Overrides Sub Finalize()
        dispose()
        GC.KeepAlive(Me)
        MyBase.Finalize()
    End Sub
#End If
'finish disposer_dispose.vbp --------
End Class
'finish disposer_T.vbp --------
'finish weak_disposer_T.vbp --------
