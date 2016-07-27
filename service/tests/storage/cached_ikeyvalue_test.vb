
Imports osi.service.storage

Public Class cached_ikeyvalue_test
    Inherits istrkeyvt_case

    Protected Overrides Function create_istrkeyvt() As istrkeyvt
        Return adapt(adapter.cached(New memory()))
    End Function
End Class
