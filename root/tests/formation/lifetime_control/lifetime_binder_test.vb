
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
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

            Public Shared Sub clear_finalized()
                atomic.eva(finalized, 0)
            End Sub

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

        Private Shared Function natural_lifetime_binder_case() As Boolean
            test_class.clear_finalized()
            Const refer_only As String = "refer-only"
            Const bind As String = "bind"
            Dim tc1 As test_class = Nothing
            Dim wr1 As WeakReference = Nothing
            Dim wr2 As WeakReference = Nothing
            create(wr1, tc1, refer_only)
            create_bind(wr2, bind)

            garbage_collector.repeat_collect()
            assertion.equal(test_class.finalized_count(), 0)
            assertion.is_true(wr1.IsAlive())
            assertion.is_true(wr2.IsAlive())
            assertion.reference_equal(tc1, cast(Of test_class)(wr1.Target()))
            assertion.equal(cast(Of test_class)(wr1.Target()).s, refer_only)
            assertion.equal(tc1.s, refer_only)
            assertion.equal(cast(Of test_class)(wr2.Target()).s, bind)

            GC.KeepAlive(tc1)
            tc1 = Nothing
            garbage_collector.repeat_collect()
#If NET8_0_OR_GREATER Then
            ' In modern .NET (RyuJIT), temporary stack/register references to tc1/tc2 evaluated in the method
            ' remain rooted until the method returns.
            assertion.less_or_equal(test_class.finalized_count(), 2)
#Else
            assertion.equal(test_class.finalized_count(), 1)
            assertion.is_false(wr1.IsAlive())
            assertion.is_true(wr2.IsAlive())
            assertion.equal(cast(Of test_class)(wr2.Target()).s, bind)
#End If

            lifetime_binder(Of test_class).instance.erase(direct_cast(Of test_class)(wr2.Target()))
            garbage_collector.repeat_collect()
#If NET8_0_OR_GREATER Then
            assertion.less_or_equal(test_class.finalized_count(), 2)
#Else
            assertion.equal(test_class.finalized_count(), 2)
            assertion.is_false(wr2.IsAlive())
#End If
            Return True
        End Function

        Public Overrides Function run() As Boolean
#If NET8_0_OR_GREATER Then
            If Not lifetime_binder_with_allocate_test_objects_case() Then
                Return False
            End If
#End If
            Return natural_lifetime_binder_case()
        End Function

#If NET8_0_OR_GREATER Then
        <MethodImpl(MethodImplOptions.NoInlining)>
        Private Shared Sub allocate_and_drop_refer_only(ByRef wr As WeakReference, ByVal s As String)
            Dim tc As test_class = Nothing
            create(wr, tc, s)
            assertion.is_true(wr.IsAlive())
            assertion.equal(cast(Of test_class)(wr.Target()).s, s)
        End Sub

        <MethodImpl(MethodImplOptions.NoInlining)>
        Private Shared Sub assert_bind_target_and_erase(ByVal wr As WeakReference, ByVal exp As String)
            assertion.is_true(wr.IsAlive())
            assertion.equal(cast(Of test_class)(wr.Target()).s, exp)
            lifetime_binder(Of test_class).instance.erase(direct_cast(Of test_class)(wr.Target()))
        End Sub

        Private Shared Function lifetime_binder_with_allocate_test_objects_case() As Boolean
            test_class.clear_finalized()
            Const refer_only As String = "refer-only"
            Const bind As String = "bind"
            Dim wr1 As WeakReference = Nothing
            Dim wr2 As WeakReference = Nothing
            allocate_and_drop_refer_only(wr1, refer_only)
            create_bind(wr2, bind)

            garbage_collector.repeat_collect()
            assertion.is_false(wr1.IsAlive())
            assert_bind_target_and_erase(wr2, bind)

            garbage_collector.repeat_collect()
            assertion.is_false(wr2.IsAlive())

            Return True
        End Function
#End If
    End Class
End Class
