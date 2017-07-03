
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants
Imports osi.service.cache

Public Class mapheap_cache_test2
    Inherits cache_test2

    Public Sub New()
        MyBase.New(mapheap_slimecache2(Of Int32, Int32)(max_uint64, constants.mapheap_cache.no_retire_ticks))
    End Sub
End Class
