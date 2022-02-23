
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.delegates
Imports osi.root.lock

Public NotInheritable Class callback_action_async_operation
    Inherits callback_action
    Private _ao As async_operation = Nothing

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Private Sub New(ByVal begin As Func(Of Boolean),
                    ByVal end_rtn As Func(Of Boolean),
                    ByVal timeoutticks As Int64,
                    ByVal additional_jump As Int32)
        MyBase.New(begin, check_async_operation(), end_async_operation(end_rtn), timeoutticks, additional_jump + 1)
        timeouted = Sub()
                        wait_when(Function() _ao Is Nothing)
                        ao().force_finish()
                    End Sub
        resetting = Sub()
                        clear()
                    End Sub
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Private Sub New(ByVal begin As _do(Of AsyncCallback, Object, IAsyncResult),
                    ByVal end_d As void(Of IAsyncResult),
                    ByVal end_rtn As Func(Of Boolean),
                    ByVal timeoutticks As Int64,
                    ByVal additional_jump As Int32)
        Me.New(begin_async_operation(begin, end_d), end_rtn, timeoutticks, additional_jump + 1)
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Sub New(ByVal begin As _do(Of AsyncCallback, Object, IAsyncResult),
                   ByVal end_d As void(Of IAsyncResult),
                   Optional ByVal end_rtn As Func(Of Boolean) = Nothing,
                   Optional ByVal timeoutticks As Int64 = npos)
        Me.New(begin, end_d, end_rtn, timeoutticks, 1)
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Sub New(ByVal begin As _do(Of AsyncCallback, Object, IAsyncResult),
                   ByVal end_d As void(Of IAsyncResult),
                   ByVal timeoutticks As Int64)
        Me.New(begin, end_d, Nothing, timeoutticks, 1)
    End Sub

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Private Overloads Shared Function ctor(Of T)(ByVal begin As _do(Of AsyncCallback, Object, IAsyncResult),
                                                 ByVal end_d As _do(Of IAsyncResult, T),
                                                 ByVal end_rtn As Func(Of Boolean),
                                                 ByVal result As ref(Of T),
                                                 ByVal timeoutticks As Int64,
                                                 ByVal additional_jump As Int32) As callback_action
        Return New callback_action_async_operation(begin_async_operation(begin, end_d, result),
                                                   end_rtn,
                                                   timeoutticks,
                                                   additional_jump + 1)
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Overloads Shared Function ctor(Of T)(ByVal begin As _do(Of AsyncCallback, Object, IAsyncResult),
                                                ByVal end_d As _do(Of IAsyncResult, T),
                                                ByVal end_rtn As Func(Of Boolean),
                                                Optional ByVal result As ref(Of T) = Nothing,
                                                Optional ByVal timeoutticks As Int64 = npos) As callback_action
        Return ctor(begin, end_d, end_rtn, result, timeoutticks, 1)
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Overloads Shared Function ctor(Of T)(ByVal begin As _do(Of AsyncCallback, Object, IAsyncResult),
                                                ByVal end_d As _do(Of IAsyncResult, T),
                                                Optional ByVal result As ref(Of T) = Nothing,
                                                Optional ByVal timeoutticks As Int64 = npos) As callback_action
        Return ctor(begin, end_d, Nothing, result, timeoutticks, 1)
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Overloads Shared Function ctor(Of T)(ByVal begin As _do(Of AsyncCallback, Object, IAsyncResult),
                                                ByVal end_d As _do(Of IAsyncResult, T),
                                                ByVal timeoutticks As Int64) As callback_action
        Return ctor(begin, end_d, Nothing, Nothing, timeoutticks, 1)
    End Function

    <Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.NoInlining)>
    Public Overloads Shared Function ctor(ByVal begin As _do(Of AsyncCallback, Object, IAsyncResult),
                                          ByVal end_d As void(Of IAsyncResult),
                                          Optional ByVal end_rtn As Func(Of Boolean) = Nothing,
                                          Optional ByVal timeoutticks As Int64 = npos) As callback_action
        Return New callback_action_async_operation(begin, end_d, end_rtn, timeoutticks, 1)
    End Function

    Private Function ao() As async_operation
        assert(_ao IsNot Nothing)
        Return _ao
    End Function

    Private Sub clear()
        _ao = Nothing
    End Sub

    Private Sub _assert()
        assert(_ao Is Nothing)
    End Sub

    Private Shared Shadows Property current() As callback_action_async_operation
        Get
            Return cast(Of callback_action_async_operation)(callback_action.current())
        End Get
        Set(ByVal value As callback_action_async_operation)
            callback_action.current() = value
        End Set
    End Property

    Private Shared Function begin_async_operation(ByVal begin As _do(Of AsyncCallback,
                                                                        Object,
                                                                        IAsyncResult),
                                                  ByVal end_d As void(Of IAsyncResult)) As Func(Of Boolean)
        If end_d Is Nothing Then
            Return todo(True)
        End If
        Return begin_async_operation(begin,
                                   async_operation.void_to_do(end_d),
                                   Nothing)
    End Function

    Private Shared Function begin_async_operation(Of T)(ByVal begin As _do(Of AsyncCallback,
                                                                              Object,
                                                                              IAsyncResult),
                                                        ByVal end_d As _do(Of IAsyncResult, T),
                                                        ByVal result As ref(Of T)) As Func(Of Boolean)
        If begin Is Nothing OrElse end_d Is Nothing Then
            Return todo(True)
        End If
        Return Function() As Boolean
                   Dim this As callback_action_async_operation = Nothing
                   this = current()
                   this._assert()
                   this._ao = async_operation.ctor(
                                      begin,
                                      Function(ByRef a)
                                          Dim rtn As T = Nothing
                                          current() = this
                                          Try
                                              rtn = end_d(a)
                                          Finally
                                              current() = Nothing
                                          End Try
                                          Return rtn
                                      End Function,
                                      Sub(ByRef suc As Boolean)
                                          'end_ticks is set right before end_async_operation called.
                                          If this.end_ticks() = npos Then
                                              this.trigger_check()
                                          Else
                                              raise_error(this.callstack(), ":begin finished already.")
                                          End If
                                      End Sub,
                                      result,
                                      this.callstack())
                   Return this.ao().begin_succeeded()
               End Function
    End Function

    Private Shared Function check_async_operation() As Func(Of Boolean)
        Return Function() As Boolean
                   Return true_to_pass(current().ao().finished())
               End Function
    End Function

    Private Shared Function end_async_operation(ByVal rtn As Func(Of Boolean)) As Func(Of Boolean)
        Return Function() As Boolean
                   Dim this As callback_action_async_operation = Nothing
                   this = current()
                   Dim r As Boolean = False
                   If Not this.ao().begin_succeeded() OrElse Not this.ao().finished() Then
                       this.ao().force_finish()
                       r = False
                   ElseIf rtn Is Nothing Then
                       r = True
                   Else
                       'if ThreadAbortException raised, ignore the lock.release, since the service is shuting down.
                       r = do_(rtn, False)
                   End If
                   this.clear()
                   Return r
               End Function
    End Function
End Class
