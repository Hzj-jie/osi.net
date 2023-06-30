
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
        While exp = def
            exp = rnd_int()
        End While
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
        assertion.equal(v, def)
        assertion.is_not_null(static_constructor.retrieve(GetType(C)))
        static_constructor.as_action(GetType(C))()
        assertion.equal(v, exp)
        static_constructor.execute(GetType(C))
        assertion.equal(v, def)

        assertion.is_null(static_constructor(Of D).retrieve())
        assertion.is_null(static_constructor(Of D).as_action())
        static_constructor(Of D).execute()

        static_constructor(Of E).execute()
        static_constructor(Of E).execute()

        assertion.is_false(F_holder.v)
        static_constructor(Of F).execute()
        assertion.is_true(F_holder.v)

        assertion.is_false(G_holder.v)
        static_constructor(Of G).execute()
        assertion.is_true(G_holder.v)
        Return True
    End Function
End Class
