
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class inherit_test
    Inherits [case]

    Private Interface inf
    End Interface

    Private Interface inf2
        Inherits inf
    End Interface

    Private Class base
        Implements inf
    End Class

    Private Class base2
    End Class

    Private Class inh
        Inherits base
    End Class

    Private Class inh2
        Inherits base2
    End Class

    Private Class inh3
        Inherits inh
    End Class

    Public Overrides Function run() As Boolean
        assert_false(GetType(inf).implement(GetType(inf)))
        assert_false(GetType(inf).inherit(GetType(inf)))

        assert_false(GetType(inf2).implement(GetType(inf)))
        assert_true(GetType(inf2).inherit(GetType(inf)))

        assert_true(GetType(base).implement(Of inf)())
        assert_false(GetType(base2).implement(Of inf)())

        assert_true(GetType(inh).inherit(Of base)())
        assert_true(GetType(inh).implement(Of inf)())
        assert_false(GetType(inh).inherit(Of inf)())
        assert_false(GetType(inh).implement(Of base)())

        assert_true(GetType(inh2).inherit(Of base2)())
        assert_false(GetType(inh2).implement(Of inf)())
        assert_false(GetType(inh2).inherit(Of base)())
        assert_false(GetType(inh2).implement(Of base2)())

        assert_true(GetType(inh3).inherit(Of inh)())
        assert_true(GetType(inh3).inherit(Of base)())
        assert_true(GetType(inh3).implement(Of inf)())
        assert_false(GetType(inh3).implement(Of base)())
        assert_false(GetType(inh3).implement(Of inh)())
        assert_false(GetType(inh3).inherit(Of base2)())
        assert_false(GetType(inh3).inherit(Of inh2)())

        Return True
    End Function
End Class
