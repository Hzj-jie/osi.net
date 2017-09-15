
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt

Public Class comparable_behavior_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assert_throw(Sub()
                         CInt(1).CompareTo("abc")
                     End Sub)
        assert_throw(Sub()
                         CStr("abc").CompareTo(1)
                     End Sub)
        assert_throw(Sub()
                         CInt(1).CompareTo(True)
                     End Sub)
        assert_throw(Sub()
                         CInt(1).CompareTo(1.1)
                     End Sub)
        assert_throw(Sub()
                         CInt(1).CompareTo(CUInt(1))
                     End Sub)
        assert_throw(Sub()
                         CBool(True).CompareTo(1)
                     End Sub)
        assert_more(CInt(1).CompareTo(CObj(Nothing)), 0)
        assert_more(CStr("abc").CompareTo(CObj(Nothing)), 0)
        assert_equal(CInt(1).CompareTo(CObj(1)), 0)
        assert_less(CInt(1).CompareTo(CObj(100)), 0)
        Return True
    End Function
End Class
