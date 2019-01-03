
Imports osi.root.utt

Public Class weakreference_behavior_test
    Inherits [case]

    Public Overrides Function run() As Boolean
        Dim i As Int32 = 0
        Dim p As WeakReference = Nothing
        p = New WeakReference(i)
        i = 100
        assertion.equal(DirectCast(p.Target(), Int32), 0)
        p.Target() = 200
        assertion.equal(DirectCast(p.Target(), Int32), 200)
        assertion.equal(i, 100)
        Return True
    End Function
End Class
