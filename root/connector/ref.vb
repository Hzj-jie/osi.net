
'according to generic_perf test,
'the performance to directly access the p should be on-par with other kind of wrapper logic
' This class is to convert a value type into a reference type, so one value type instance can be passed into a lambda
' expression, which is useful when using slimlock / lock / singleentry / forks, etc with event_comb.
' This class is intended to be a simple class, if comparing / cloning are required, use pointer.
Public Class ref(Of T As Structure)
    Public p As T
End Class
