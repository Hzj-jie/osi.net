
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.template
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class ref_map_test
    <test>
    Private Shared Sub can_allocate_object()
        Dim m As ref_map(Of Int32, Int32) = Nothing
        m = New ref_map(Of Int32, Int32)()
        Using r As ref_ptr(Of Int32) = m.get(100, Function() As Int32
                                                      Return 100
                                                  End Function)
            assertion.is_true(r.referred())
            assertion.equal(+r, 100)
        End Using
        Using r As ref_ptr(Of Int32) = m.get(100)
            assertion.is_true(r.referred())
            assertion.equal(+r, 100)
        End Using
    End Sub

    <test>
    Private Shared Sub unref_will_remove_instance()
        Dim m As ref_map(Of Int32, Int32) = Nothing
        m = New ref_map(Of Int32, Int32)()
        m.get(100, Function() As Int32
                       Return 100
                   End Function).unref()
        assertion.is_false(m.created(100))
    End Sub

    <test>
    Private Shared Sub getting_twice_return_same_instance()
        Dim m As ref_map(Of Int32, Int32) = Nothing
        m = New ref_map(Of Int32, Int32)()
        Dim r As ref_ptr(Of Int32) = Nothing
        r = m.get(100, Function() As Int32
                           Return 100
                       End Function)
        Dim r2 As ref_ptr(Of Int32) = Nothing
        r2 = m.get(100)
        assertion.reference_equal(r, r2)
        assertion.equal(r.ref_count(), uint32_2)
        r.unref()
        assertion.equal(r.ref_count(), uint32_1)
        r.unref()
        assertion.is_false(r.referred())
        assertion.is_false(r2.referred())
        assertion.is_false(m.created(100))
    End Sub

    <test>
    Private Shared Sub wont_create_two_instances()
        Dim m As ref_map(Of Int32, cd_object(Of joint_type(Of ref_map_test, _0))) = Nothing
        m.[New]()
        Dim r As ref_ptr(Of cd_object(Of joint_type(Of ref_map_test, _0))) = Nothing
        r = m.get(100, Function() As cd_object(Of joint_type(Of ref_map_test, _0))
                           Return New cd_object(Of joint_type(Of ref_map_test, _0))()
                       End Function)
        Dim r2 As ref_ptr(Of cd_object(Of joint_type(Of ref_map_test, _0))) = Nothing
        r2 = m.get(100, Function() As cd_object(Of joint_type(Of ref_map_test, _0))
                            Return New cd_object(Of joint_type(Of ref_map_test, _0))()
                        End Function)
        assertion.reference_equal(+r, +r2)
        assertion.equal(cd_object(Of joint_type(Of ref_map_test, _0)).constructed(), uint32_1)
        assertion.equal(cd_object(Of joint_type(Of ref_map_test, _0)).disposed(), uint32_0)
        r.unref()
        r2.unref()
        assertion.equal(cd_object(Of joint_type(Of ref_map_test, _0)).constructed(), uint32_1)
        assertion.equal(cd_object(Of joint_type(Of ref_map_test, _0)).disposed(), uint32_1)
    End Sub

    <test>
    Private Shared Sub create_different_instances_with_different_keys()
        Dim m As ref_map(Of Int32, cd_object(Of joint_type(Of ref_map_test, _1))) = Nothing
        m.[New]()
        Dim r As ref_ptr(Of cd_object(Of joint_type(Of ref_map_test, _1))) = Nothing
        r = m.get(1, Function() As cd_object(Of joint_type(Of ref_map_test, _1))
                         Return New cd_object(Of joint_type(Of ref_map_test, _1))()
                     End Function)
        Dim r2 As ref_ptr(Of cd_object(Of joint_type(Of ref_map_test, _1))) = Nothing
        r2 = m.get(2, Function() As cd_object(Of joint_type(Of ref_map_test, _1))
                          Return New cd_object(Of joint_type(Of ref_map_test, _1))()
                      End Function)
        assertion.not_reference_equal(+r, +r2)
        assertion.equal(cd_object(Of joint_type(Of ref_map_test, _1)).constructed(), uint32_2)
        assertion.equal(cd_object(Of joint_type(Of ref_map_test, _1)).disposed(), uint32_0)
        r.unref()
        assertion.equal(cd_object(Of joint_type(Of ref_map_test, _1)).constructed(), uint32_2)
        assertion.equal(cd_object(Of joint_type(Of ref_map_test, _1)).disposed(), uint32_1)
        r2.unref()
        assertion.equal(cd_object(Of joint_type(Of ref_map_test, _1)).constructed(), uint32_2)
        assertion.equal(cd_object(Of joint_type(Of ref_map_test, _1)).disposed(), uint32_2)
    End Sub

    <test>
    Private Shared Sub multi_threading_get_should_succeed()
        case2.run(Of multi_threading_get_should_succeed_case)().assert_succeeded()
    End Sub

    <test>
    Private NotInheritable Class multi_threading_get_should_succeed_case
        Private ReadOnly m As ref_map(Of Int32, cd_object(Of joint_type(Of ref_map_test, _2)))
        Private ReadOnly s As [set](Of Int32)

        <test>
        <multi_threading(4)>
        <repeat(10000)>
        Private Sub get_object()
            Dim i As Int32 = 0
            i = rnd_int(0, 100)
            SyncLock s
                s.emplace(i)
            End SyncLock
            m.get(i, Function() As cd_object(Of joint_type(Of ref_map_test, _2))
                         Return New cd_object(Of joint_type(Of ref_map_test, _2))()
                     End Function)
        End Sub

        <finish>
        Private Sub check_created_instances()
            assertion.equal(s.size(), m.created_size())
            assertion.equal(s.size(), cd_object(Of joint_type(Of ref_map_test, _2)).constructed())
        End Sub

        Private Sub New()
            m.[New]()
            s.[New]()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
