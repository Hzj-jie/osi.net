
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class joint_type_test
    Private Interface t1
    End Interface

    Private Interface t2
    End Interface

    Private Interface t3
    End Interface

    Private Interface t4
    End Interface

    Private Interface t5
    End Interface

    Private Interface t6
    End Interface

    Private Interface t7
    End Interface

    Private Interface t8
    End Interface

    Private Interface t9
    End Interface

    Private Interface t10
    End Interface

    <test>
    Private Shared Sub _2()
        assertion.equal(GetType(joint_type(Of t1, t2)), joint_type.of(GetType(t1), GetType(t2)))
        assertion.is_null(joint_type.of(GetType(t1), Nothing))
        assertion.is_null(joint_type.of(Nothing, GetType(t2)))
    End Sub

    <test>
    Private Shared Sub _3()
        assertion.equal(GetType(joint_type(Of t1, t2, t3)), joint_type.of(GetType(t1), GetType(t2), GetType(t3)))
        assertion.is_null(joint_type.of(GetType(t1), Nothing, Nothing))
        assertion.is_null(joint_type.of(Nothing, GetType(t2), Nothing))
        assertion.is_null(joint_type.of(Nothing, Nothing, GetType(t3)))
    End Sub

    ' TODO: Other tests

    Private Sub New()
    End Sub
End Class
