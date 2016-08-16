
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

        Public Overrides Function run() As Boolean
            Dim id As UInt32 = 0
            Dim o As cd_object(Of arrayless_case) = Nothing
            id = rnd_uint(0, size)
            assert_true(a.[New](id, o))
            If assert_not_nothing(o) Then
                atomic.compare_exchange(ids(id), o.id, npos)
                assert_equal(ids(id), CInt(o.id))
            End If
            assert_true(a.get(id, o))
            If assert_not_nothing(o) Then
                atomic.compare_exchange(ids(id), o.id, npos)
                assert_equal(ids(id), CInt(o.id))
            End If
            assert_false(a.[New](rnd_uint(size, max_uint32), o))
            assert_false(a.get(rnd_uint(size, max_uint32), o))
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            assert_more(cd_object(Of arrayless_case).constructed(), uint32_1)
            assert_equal(a.created_count(), cd_object(Of arrayless_case).constructed())
            a.clear()
            assert_equal(a.created_count(), uint32_0)
            a = Nothing
            ids = Nothing
            Return MyBase.finish()
        End Function
    End Class
End Class
