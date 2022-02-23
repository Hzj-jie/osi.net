
Option Explicit On
Option Infer Off
Option Strict On

Imports System.ComponentModel
Imports System.Threading

Public MustInherit Class synchronize_invoke
    Implements ISynchronizeInvoke

    Private NotInheritable Class async_result
        Implements IAsyncResult, IDisposable

        Private ReadOnly method As [Delegate]
        Private ReadOnly args() As Object
        Private ReadOnly sync As Boolean
        Private ReadOnly mre As ManualResetEvent
        Private r As Object

        Public Sub New(ByVal method As [Delegate], ByVal args() As Object, ByVal sync As Boolean)
            assert(method IsNot Nothing)
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
            r = method.safe_invoke(args)
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
                End If
                Return mre
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

        Public Sub Dispose() Implements IDisposable.Dispose
            If Not sync Then
                mre.Close()
                mre.Dispose()
            End If
            GC.SuppressFinalize(Me)
        End Sub

        Protected Overrides Sub Finalize()
            Dispose()
            GC.KeepAlive(Me)
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
        End If
        If synchronously() Then
            Return New async_result(method, args, True)
        End If
        Dim r As async_result = Nothing
        r = New async_result(method, args, False)
        push(AddressOf r.execute)
        Return r
    End Function

    Public Function EndInvoke(ByVal result As IAsyncResult) As Object Implements ISynchronizeInvoke.EndInvoke
        If result Is Nothing Then
            Return Nothing
        End If
        Dim r As async_result = Nothing
        r = direct_cast(Of async_result)(result)
        assert(r IsNot Nothing)
        assert(r.AsyncWaitHandle().wait())
        r.AsyncWaitHandle().Dispose()
        Return r.result()
    End Function

    Public Function Invoke(ByVal method As [Delegate],
                           ByVal args() As Object) As Object Implements ISynchronizeInvoke.Invoke
        Return EndInvoke(BeginInvoke(method, args))
    End Function

    Public Function async_invoke(ByVal method As [Delegate], ByVal args() As Object) As Boolean
        If method Is Nothing Then
            Return False
        End If
        If synchronously() Then
            method.safe_invoke(args)
        Else
            push(Sub()
                     ' The underlying runner should take care of the exceptions.
                     method.DynamicInvoke(args)
                 End Sub)
        End If
        Return True
    End Function

    Protected MustOverride Sub push(ByVal v As Action)

    Protected Overridable Function synchronously() As Boolean
        Return False
    End Function
End Class
