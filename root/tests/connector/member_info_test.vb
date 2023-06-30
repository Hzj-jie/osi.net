
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class member_info_test
    Inherits [case]

    Private Class a
        Public b As Int32

        Public Function c() As Int32
            Return 0
        End Function

        Public Event d()

        Public Property e() As Int32
    End Class

    Public Overrides Function run() As Boolean
        assertion.equal(GetType(member_info_test).full_name(),
                     "osi.tests.root.connector.member_info_test")
        assertion.equal(GetType(a).full_name(),
                     "osi.tests.root.connector.member_info_test+a")
        assertion.equal(GetType(a).GetMember("b")(0).full_name(),
                     "osi.tests.root.connector.member_info_test+a.{Int32 b}")
        assertion.equal(GetType(a).GetMethod("c").full_name(),
                     "osi.tests.root.connector.member_info_test+a.{Int32 c()}")
        assertion.equal(GetType(a).GetEvent("d").full_name(),
                     "osi.tests.root.connector.member_info_test+a.{dEventHandler d}")
        assertion.equal(GetType(a).GetProperty("e").full_name(),
                     "osi.tests.root.connector.member_info_test+a.{Int32 e}")

        Return True
    End Function
End Class
