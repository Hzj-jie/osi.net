
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with disposer_T.vbp ----------
'so change disposer_T.vbp instead of this file



Public Module _disposer
#If 0 Then
    Public Function make_disposer(Of T) _
					    (ByVal p As Func(Of T),
                         Optional ByVal init As Action = Nothing,
                         Optional ByVal disposer As Action(Of T) = Nothing) _
                        As disposer(Of T)
        Return New disposer(Of T)(p, init, disposer)
    End Function

    Public Function make_disposer(Of T) _
						(ByVal p As T,
                         Optional ByVal init As Action = Nothing,
                         Optional ByVal disposer As Action(Of T) = Nothing) _
                        As disposer(Of T)
        Return New disposer(Of T)(p, init, disposer)
    End Function
#End If

    Public Function make_disposer(Of T) _
						(ByVal p As T,
                         Optional ByVal disposer As Action(Of T) = Nothing) _
                        As disposer(Of T)
        Return New disposer(Of T)(p, disposer)
    End Function
End Module

Public Class disposer(Of T)
    Inherits dispose_ptr(Of T)
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

    Public Shadows Sub queue_dispose()
        MyBase.queue_dispose()
        GC.SuppressFinalize(Me)
    End Sub


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
End Class
'finish disposer_T.vbp --------
