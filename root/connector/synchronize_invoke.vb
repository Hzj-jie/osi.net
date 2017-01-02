
Imports System.ComponentModel
Imports System.Threading

Public MustInherit Class synchronize_invoke
    Implements ISynchronizeInvoke

    Private Class async_result
        Implements IAsyncResult

        Private ReadOnly method As [Delegate]
        Private ReadOnly args() As Object
        Private ReadOnly sync As Boolean
        Private ReadOnly mre As ManualResetEvent
        Private r As Object

        Public Sub New(ByVal method As [Delegate], ByVal args() As Object, ByVal sync As Boolean)
            assert(Not method Is Nothing)
            Me.method = method
            Me.args = args
            Me.sync = sync
            If Not Me.sync Then
                mre = New ManualResetEvent(False)
            Else
                execute()
            End If
        End Sub

        Public Sub execute()
            r = do_(AddressOf method.DynamicInvoke, args, Nothing)
            If Not sync Then
                assert(mre.force_set())
            End If
        End Sub

        Public Function result() As Object
            Return r
        End Function

        Public ReadOnly Property AsyncState() As Object Implements IAsyncResult.AsyncState
            Get
                Return Nothing
            End Get
        End Property

        Public ReadOnly Property AsyncWaitHandle() As WaitHandle Implements IAsyncResult.AsyncWaitHandle
            Get
                If sync Then
                    Return null_wait_handle.instance
                Else
                    Return mre
                End If
            End Get
        End Property

        Public ReadOnly Property CompletedSynchronously() As Boolean Implements IAsyncResult.CompletedSynchronously
            Get
                Return sync
            End Get
        End Property

        Public ReadOnly Property IsCompleted() As Boolean Implements IAsyncResult.IsCompleted
            Get
                Return sync OrElse mre.wait(0)
            End Get
        End Property

        Protected Overrides Sub Finalize()
            If Not sync Then
                mre.Close()
            End If
            MyBase.Finalize()
        End Sub
    End Class

    Public ReadOnly Property InvokeRequired() As Boolean Implements ISynchronizeInvoke.InvokeRequired
        Get
            Return False
        End Get
    End Property

    Public Function BeginInvoke(ByVal method As [Delegate],
                                ByVal args() As Object) As IAsyncResult Implements ISynchronizeInvoke.BeginInvoke
        If method Is Nothing Then
            Return Nothing
        Else
            If synchronously() Then
                Return New async_result(method, args, True)
            Else
                Dim r As async_result = Nothing
                r = New async_result(method, args, False)
                push(AddressOf r.execute)
                Return r
            End If
        End If
    End Function

    Public Function EndInvoke(ByVal result As IAsyncResult) As Object Implements ISynchronizeInvoke.EndInvoke
        If result Is Nothing Then
            Return Nothing
        Else
            Dim r As async_result = Nothing
            r = direct_cast(Of async_result)(result)
            assert(Not r Is Nothing)
            assert(r.AsyncWaitHandle().wait())
            Return r.result()
        End If
    End Function

    Public Function Invoke(ByVal method As [Delegate],
                           ByVal args() As Object) As Object Implements ISynchronizeInvoke.Invoke
        Return EndInvoke(BeginInvoke(method, args))
    End Function

    Protected MustOverride Sub push(ByVal v As Action)

    Protected Overridable Function synchronously() As Boolean
        Return False
    End Function
End Class
