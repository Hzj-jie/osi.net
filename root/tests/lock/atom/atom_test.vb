
Imports osi.root.utt
Imports osi.root.lock

Public MustInherit Class atom_test
    Inherits [case]

    Protected Const thread_count As Int32 = 4
    Protected Const round As Int32 = 1000000

    Public NotOverridable Overrides Function reserved_processors() As Int16
        Return thread_count
    End Function

    Protected MustInherit Class atom_case
        Inherits [case]
        Public MustOverride Overrides Function run() As Boolean
    End Class

    Protected MustOverride Function create_case() As atom_case
    Protected MustOverride Sub validate(ByVal ac As atom_case)

    Public NotOverridable Overrides Function run() As Boolean
        Dim ac As atom_case = Nothing
        ac = create_case()
        Dim c As [case] = Nothing
        c = multithreading(repeat(ac, round), thread_count)
        If c.run() Then
            validate(ac)
            Return True
        Else
            Return False
        End If
    End Function
End Class
