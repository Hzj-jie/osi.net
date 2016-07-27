
Namespace slimlock
    Public Interface islimlock
        Sub wait()
        Sub release()
        Function can_thread_owned() As Boolean
        Function can_cross_thread() As Boolean
    End Interface
End Namespace
