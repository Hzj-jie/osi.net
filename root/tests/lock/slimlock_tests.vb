
Imports osi.root.lock
Imports osi.root.lock.slimlock
Imports osi.root.utt

Public Class eventlock_test
    Inherits islimlock_test(Of eventlock)
End Class

Public Class lazylock_test
    Inherits islimlock_test(Of lazylock)
End Class

Public Class monitorlock_islimlock_test
    Inherits islimlock_test(Of slimlock.monitorlock)
End Class

Public Class sequentiallock_test
    Inherits islimlock_test(Of slimlock.sequentiallock)

    Public Sub New()
        MyBase.New(True)
    End Sub
End Class

Public Class simplelock_test
    Inherits islimlock_test(Of simplelock)
End Class

Public Class spinlock_test
    Inherits islimlock_test(Of spinlock)

    Public Sub New()
        MyBase.New(True)
    End Sub
End Class

Public Class spinlock2_test
    Inherits islimlock_test(Of spinlock2)

    Public Sub New()
        MyBase.New(True)
    End Sub
End Class
