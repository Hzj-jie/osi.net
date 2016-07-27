
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utt
Imports osi.root.lock
Imports osi.root.utils

Public Class autolock_test
    Inherits chained_case_wrapper

    Private Shared ReadOnly size As Int64
    Private Shared ReadOnly thread_count As Int32

    Shared Sub New()
        size = 524288 * If(isdebugbuild(), 1, 8)
        thread_count = 4 * If(isdebugbuild(), 1, 4)
    End Sub

    Public Sub New()
        MyBase.New(R(New autolock_case(Of ref(Of slimlock.simplelock))(
                             AddressOf make_autolock(Of slimlock.simplelock))),
                   R(New autolock_case(Of ref(Of monitorlock))(
                             AddressOf make_autolock(Of monitorlock))),
                   R(New autolock_case(Of ref(Of slimlock.monitorlock))(
                             AddressOf make_autolock(Of slimlock.monitorlock))),
                   R(New autolock_case(Of ref(Of slimlock.eventlock))(
                             AddressOf make_autolock(Of slimlock.eventlock))))
    End Sub

    Private Shared Function R(ByVal i As [case]) As [case]
        Return multithreading(repeat(i, size), thread_count)
    End Function

    Private Class autolock_case(Of T)
        Inherits [case]

        Private ReadOnly run_times As atomic_int
        Private ReadOnly make_autolock As _do(Of T, IDisposable)
        Private i As Int32
        Private l As T

        Public Sub New(ByVal ma As _do(Of T, IDisposable))
            assert(Not ma Is Nothing)
            Me.make_autolock = ma
            run_times = New atomic_int()
            If GetType(T).IsClass() Then
                l = alloc(Of T)()
            End If
        End Sub

        Public Overrides Function run() As Boolean
            run_times.increment()
            Using make_autolock(l)
                i += 1
            End Using
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            raise_error("finished islimlock_test case ",
                        Me.GetType().Name(),
                        ", run_times ",
                        +run_times,
                        ", trigger_times ",
                        i)
            assert_equal(i, +run_times)
            Return MyBase.finish()
        End Function
    End Class
End Class
