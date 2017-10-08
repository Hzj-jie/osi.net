
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Threading
Imports osi.root.utt
Imports osi.root.formation
Imports osi.root.connector
Imports osi.root.lock

Public Class lifetime_binder_test
    Inherits flaky_case_wrapper

    Public Sub New()
        MyBase.New(New lifetime_binder_case())
    End Sub

    Private Class lifetime_binder_case
        Inherits [case]

        Private Class test_class
            Public ReadOnly s As String = Nothing
            Private Shared finalized As Int64 = 0

            Public Sub New(ByVal s As String)
                Me.s = s
            End Sub

            Public Shared Function finalized_count() As Int64
                Return atomic.read(finalized)
            End Function

            Protected Overrides Sub Finalize()
                raise_error("finalized test_class with ", s)
                Interlocked.Increment(finalized)
                MyBase.Finalize()
            End Sub
        End Class

        Private Shared Sub create(ByRef wr As WeakReference, ByRef tc As test_class, ByVal s As String)
            tc = New test_class(s)
            wr = New WeakReference(tc)
        End Sub

        Private Shared Sub create_bind(ByRef wr As WeakReference, ByVal s As String)
            Dim tc As test_class = Nothing
            create(wr, tc, s)
            lifetime_binder(Of test_class).instance.insert(tc)
        End Sub

        Public Overrides Function run() As Boolean
            Const refer_only As String = "refer-only"
            Const bind As String = "bind"
            Dim tc1 As test_class = Nothing
            Dim wr1 As WeakReference = Nothing
            Dim wr2 As WeakReference = Nothing
            create(wr1, tc1, refer_only)
            create_bind(wr2, bind)

            repeat_gc_collect()
            assert_equal(test_class.finalized_count(), 0)
            assert_true(wr1.IsAlive())
            assert_true(wr2.IsAlive())
            assert_reference_equal(tc1, cast(Of test_class)(wr1.Target()))
            assert_equal(cast(Of test_class)(wr1.Target()).s, refer_only)
            assert_equal(tc1.s, refer_only)
            assert_equal(cast(Of test_class)(wr2.Target()).s, bind)

            GC.KeepAlive(tc1)
            tc1 = Nothing
            repeat_gc_collect()
            assert_equal(test_class.finalized_count(), 1)
            assert_false(wr1.IsAlive())
            assert_true(wr2.IsAlive())
            assert_equal(cast(Of test_class)(wr2.Target()).s, bind)

            lifetime_binder(Of test_class).instance.erase(direct_cast(Of test_class)(wr2.Target()))
            repeat_gc_collect()
            assert_equal(test_class.finalized_count(), 2)
            assert_false(wr2.IsAlive())

            Return True
        End Function
    End Class
End Class
