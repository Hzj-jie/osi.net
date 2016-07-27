
Imports osi.root.connector
Imports osi.root.utt

Public Class kick_between_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assert_equal(kick_between("{abc}}", "{", "}"), "}")
        assert_equal(kick_between("abc", Nothing, Nothing), String.Empty)
        assert_equal(kick_between("bcd{def{abc}efg}hij", "{", "}"), "bcdhij")
        assert_equal(kick_between("abc{def{hig}klm", "{", "}"), "abc")
        assert_equal(kick_between("abc<script><scrip abcde << >> lsf</scrip></script>def",
                                  "<script>",
                                  "</script>"),
                     "abcdef")
        assert_equal(kick_between("ABC<STyLE>ab sd <STYljsdo iasoj aosdfj {} []adf </stYle>BCD<sTyLe>def</StYlE>def",
                                  "<style>",
                                  "</style>",
                                  False),
                     "ABCBCDdef")
        Return True
    End Function
End Class
