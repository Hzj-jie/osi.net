
Imports System.Threading
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt

Public Class arrayless_test
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(multithreading(repeat(New arrayless_case(), 1024 * 1024), 8))
    End Sub

    Private Class arrayless_case
        Inherits [case]

        Private ReadOnly size As UInt32
        Private a As arrayless(Of cd_object(Of arrayless_case))
        Private ids() As Int32

        Public Sub New()
            size = rnd_uint(100000, 200000)
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                a = New arrayless(Of cd_object(Of arrayless_case))(size)
                ReDim ids(size - 1)
                For i As Int32 = 0 To size - 1
                    ids(i) = npos
                Next
                cd_object(Of arrayless_case).reset()
                Return True
            Else
                Return False
            End If
        End Function

        Private Sub assert_ids(ByVal id As Int32, ByVal o As cd_object(Of arrayless_case))
            If assert_not_nothing(o) Then
                assert_true(ids(id) = npos OrElse ids(id) = o.id)
                ids(id) = o.id
                assert_equal(ids(id), CInt(o.id))
            End If
            assert_true(a.get(id, o))
            If assert_not_nothing(o) Then
                assert_equal(ids(id), CInt(o.id))
            End If
        End Sub

        Public Overrides Function run() As Boolean
            Dim id As UInt32 = 0
            Dim o As cd_object(Of arrayless_case) = Nothing
            If repeat_case_wrapper.this.current_round() < 1000 AndAlso rnd_bool_trues(3) Then
                If a.next(id, o) Then
                    assert_ids(id, o)
                Else
                    assert_equal(a.created_count(), size)
                    assert_equal(cd_object(Of arrayless_case).constructed(), size)
                End If
            ElseIf repeat_case_wrapper.this.last_round() Then
                While a.next(id, o)
                    assert_ids(id, o)
                End While
            Else
                id = rnd_uint(0, size)
                assert_true(a.[New](id, o))
                assert_ids(id, o)
            End If
            assert_false(a.[New](rnd_uint(size, max_uint32), o))
            assert_false(a.get(rnd_uint(size, max_uint32), o))
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            Dim o As cd_object(Of arrayless_case) = Nothing
            assert_equal(cd_object(Of arrayless_case).constructed(), size)
            assert_equal(a.created_count(), cd_object(Of arrayless_case).constructed())
            For i As Int32 = 0 To size - 1
                assert_true(a.get(i, o))
                assert_not_nothing(o)
                assert_equal(CInt(o.id), CInt(ids(i)))
            Next
            a.clear()
            assert_equal(a.created_count(), uint32_0)
            a = Nothing
            ids = Nothing
            Return MyBase.finish()
        End Function
    End Class
End Class
