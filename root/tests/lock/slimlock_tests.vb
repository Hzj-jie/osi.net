
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.lock
Imports osi.root.lock.slimlock

Public NotInheritable Class eventlock_test
    Inherits islimlock_test(Of eventlock)

    Public Sub New()
        MyBase.New(True)
    End Sub
End Class

Public NotInheritable Class lazylock_test
    Inherits islimlock_test(Of lazylock)
End Class

Public NotInheritable Class monitorlock_islimlock_test
    Inherits islimlock_test(Of slimlock.monitorlock)
End Class

Public NotInheritable Class sequentiallock_test
    Inherits islimlock_test(Of slimlock.sequentiallock)

    Public Sub New()
        MyBase.New(True)
    End Sub
End Class

Public NotInheritable Class simplelock_test
    Inherits islimlock_test(Of simplelock)
End Class

Public NotInheritable Class spinlock_test
    Inherits islimlock_test(Of spinlock)

    Public Sub New()
        MyBase.New(True)
    End Sub
End Class

Public NotInheritable Class spinlock2_test
    Inherits islimlock_test(Of spinlock2)

    Public Sub New()
        MyBase.New(True)
    End Sub
End Class
