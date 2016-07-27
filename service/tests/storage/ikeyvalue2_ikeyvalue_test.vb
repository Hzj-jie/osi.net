
Imports osi.service.storage

Public Class ikeyvalue2_ikeyvalue_test
    Inherits istrkeyvt_case

    Protected Overrides Function create_istrkeyvt() As istrkeyvt
        Return adapt(adapter.isynckeyvalue2_ikeyvalue2(New memory2()))
    End Function
End Class
