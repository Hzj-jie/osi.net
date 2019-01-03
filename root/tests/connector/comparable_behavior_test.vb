
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt

Public Class comparable_behavior_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        assertion.thrown(Sub()
                             CInt(1).CompareTo("abc")
                         End Sub)
        assertion.thrown(Sub()
                             CStr("abc").CompareTo(1)
                         End Sub)
        assertion.thrown(Sub()
                             CInt(1).CompareTo(True)
                         End Sub)
        assertion.thrown(Sub()
                             CInt(1).CompareTo(1.1)
                         End Sub)
        assertion.thrown(Sub()
                             CInt(1).CompareTo(CUInt(1))
                         End Sub)
        assertion.thrown(Sub()
                             CBool(True).CompareTo(1)
                         End Sub)
        assertion.more(CInt(1).CompareTo(CObj(Nothing)), 0)
        assertion.more(CStr("abc").CompareTo(CObj(Nothing)), 0)
        assertion.equal(CInt(1).CompareTo(CObj(1)), 0)
        assertion.less(CInt(1).CompareTo(CObj(100)), 0)
        Return True
    End Function
End Class
