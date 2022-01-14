
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class kick_between_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assertion.equal(kick_between("{abc}}", "{", "}"), "}")
        assertion.equal(kick_between("abc", Nothing, Nothing), "")
        assertion.equal(kick_between("bcd{def{abc}efg}hij", "{", "}"), "bcdhij")
        assertion.equal(kick_between("abc{def{hig}klm", "{", "}"), "abc")
        assertion.equal(kick_between("abc<script><scrip abcde << >> lsf</scrip></script>def",
                                  "<script>",
                                  "</script>"),
                     "abcdef")
        assertion.equal(kick_between("ABC<STyLE>ab sd <STYljsdo iasoj aosdfj {} []adf </stYle>BCD<sTyLe>def</StYlE>def",
                                  "<style>",
                                  "</style>",
                                  case_sensitive:=False),
                     "ABCBCDdef")

        assertion.equal(kick_between("abc{{{{{d}efg", "{", "}", recursive:=False), "abcefg")
        Return True
    End Function
End Class
