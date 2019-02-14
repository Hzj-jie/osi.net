﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class type_comparer_test
    <test>
    <repeat(10000)>
    Private Shared Sub int_case()
        Dim x As Int32 = 0
        Dim y As Int32 = 0
        x = rnd_int()
        y = rnd_int()
        assertion.equal(compare(x, y), type_comparer.compare(GetType(Int32), GetType(Int32), x, y))
    End Sub

    <test>
    <repeat(10000)>
    Private Shared Sub string_case()
        Dim x As String = Nothing
        Dim y As String = Nothing
        x = rnd_utf8_chars(rnd_int(10, 1000))
        y = rnd_utf8_chars(rnd_int(10, 1000))
        assertion.equal(compare(x, y), type_comparer.compare(GetType(String), GetType(String), x, y))
    End Sub

    Private Sub New()
    End Sub
End Class
