
Imports osi.root.utt
Imports osi.service.cache

Public Class hashmap_cache_test
    Inherits case_wrapper

    Public Sub New()
        MyBase.New(New cache_test(hashmap_cache(Of String, Byte())(), True))
    End Sub
End Class
