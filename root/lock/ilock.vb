
Imports osi.root.lock.slimlock

Public Interface ilock
    Inherits islimlock
    Function held_in_thread() As Boolean
    Function held() As Boolean
End Interface
