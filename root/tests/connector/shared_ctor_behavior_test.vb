
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class shared_ctor_behavior_test
    Private NotInheritable Class A_holder
        Public Shared v As Boolean
    End Class

    Private NotInheritable Class A_executor
        Public Sub New()
            A_holder.v = True
        End Sub
    End Class

    Private NotInheritable Class A
        Private Shared ReadOnly executor As A_executor = New A_executor()

        Public Shared Sub init()
        End Sub
    End Class

    ' Do not use empty init to trigger shared ctor or variable.
    <test>
    Private Shared Sub wont_trigger_shared_variable()
        assertion.is_false(A_holder.v)
        A.init()
        assertion.is_false(A_holder.v)
    End Sub

    Private NotInheritable Class B_holder
        Public Shared v As Boolean
    End Class

    Private NotInheritable Class B
        Shared Sub New()
            B_holder.v = True
        End Sub

        Public Shared Sub init()
        End Sub
    End Class

    <test>
    Private Shared Sub trigger_shared_ctor()
        assertion.is_false(B_holder.v)
        B.init()
        assertion.is_true(B_holder.v)
    End Sub

    Private Sub New()
    End Sub
End Class
