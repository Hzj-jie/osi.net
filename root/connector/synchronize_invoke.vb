
Imports System.ComponentModel
Imports System.Threading

Public MustInherit Class synchronize_invoke
    Implements ISynchronizeInvoke

    Private Class async_result
        Implements IAsyncResult

        Private ReadOnly method As [Delegate]
        Private ReadOnly args() As Object
        Private ReadOnly state As Object
        Private ReadOnly mre As ManualResetEvent
        Private r As Object

        Public Sub New(ByVal method As [Delegate], ByVal args() As Object)
            Me.New(method, args, Nothing)
        End Sub

        Public Sub New(ByVal method As [Delegate], ByVal args() As Object, ByVal state As Object)
            assert(Not method Is Nothing)
            Me.method = method
            Me.args = args
            Me.state = state
            mre = New ManualResetEvent(False)
        End Sub

        Public Sub execute()
            r = do_(AddressOf method.DynamicInvoke, args, Nothing)
            assert(mre.force_set())
        End Sub

        Public Function result() As Object
            Return r
        End Function

        Public ReadOnly Property AsyncState() As Object Implements IAsyncResult.AsyncState
            Get
                Return state
            End Get
        End Property

        Public ReadOnly Property AsyncWaitHandle() As WaitHandle Implements IAsyncResult.AsyncWaitHandle
            Get
                Return mre
            End Get
        End Property

        Public ReadOnly Property CompletedSynchronously() As Boolean Implements IAsyncResult.CompletedSynchronously
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property IsCompleted() As Boolean Implements IAsyncResult.IsCompleted
            Get
                Return mre.wait(0)
            End Get
        End Property

        Protected Overrides Sub Finalize()
            mre.Close()
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
            Dim r As async_result = Nothing
            r = New async_result(method, args)
            push(AddressOf r.execute)
            Return r
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
End Class
