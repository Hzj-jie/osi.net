
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utt

Public Class member_info_behavior_test
    Inherits [case]

    Public Sub a()
    End Sub

    Public Class b
        Public c As Int32
    End Class

    Public Overrides Function run() As Boolean
        assertion.is_true(GetType(member_info_behavior_test).GetType().inherit(GetType(Type)))
        assertion.equal(GetType(member_info_behavior_test).ToString(), GetType(member_info_behavior_test).FullName())
        assertion.equal(GetType(member_info_behavior_test).ToString(),
                     "osi.tests.root.connector.member_info_behavior_test")
        assertion.equal(GetType(member_info_behavior_test).Name(), "member_info_behavior_test")
        assertion.is_null(GetType(member_info_behavior_test).DeclaringType())

        assertion.is_true(GetType(b).GetMember("c")(0).GetType().inherit(GetType(MemberInfo)))
        assertion.equal(GetType(b).GetMember("c")(0).ToString(), "Int32 c")
        assertion.equal(GetType(b).GetMember("c")(0).Name(), "c")
        assertion.is_not_null(GetType(b).GetMember("c")(0).DeclaringType())
        assertion.equal(GetType(b).GetMember("c")(0).DeclaringType().FullName(),
                     "osi.tests.root.connector.member_info_behavior_test+b")

        assertion.is_true(GetType(member_info_behavior_test).GetMethod("a").GetType().inherit(GetType(MethodInfo)))
        assertion.equal(GetType(member_info_behavior_test).GetMethod("a").ToString(), "Void a()")
        assertion.equal(GetType(member_info_behavior_test).GetMethod("a").Name(), "a")
        assertion.is_not_null(GetType(member_info_behavior_test).GetMethod("a").DeclaringType())
        assertion.equal(GetType(member_info_behavior_test).GetMethod("a").DeclaringType().FullName(),
                     "osi.tests.root.connector.member_info_behavior_test")
        Return True
    End Function
End Class
