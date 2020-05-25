
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public NotInheritable Class weak_ref_test
    Inherits flaky_case_wrapper

    Public Sub New()
        MyBase.New(New weak_ref_case())
    End Sub

    Private NotInheritable Class weak_ref_case
        Inherits [case]

        Private Class test_class
            Public ReadOnly s As String
            Public ReadOnly o As Object
            Public ReadOnly i As Int32
            Public ReadOnly l As Int64
            Public ReadOnly a() As Int32

            Public Sub New(ByVal s As String)
                copy(Me.s, s)
                o = New Object()
                i = rnd_int(0, 10000)
                l = rnd_int64(0, 100000)
                ReDim a(16 * 1024 * rnd_int(1, 5) - 1)
            End Sub
        End Class

        Private Shared Function not_pinning_case() As Boolean
            Const s As String = "this is a test string only"
            Dim c As test_class = Nothing
            c = New test_class(s)
            Dim p As weak_ref(Of test_class) = Nothing
            p = weak_ref.of(c)
            assertion.is_true(p.alive())
            Dim c2 As test_class = Nothing
            assertion.is_true(p.get(c2))
            assertion.equal(c2.s, s)
            c2 = Nothing
            GC.KeepAlive(c)

            c = Nothing
            garbage_collector.repeat_collect()
            assertion.is_false(p.alive())
            assertion.is_false(p.get(c2))

            GC.KeepAlive(p)
            Return True
        End Function

        Private Shared Function not_pinning_multiple_instance_case() As Boolean
            Const s As String = "this is a test string only"
            Dim size As Int32 = 0
            size = 1024 * rnd_int(2, 8)
            Dim cs() As test_class = Nothing
            ReDim cs(size - 1)
            Dim ps() As weak_ref(Of test_class) = Nothing
            ReDim ps(size - 1)

            For i As Int32 = 0 To size - 1
                cs(i) = New test_class(s)
                ps(i) = weak_ref.of(cs(i))
            Next
            garbage_collector.repeat_collect()

            For i As Int32 = 0 To size - 1
                Dim t As test_class = Nothing
                assertion.is_true(ps(i).alive())
                assertion.is_true(ps(i).get(t))
                assertion.equal(t.s, s)
            Next
            cs.gc_keepalive()

            For i As Int32 = 0 To size - 1
                If rnd_bool() Then
                    cs(i) = Nothing
                End If
            Next
            garbage_collector.repeat_collect()

            For i As Int32 = 0 To size - 1
                assertion.equal(Not cs(i) Is Nothing, ps(i).alive())
                If Not cs(i) Is Nothing Then
                    assertion.is_true(ps(i).alive())
                    Dim t As test_class = Nothing
                    assertion.is_true(ps(i).get(t))
                    assertion.equal(t.s, s)
                End If
            Next
            cs.gc_keepalive()

            For i As Int32 = 0 To size - 1
                cs(i) = Nothing
            Next
            garbage_collector.repeat_collect()

            For i As Int32 = 0 To size - 1
                assertion.is_false(ps(i).alive())
            Next

            Return True
        End Function

        Public Overrides Function run() As Boolean
            Return not_pinning_case() AndAlso
                   not_pinning_multiple_instance_case()
        End Function
    End Class
End Class

