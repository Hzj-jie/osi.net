
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt

Public Class slimheapless_concurrently_push_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New exec(), 1024 * 1024))
    End Sub

    Private Class exec
        Inherits [case]

        Private ReadOnly q As slimheapless(Of Int32)
        Private ReadOnly i As atomic_int

        Public Sub New()
            q = New slimheapless(Of Int32)
            i = New atomic_int()
        End Sub

        Public Overrides Function run() As Boolean
            q.emplace(i.increment() - 1)
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            For i As Int32 = 0 To (+Me.i) - 1
                Dim j As Int32 = 0
                assertion.is_true(q.pop(j))
                assertion.less_or_equal(j, +Me.i)
            Next
            Return MyBase.finish()
        End Function
    End Class
End Class

Public Class slimheapless_concurrently_pop_test
    Inherits multithreading_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New exec(), 1024 * 1024))
    End Sub

    Private Class exec
        Inherits [case]

        Private Const size As Int32 = 8 * 1024 * 1024
        Private ReadOnly q As slimheapless(Of Int32)
        Private ReadOnly i As atomic_int

        Public Sub New()
            q = New slimheapless(Of Int32)
            i = New atomic_int()
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                q.clear()
                For j As Int32 = 0 To size - 1
                    q.emplace(j)
                Next
                i.set(size)
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If i.decrement() >= 0 Then
                Dim j As Int32 = 0
                assertion.is_true(q.pop(j))
                assertion.less(j, size)
            Else
                i.increment()
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            While i.decrement() >= 0
                Dim j As Int32 = 0
                assertion.is_true(q.pop(j))
                assertion.less(j, size)
            End While
            Return MyBase.finish()
        End Function
    End Class
End Class

