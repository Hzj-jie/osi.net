
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
        assert_true(GetType(member_info_behavior_test).GetType().inherit(GetType(Type)))
        assert_equal(GetType(member_info_behavior_test).ToString(), GetType(member_info_behavior_test).FullName())
        assert_equal(GetType(member_info_behavior_test).ToString(),
                     "osi.tests.root.connector.member_info_behavior_test")
        assert_equal(GetType(member_info_behavior_test).Name(), "member_info_behavior_test")
        assert_nothing(GetType(member_info_behavior_test).DeclaringType())

        assert_true(GetType(b).GetMember("c")(0).GetType().inherit(GetType(MemberInfo)))
        assert_equal(GetType(b).GetMember("c")(0).ToString(), "Int32 c")
        assert_equal(GetType(b).GetMember("c")(0).Name(), "c")
        assert_not_nothing(GetType(b).GetMember("c")(0).DeclaringType())
        assert_equal(GetType(b).GetMember("c")(0).DeclaringType().FullName(),
                     "osi.tests.root.connector.member_info_behavior_test+b")

        assert_true(GetType(member_info_behavior_test).GetMethod("a").GetType().inherit(GetType(MethodInfo)))
        assert_equal(GetType(member_info_behavior_test).GetMethod("a").ToString(), "Void a()")
        assert_equal(GetType(member_info_behavior_test).GetMethod("a").Name(), "a")
        assert_not_nothing(GetType(member_info_behavior_test).GetMethod("a").DeclaringType())
        assert_equal(GetType(member_info_behavior_test).GetMethod("a").DeclaringType().FullName(),
                     "osi.tests.root.connector.member_info_behavior_test")
        Return True
    End Function
End Class
