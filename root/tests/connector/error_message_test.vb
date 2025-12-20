
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public NotInheritable Class error_message_test
    Inherits [case]

    Private Shared Function merge_case() As Boolean
        Const s1 As String = "first, "
        Const s2 As String = "second, "
        Const s3 As String = "third, "
        Const s4 As String = "fourth, "

        assertion.equal(error_message.merge({s1, New String() {s2, s3}, s4}), String.Concat(s1, s2, s3, s4))
        Return True
    End Function

    Private Shared Function null_msg() As Boolean
        assertion.equal(error_message.merge(New Object() {"1", Nothing, 2}), "12")
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return merge_case() AndAlso
               null_msg()
    End Function
End Class
