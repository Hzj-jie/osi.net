
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.envs
Imports osi.root.formation
Imports osi.root.utt
Imports osi.service.convertor

Public Class environment_transform_test
    Inherits [case]

    Private ReadOnly kv() As pair(Of String, String)

    Sub New()
        ReDim kv(16 - 1)
        For i As Int32 = 0 To array_size_i(kv) - 1
            kv(i) = emplace_make_pair(guid_str(), guid_str())
        Next
    End Sub

    Private Function run_case() As Boolean
        Const start_str As String = environment_transform_default_start_str
        Const end_str As String = environment_transform_default_end_str
        Dim p As String = Nothing
        Dim e As String = Nothing
        For i As Int32 = 0 To 128 - 1
            If rnd_bool() Then
                Dim r As Int32 = 0
                r = rnd_int(0, array_size_i(kv))
                p += start_str + kv(r).first + end_str
                e += kv(r).second
            ElseIf rnd_bool() Then
                p += start_str
                e += start_str
            ElseIf rnd_bool() Then
                p += end_str
                e += end_str
            Else
                Dim r As String = Nothing
                'hope there is no default environment ends with HHHH
                r = rnd_en_chars(rnd_int(1, 20)) + "HHHH"
                p += r
                e += r
            End If
        Next
        Dim o As String = Nothing
        o = p.env_transform()
        assertion.equal(o, e, p, " -> ", o, ", exp ", e)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Using New scoped_environments(kv.to_array())
            For i As Int32 = 0 To 1024 * 32 - 1
                If Not run_case() Then
                    Return False
                End If
            Next
            Return True
        End Using
    End Function
End Class
