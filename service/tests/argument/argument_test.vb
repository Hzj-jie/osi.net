
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.service.argument

Public NotInheritable Class argument_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim v As var = Nothing
        v = New var()
        v.parse("--a=bcd -ijk ~op --b=def --de=efg ghghgh --hi")
        assertion.is_true(v.bind("abc", "bcd", "def", "opq"))
        assertion.is_true(v.switch("i"))
        assertion.is_true(v.switch("j"))
        assertion.is_true(v.switch("k"))
        assertion.is_true(v.switch("opq"))
        assertion.is_true(v.switch("hi"))
        assertion.equal(v.value("abc"), "bcd")
        assertion.equal(v.value("bcd"), "def")
        assertion.equal(v.value("def"), "efg")
        If assertion.equal(v.other_values().size(), CUInt(1)) Then
            assertion.equal(v.other_values()(0), "ghghgh")
        End If
        Return True
    End Function
End Class
