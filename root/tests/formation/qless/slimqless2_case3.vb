
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with slimqless2_case3.vbp ----------
'so change slimqless2_case3.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with qless_case3.vbp ----------
'so change qless_case3.vbp instead of this file



Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.lock
Imports osi.root.formation
Imports osi.root.utt

Friend Class slimqless2_case3
    Inherits multithreading_case_wrapper

    Public Sub New(Optional ByVal size As Int64 = 32 * 1024 * 1024, Optional ByVal threadcount As Int32 = 8)
        MyBase.New(repeat(New case3(), size), threadcount)
    End Sub

    Private Class case3
        Inherits [case]

        Private Const range As Int32 = 20000
        Private ReadOnly q As slimqless2(Of Int64)
        Private ReadOnly i As range_int

        Public Sub New()
            q = New slimqless2(Of Int64)()
            i = New range_int(0, range)
        End Sub

        Public Overrides Function prepare() As Boolean
            If MyBase.prepare() Then
                i.reset()
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Function run() As Boolean
            If multithreading_case_wrapper.thread_id() = 0 Then
                q.pop(Nothing)
                If multithreading_case_wrapper.running_thread_count() > 1 Then
                    sleep(rnd_int(0, 100))
                End If
            Else
                Dim j As Int32 = 0
                j = i.increment() Mod (range << 1)
                Dim pop As Boolean = False
                If j >= range Then
                    pop = rnd_int64(int64_0, range) <= ((range << 1) - j - 1)
                Else
                    pop = rnd_int64(int64_0, range) <= j
                End If
                If pop Then
                    q.pop(Nothing)
                Else
                    q.emplace(rnd_int64())
                End If
            End If
            Return True
        End Function
    End Class
End Class
'finish qless_case3.vbp --------
'finish slimqless2_case3.vbp --------
