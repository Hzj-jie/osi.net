
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.service.cache

Public Class unordered_map_cache_test
    Inherits cache_test

    Public Sub New()
        MyBase.New(unordered_map_cache(Of String, Byte())(), True)
    End Sub
End Class

Public Class unordered_map_cache_test2
    Inherits cache_test2

    Public Sub New()
        MyBase.New(unordered_map_cache(Of Int32, Int32)())
    End Sub
End Class