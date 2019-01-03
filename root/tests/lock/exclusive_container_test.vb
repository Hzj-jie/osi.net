
Imports System.Threading
Imports osi.root.lock
Imports osi.root.utt

Public Class exclusive_container_test
    Inherits chained_case_wrapper

    Public Const thread_count As UInt32 = 8
    Public Const repeat_count As UInt32 = 1024

    Public Sub New()
        MyBase.New(rinne(multithreading(repeat(New create_case(), repeat_count), thread_count), 1024),
                   rinne(multithreading(repeat(New clear_case(), 16), thread_count), 4 * 1024))
    End Sub

    Private Class fake_class
        Private i As Int32

        Public Sub New()
            i = 0
        End Sub

        Public Function ref_count() As Int32
            Return atomic.read(i)
        End Function

        Public Sub ref()
            Interlocked.Increment(i)
        End Sub
    End Class

    Private Class create_case
        Inherits [case]

        Private ReadOnly ec As exclusive_container(Of fake_class)
        Private create_suc As Int32

        Public Sub New()
            MyBase.New()
            ec = New exclusive_container(Of fake_class)()
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                ec.clear()
                create_suc = 0
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If ec.create(Of fake_class)() Then
                assertion.equal(Interlocked.Increment(create_suc), 1)
            End If
            If assertion.is_true(ec.has_value()) Then
                ec.get().ref()
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            If MyBase.finish() Then
                assertion.equal(create_suc, 1)
                If assertion.is_true(ec.has_value()) Then
                    assertion.equal(ec.get().ref_count(), CInt(thread_count * repeat_count))
                End If
                Return True
            Else
                Return False
            End If
        End Function
    End Class

    Private Class clear_case
        Inherits [case]

        Private ReadOnly ec As exclusive_container(Of fake_class)
        Private clear_suc As Int32

        Public Sub New()
            MyBase.New()
            ec = New exclusive_container(Of fake_class)()
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                ec.create(Of fake_class)()
                clear_suc = 0
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If ec.clear() Then
                assertion.equal(Interlocked.Increment(clear_suc), 1)
            End If
            assertion.is_false(ec.has_value())
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            If MyBase.finish() Then
                assertion.equal(clear_suc, 1)
                assertion.is_false(ec.has_value())
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Class
