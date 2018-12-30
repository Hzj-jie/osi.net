
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class inherit_test2
    Private Interface inf(Of T)
    End Interface

    Private Interface inf2
        Inherits inf(Of Object)
    End Interface

    Private Interface inf3(Of T)
        Inherits inf(Of T)
    End Interface

    Private Class impl
        Implements inf(Of Object)
    End Class

    Private Class impl2(Of T)
        Implements inf(Of T)
    End Class

    Private Class impl3(Of T)
        Inherits impl2(Of T)
        Implements inf2
    End Class

    <test>
    Private Shared Sub interface_inherit_case()
        assert_true(GetType(inf2).interface_inherit(GetType(inf(Of Object))))
        assert_true(GetType(inf2).interface_inherit(GetType(inf(Of ))))
        assert_true(GetType(inf3(Of Object)).interface_inherit(GetType(inf(Of Object))))
        assert_true(GetType(inf3(Of Object)).interface_inherit(GetType(inf(Of ))))
        assert_true(GetType(inf3(Of )).interface_inherit(GetType(inf(Of ))))

        assert_true(GetType(inf2).inherit(GetType(inf(Of Object))))
        assert_true(GetType(inf2).inherit(GetType(inf(Of ))))
        assert_true(GetType(inf3(Of Object)).inherit(GetType(inf(Of Object))))
        assert_true(GetType(inf3(Of Object)).inherit(GetType(inf(Of ))))
        assert_true(GetType(inf3(Of )).inherit(GetType(inf(Of ))))
    End Sub

    <test>
    Private Shared Sub implement_case()
        assert_true(GetType(impl).implement(GetType(inf(Of Object))))
        assert_true(GetType(impl).implement(GetType(inf(Of ))))
        assert_true(GetType(impl2(Of Object)).implement(GetType(inf(Of Object))))
        assert_true(GetType(impl2(Of Object)).implement(GetType(inf(Of ))))
        assert_true(GetType(impl2(Of )).implement(GetType(inf(Of ))))
        assert_true(GetType(impl3(Of String)).implement(GetType(inf2)))
        assert_true(GetType(impl3(Of String)).implement(GetType(inf(Of Object))))
        assert_true(GetType(impl3(Of String)).implement(GetType(inf(Of ))))
        assert_true(GetType(impl3(Of )).implement(GetType(inf2)))
        assert_true(GetType(impl3(Of )).implement(GetType(inf(Of Object))))
        assert_true(GetType(impl3(Of )).implement(GetType(inf(Of ))))
    End Sub

    <test>
    Private Shared Sub inherit_case()
        assert_true(GetType(impl3(Of String)).inherit(GetType(impl2(Of String))))
        assert_true(GetType(impl3(Of String)).inherit(GetType(impl2(Of ))))
        assert_true(GetType(impl3(Of )).inherit(GetType(impl2(Of ))))
    End Sub

    Private Sub New()
    End Sub
End Class
