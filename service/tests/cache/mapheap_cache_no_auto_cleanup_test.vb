
Imports osi.root.constants
Imports osi.root.utt
Imports osi.service.cache

Public Class mapheap_cache_no_auto_cleanup_test
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New cache_test(mapheap_cache(Of String, Byte())(max_uint64,
                                                                   constants.mapheap_cache.no_retire_ticks),
                                  True))
    End Sub
End Class
