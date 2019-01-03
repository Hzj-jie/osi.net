
Imports osi.root.connector
Imports osi.root.utt

Public Class static_constructor_test
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
        Return True
    End Function
End Class
