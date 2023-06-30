
Imports osi.root.connector
Imports osi.root.utt

Public Class shared_ctor_callstack_behavior_test
    Inherits [case]

    Private Shared A_stack As String
    Private Shared B_stack As String
    Private Shared A_order As Int32
    Private Shared B_order As Int32
    Private Shared order As Int32

    Private Class A
        Shared Sub New()
            A_stack = callstack()
            A_order = order
            order += 1
        End Sub
    End Class

    Private Class B
        Inherits A

        Shared Sub New()
            static_constructor(Of A).execute()
            B_stack = callstack()
            B_order = order
            order += 1
        End Sub
    End Class

    Public Overrides Function run() As Boolean
        Dim x As B = Nothing
        x = New B()
        assertion.is_true(strcontains(A_stack, ".B..cctor()"))
        assertion.is_true(in_shared_constructor_of(A_stack, GetType(B)))

        assertion.is_false(strcontains(B_stack, ".A..cctor()"))
        assertion.is_false(in_shared_constructor_of(B_stack, GetType(A)))

        assertion.equal(A_order, 0)
        assertion.equal(B_order, 1)
        Return True
    End Function
End Class
