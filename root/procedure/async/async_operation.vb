
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utils
Imports error_type = osi.root.constants.error_type

Public Interface async_state_t
    Sub mark_finish(ByVal ar As IAsyncResult, ByVal set_result As Boolean)
    Function finished() As Boolean
End Interface

Public NotInheritable Class async_operation
    Private NotInheritable Class async_state_t(Of T)
        Implements async_state_t
        Public ReadOnly result As pointer(Of T) = Nothing
        Public ReadOnly callstack As String = Nothing
        Private ReadOnly [end] As _do(Of IAsyncResult, T) = Nothing
        Private ReadOnly after_set_result As void(Of Boolean) = Nothing
        Private _finished_called As singleentry
        Private _finished As Boolean = False

        Public Sub New(ByVal [end] As _do(Of IAsyncResult, T),
                       ByVal after_set_result As void(Of Boolean),
                       ByVal result As pointer(Of T),
                       ByVal callstack As String)
            assert(Not [end] Is Nothing)
            Me.end = [end]
            Me.after_set_result = after_set_result
            Me.result = result
            Me.callstack = callstack
        End Sub

        Public Sub mark_finish(ByVal ar As IAsyncResult,
                               ByVal set_result As Boolean) Implements async_state_t.mark_finish
            assert(Not ar Is Nothing)
            If _finished_called.mark_in_use() Then
                Dim rst As T = Nothing
                Dim end_result As Boolean = False
                Try
                    rst = [end](ar)
                    end_result = True
                Catch ex As Exception
                    raise_error(error_type.warning,
                                "cannot end async operation @",
                                callstack, ":end, ex ", ex.Message)
                    end_result = False
                Finally
                    If end_result AndAlso set_result Then
                        eva(result, rst)
                    End If
                    counter.decrease(ASYNC_OPERATION_COUNT)
                    void_(after_set_result, end_result AndAlso set_result)
                    _finished = True
                End Try
            End If
        End Sub

        Public Function finished() As Boolean Implements async_state_t.finished
            Return _finished
        End Function
    End Class

    Private Shared ReadOnly ASYNC_OPERATION_COUNT As Int64
    Private ar As IAsyncResult = Nothing
    Private state As async_state_t = Nothing
    Private callstack As String = Nothing

    Shared Sub New()
        ASYNC_OPERATION_COUNT = counter.register_counter("ASYNC_OPERATION_COUNT")
    End Sub

    Private Sub _new(Of T)(ByVal begin As _do(Of AsyncCallback, Object, IAsyncResult),
                           ByVal [end] As _do(Of IAsyncResult, T),
                           ByVal after_set_result As void(Of Boolean),
                           ByVal result As pointer(Of T),
                           Optional ByVal callstack As String = Nothing)
        assert(Not begin Is Nothing)
        Me.callstack = callstack
        state = New async_state_t(Of T)([end], after_set_result, result, callstack)
        Try
            ar = begin(Sub(a)
                           state.mark_finish(a, True)
                       End Sub,
                       Nothing)
        Catch ex As Exception
            raise_error(error_type.warning,
                        "cannot begin async operation @", callstack,
                        ":begin, ex ", ex.Message)
        End Try
        If begin_succeeded() Then
            counter.increase(ASYNC_OPERATION_COUNT)
        End If
    End Sub

    Private Sub New()
    End Sub

    Public Shared Function void_to_do(ByVal [end] As void(Of IAsyncResult)) As _do(Of IAsyncResult, Byte)
        Return Function(ByRef a As IAsyncResult) As Byte
                   [end](a)
                   Return 0
               End Function
    End Function

    Public Shared Function ctor(Of T)(ByVal begin As _do(Of AsyncCallback, Object, IAsyncResult),
                                      ByVal [end] As _do(Of IAsyncResult, T),
                                      ByVal after_set_result As void(Of Boolean),
                                      ByVal result As pointer(Of T),
                                      Optional ByVal callstack As String = Nothing) As async_operation
        Dim rtn As async_operation = Nothing
        rtn = New async_operation()
        rtn._new(begin, [end], after_set_result, result, callstack)
        Return rtn
    End Function

    Private Function wait_ar() As IAsyncResult
        assert(begin_succeeded())
        wait_when(Function() ar Is Nothing)
        Return ar
    End Function

    Public Function begin_succeeded() As Boolean
        Return Not ar Is Nothing
    End Function

    Public Function finished() As Boolean
        Return Not begin_succeeded() OrElse state.finished()
    End Function

    Public Sub force_finish()
        If Not state.finished() AndAlso begin_succeeded() Then
            async_result_destructor.queue(state, wait_ar())
        End If
    End Sub
End Class
