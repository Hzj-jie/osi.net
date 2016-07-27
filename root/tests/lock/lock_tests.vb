
Imports osi.root.lock
Imports osi.root.utt

Public Class lock_eventlock_test
    Inherits ilock_test(Of lock(Of slimlock.eventlock))
End Class

Public Class lock_lazylock_test
    Inherits ilock_test(Of lock(Of slimlock.lazylock))
End Class

Public Class lock_monitorlock_islimlock_test
    Inherits ilock_test(Of lock(Of slimlock.monitorlock))
End Class

Public Class monitorlock_ilock_test
    Inherits ilock_test(Of monitorlock)
End Class

Public Class lock_sequentiallock_test
    Inherits ilock_test(Of lock(Of slimlock.sequentiallock))

    Public Sub New()
        MyBase.New(True)
    End Sub
End Class

Public Class lock_simplelock_test
    Inherits ilock_test(Of lock(Of slimlock.simplelock))
End Class

Public Class lock_spinlock_test
    Inherits ilock_test(Of lock(Of slimlock.spinlock))

    Public Sub New()
        MyBase.New(True)
    End Sub
End Class

Public Class lock_spinlock2_test
    Inherits ilock_test(Of lock(Of slimlock.spinlock2))

    Public Sub New()
        MyBase.New(True)
    End Sub
End Class
