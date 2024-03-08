
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.connector
Imports osi.root.event
Imports osi.root.procedure
Imports osi.root.utils
Imports osi.root.utt

Public NotInheritable Class promise_share_test
    Inherits [case]

    Private Shared Function all_case() As Boolean
        Const size As Int32 = 100
        Dim mre As New ManualResetEvent(False)
        Dim start_ms As Int64 = nowadays.milliseconds()
        Dim finished(size - 1) As Boolean
        arrays.clear(finished)
        Dim ps(size - 1) As promise
        For i As Int32 = 0 To size - 1
            Dim j As Int32 = i
            ps(i) = New promise(Sub(ByVal resolve As Action(Of Object))
                                    assert(Not stopwatch.push(10,
                                                              Sub()
                                                                  ' resolve() will immediately call on_resolve in p.then
                                                                  finished(j) = True
                                                                  resolve(rnd_int())
                                                              End Sub) Is Nothing)
                                End Sub)
        Next
        Dim p As promise = promise.all(ps).
                                   then(Sub(ByVal result As Object)
                                            assertion.is_null(result)
                                            For i As Int32 = 0 To size - 1
                                                assertion.is_true(finished(i))
                                            Next
                                            assert(mre.force_set())
                                        End Sub)
        assert(mre.wait())
        assertion.more_or_equal(nowadays.milliseconds() - start_ms, 10)
        Return True
    End Function

    Private Shared Function race_case() As Boolean
        Const value1 As Int32 = 10
        Const value2 As Int32 = 20
        Dim ce As New count_event(2)
        Dim finished As Boolean = False
        Dim p As promise = promise.race(New promise(Sub(ByVal resolve As Action(Of Object))
                                                        assert(Not stopwatch.push(1000,
                                                                                  Sub()
                                                                                      resolve(value1)
                                                                                      ce.decrement()
                                                                                  End Sub) Is Nothing)
                                                    End Sub),
                                        New promise(Function() As Object
                                                        ce.decrement()
                                                        Return value2
                                                    End Function)).
                                   then(Sub(ByVal v As Object)
                                            assertion.is_true(v.GetType().[is](Of Int32)())
                                            assertion.equal(v, value2)
                                            finished = True
                                        End Sub)
        assertion.is_true(finished)
        assert(ce.wait())
        Return True
    End Function

    Private Shared Function any_case() As Boolean
        Const value1 As Int32 = 10
        Const value2 As Int32 = 20
        Dim ce As New count_event(3)
        Dim finished As Boolean = False
        Dim p As promise = promise.any(New promise(Sub(ByVal resolve As Action(Of Object))
                                                       assert(Not stopwatch.push(1000,
                                                                                Sub()
                                                                                    resolve(value1)
                                                                                    ce.decrement()
                                                                                End Sub) Is Nothing)
                                                   End Sub),
                                        New promise(Function() As Object
                                                        ce.decrement()
                                                        Return value2
                                                    End Function),
                                        New promise(Sub(ByVal resolve As Action(Of Object),
                                                        ByVal reject As Action(Of Object))
                                                        ce.decrement()
                                                        reject(Nothing)
                                                    End Sub)).
                                   then(Sub(ByVal v As Object)
                                            assertion.is_true(v.GetType().[is](Of Int32)())
                                            assertion.equal(v, value2)
                                            finished = True
                                        End Sub)
        assertion.is_true(finished)
        assert(ce.wait())
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return all_case() AndAlso
               race_case() AndAlso
               any_case()
    End Function
End Class
