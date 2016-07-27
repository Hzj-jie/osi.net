
'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with slimheapless_case4.vbp ----------
'so change slimheapless_case4.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with qless_case4.vbp ----------
'so change qless_case4.vbp instead of this file



Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.lock
Imports osi.root.utt

Friend Class slimheapless_case4
    Inherits multithreading_case_wrapper

    Public Sub New(Optional ByVal size As Int64 = 32 * 1024 * 1024, Optional ByVal threadcount As Int32 = 8)
        MyBase.New(repeat(New case4(), size), threadcount)
    End Sub

    Private Class case4
        Inherits [case]

        Private ReadOnly q As slimheapless(Of Int32)
        Private ReadOnly s As atomic_int

        Public Sub New()
            q = New slimheapless(Of Int32)()
            s = New atomic_int()
        End Sub

        Public Overrides Function run() As Boolean
            If rnd_bool() OrElse s.get() > (multithreading_case_wrapper.running_thread_count() >> 1) Then
                If s.decrement() >= 0 Then
                    assert_true(q.pop(Nothing))
                Else
                    s.increment()
                    q.emplace(rnd_int())
                    s.increment()
                End If
            Else
                q.emplace(rnd_int())
                s.increment()
            End If
            Return True
        End Function

        Public Overrides Function finish() As Boolean
            While s.decrement() >= 0
                assert_true(q.pop(Nothing))
            End While
            assert_false(q.pop(Nothing))
            Return MyBase.finish()
        End Function
    End Class
End Class

'finish qless_case4.vbp --------
'finish slimheapless_case4.vbp --------
