
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.event
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt

Public Class promise_test
    Inherits [case]

    Private Shared Function basic_case() As Boolean
        Const value As Int32 = 20
        Dim mre As ManualResetEvent = Nothing
        mre = New ManualResetEvent(False)
        Dim start_ms As Int64 = 0
        start_ms = nowadays.milliseconds()
        Dim p As promise = Nothing
        p = New promise(Sub(ByVal resolve As Action(Of Object))
                            assert(Not stopwatch.push(20,
                                                      Sub()
                                                          resolve(value)
                                                      End Sub) Is Nothing)
                        End Sub)
        p.then(Sub(v As Object)
                   assertion.is_true(v.GetType().[is](Of Int32)())
                   assertion.equal(v, value)
                   assert(mre.force_set())
               End Sub)
        mre.wait_close()
        assertion.more_or_equal(nowadays.milliseconds() - start_ms, 20)
        Return True
    End Function

    Private Shared Function chain_more_case() As Boolean
        Const value1 As Int32 = 20
        Const value2 As Int32 = 20
        Dim ce As count_event = Nothing
        ce = New count_event(2)
        Dim start_ms As Int64 = 0
        start_ms = nowadays.milliseconds()
        Dim p As promise = Nothing
        p = New promise(Sub(ByVal resolve As Action(Of Object))
                            assert(Not stopwatch.push(10,
                                                      Sub()
                                                          resolve(value1)
                                                      End Sub) Is Nothing)
                        End Sub)
        p.then(Function(v As Object) As Object
                   assertion.is_true(v.GetType().[is](Of Int32)())
                   assertion.equal(v, value1)
                   assertion.equal(ce.decrement(), uint32_1)
                   Return New promise(Sub(ByVal resolve As Action(Of Object))
                                          assert(Not stopwatch.push(10,
                                                                    Sub()
                                                                        resolve(value2)
                                                                    End Sub) Is Nothing)
                                      End Sub)
               End Function).
          then(Sub(v As Object)
                   assertion.is_true(v.GetType().[is](Of Int32)())
                   assertion.equal(v, value2)
                   assertion.equal(ce.decrement(), uint32_0)
               End Sub)
        assert(ce.wait())
        assertion.more_or_equal(nowadays.milliseconds() - start_ms, 20)
        Return True
    End Function

    Private Shared Function reject_case() As Boolean
        Const reason As Int32 = 20
        Dim mre As ManualResetEvent = Nothing
        mre = New ManualResetEvent(False)
        Dim start_ms As Int64 = 0
        start_ms = nowadays.milliseconds()
        Dim p As promise = Nothing
        p = New promise(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                            assert(Not stopwatch.push(20,
                                                      Sub()
                                                          reject(reason)
                                                      End Sub) Is Nothing)
                        End Sub)
        p.then(Sub(ByVal result As Object)
                   assertion.is_true(False)
               End Sub,
               Sub(ByVal r As Object)
                   assertion.is_true(r.GetType().[is](Of Int32)())
                   assertion.equal(r, reason)
                   assert(mre.force_set())
               End Sub)
        assert(mre.wait())
        assertion.more_or_equal(nowadays.milliseconds() - start_ms, 20)
        Return True
    End Function

    Private Shared Function chain_more_rejects_case() As Boolean
        Const reason As Int32 = 20
        Dim ce As count_event = Nothing
        ce = New count_event(2)
        Dim start_ms As Int64 = 0
        start_ms = nowadays.milliseconds()
        Dim p As promise = Nothing
        p = New promise(Sub(ByVal resolve As Action(Of Object), ByVal reject As Action(Of Object))
                            assert(Not stopwatch.push(20,
                                                      Sub()
                                                          reject(reason)
                                                      End Sub) Is Nothing)
                        End Sub)
        p.then(Sub(ByVal result As Object)
                   assertion.is_true(False)
               End Sub,
               Sub(ByVal r As Object)
                   assertion.is_true(r.GetType().[is](Of Int32)())
                   assertion.equal(r, reason)
                   assertion.equal(ce.decrement(), uint32_1)
               End Sub).
          then(Sub(ByVal result As Object)
                   assertion.is_true(False)
               End Sub,
               Sub(ByVal r As Object)
                   assertion.is_true(r.GetType().[is](Of Int32)())
                   assertion.equal(r, reason)
                   assertion.equal(ce.decrement(), uint32_0)
               End Sub)
        assert(ce.wait())
        assertion.more_or_equal(nowadays.milliseconds() - start_ms, 20)
        Return True
    End Function

    Private Shared Function share_resolve_case() As Boolean
        Const value As Int32 = 20
        Dim mre As ManualResetEvent = Nothing
        mre = New ManualResetEvent(False)
        Dim start_ms As Int64 = 0
        start_ms = nowadays.milliseconds()
        Dim p As promise = Nothing
        p = New promise(Function() As Object
                            Return promise.resolve(New promise(Sub(ByVal resolve As Action(Of Object))
                                                                   assert(Not stopwatch.push(10, Sub()
                                                                                                     resolve(value)
                                                                                                 End Sub) Is Nothing)
                                                               End Sub))
                        End Function)
        p.then(Sub(ByVal result As Object)
                   assertion.is_true(result.GetType().[is](Of Int32)())
                   assertion.equal(result, value)
                   assert(mre.force_set())
               End Sub)
        assert(mre.wait())
        assertion.more_or_equal(nowadays.milliseconds() - start_ms, 10)
        Return True
    End Function

    Private Shared Function return_result_case() As Boolean
        Const value As Int32 = 20
        Dim finished As Boolean = False
        Dim p As promise = Nothing
        p = New promise(Function() As Object
                            Return value
                        End Function)
        p.then(Sub(v As Object)
                   assertion.is_true(v.GetType().[is](Of Int32)())
                   assertion.equal(v, value)
                   finished = True
               End Sub)
        assertion.is_true(finished)
        Return True
    End Function

    Private Shared Function return_promise_case() As Boolean
        Const value As Int32 = 20
        Dim mre As ManualResetEvent = Nothing
        mre = New ManualResetEvent(False)
        Dim start_ms As Int64 = 0
        start_ms = nowadays.milliseconds()
        Dim p As promise = Nothing
        p = New promise(Function() As Object
                            Return New promise(Sub(ByVal resolve As Action(Of Object))
                                                   assert(Not stopwatch.push(10, Sub()
                                                                                     resolve(value)
                                                                                 End Sub) Is Nothing)
                                               End Sub)
                        End Function)
        p.then(Sub(v As Object)
                   assertion.is_true(v.GetType().[is](Of Int32)())
                   assertion.equal(v, value)
                   assert(mre.force_set())
               End Sub)
        assert(mre.wait())
        assertion.more_or_equal(nowadays.milliseconds() - start_ms, 10)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return basic_case() AndAlso
               chain_more_case() AndAlso
               reject_case() AndAlso
               chain_more_rejects_case() AndAlso
               share_resolve_case() AndAlso
               return_result_case() AndAlso
               return_promise_case()
    End Function
End Class
