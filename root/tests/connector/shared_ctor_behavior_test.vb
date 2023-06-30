
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
        ' If init() is inlining, the executor construction will be ignored.
#If DEBUG Then
        assertion.is_true(A_holder.v)
#Else
        assertion.is_false(A_holder.v)
#End If
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

    Private NotInheritable Class C_holder
        Public Shared v As Boolean
    End Class

    Private NotInheritable Class C_executor
        Public Shared Function execute() As Int32
            C_holder.v = True
            Return 10
        End Function

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class C
        Private Shared ReadOnly executor As Int32 = C_executor.execute()

        Public Shared Sub init()
        End Sub
    End Class

    <test>
    Private Shared Sub wont_trigger_shared_variable_if_not_using_constructor()
        assertion.is_false(C_holder.v)
        C.init()
        ' If init() is inlining, the executor construction will be ignored.
#If DEBUG Then
        assertion.is_true(C_holder.v)
#Else
        assertion.is_false(C_holder.v)
#End If
    End Sub

    Private NotInheritable Class D_holder
        Public Shared v As Boolean
    End Class

    Private NotInheritable Class D_executor
        Public Shared Function execute() As Int32
            D_holder.v = True
            Return 0
        End Function

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class D
        Private Shared ReadOnly executor As Int32 = D_executor.execute()

        Public Shared Sub init()
        End Sub
    End Class

    <test>
    Private Shared Sub wont_trigger_shared_variable_if_return_default()
        assertion.is_false(D_holder.v)
        D.init()
        ' If init() is inlining, the executor construction will be ignored.
#If DEBUG Then
        assertion.is_true(D_holder.v)
#Else
        assertion.is_false(D_holder.v)
#End If
    End Sub

    Private Sub New()
    End Sub
End Class
