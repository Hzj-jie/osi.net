
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public NotInheritable Class static_constructor_test
    Inherits [case]

    Private Shared ReadOnly def As Int32
    Private Shared ReadOnly exp As Int32
    Private Shared v As Int32

    Shared Sub New()
        def = rnd_int()
        Do
            exp = rnd_int()
        Loop While exp = def
        v = def
    End Sub

    Private Class C
        Shared Sub New()
            If v = def Then
                v = exp
            Else
                v = def
            End If
        End Sub
    End Class

    Private Class D
    End Class

    Private Class E
        Private Shared v As Boolean

        Shared Sub New()
            assertion.is_false(v)
            v = True
        End Sub
    End Class

    Private NotInheritable Class F_holder
        Public Shared v As Boolean
    End Class

    Private NotInheritable Class F
        Private Shared ReadOnly instance As F = New F()

        Private Sub New()
            F_holder.v = True
        End Sub
    End Class

    Private NotInheritable Class G_holder
        Public Shared v As Boolean
    End Class

    Private NotInheritable Class G_executor
        Public Sub New()
            G_holder.v = True
        End Sub
    End Class

    Private NotInheritable Class G
        Private Shared ReadOnly instance As G_executor = New G_executor()
    End Class

    Public Overrides Function run() As Boolean
        Dim c As C = Nothing
        assertion.equal(v, def)
        static_constructor(Of C).execute()
        assertion.equal(v, exp)
        static_constructor(Of C).execute()
        c = New C()
        assertion.equal(v, exp)

        assertion.is_not_null(static_constructor(Of C).retrieve())
        static_constructor(Of C).as_action()()
#If NET8_0_OR_GREATER Then
        ' In modern .NET (CoreCLR), ConstructorInfo.Invoke on a .cctor that has already executed
        ' is a no-op and will not re-execute the static constructor.
        assertion.equal(v, exp)
#Else
        assertion.equal(v, def)
#End If
        assertion.is_not_null(static_constructor.retrieve(GetType(C)))
        static_constructor.as_action(GetType(C))()
        assertion.equal(v, exp)
        static_constructor.execute(GetType(C))
#If NET8_0_OR_GREATER Then
        assertion.equal(v, exp)
#Else
        assertion.equal(v, def)
#End If

        assertion.is_null(static_constructor(Of D).retrieve())
        assertion.is_null(static_constructor(Of D).as_action())
        static_constructor(Of D).execute()

        static_constructor(Of E).execute()
        static_constructor(Of E).execute()

#If Not NET8_0_OR_GREATER Then
        ' Types with beforefieldinit (like F and G) may be initialized earlier in modern .NET
        ' during test discovery / reflection, so F_holder.v and G_holder.v are not guaranteed
        ' to be False before explicit execution.
        assertion.is_false(F_holder.v)
#End If
        static_constructor(Of F).execute()
        assertion.is_true(F_holder.v)

#If Not NET8_0_OR_GREATER Then
        assertion.is_false(G_holder.v)
#End If
        static_constructor(Of G).execute()
        assertion.is_true(G_holder.v)
        Return True
    End Function
End Class
