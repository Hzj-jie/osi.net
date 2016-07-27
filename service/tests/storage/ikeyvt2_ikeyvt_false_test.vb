
Imports osi.root.formation
Imports osi.service.storage

Public Class ikeyvt2_ikeyvt_false_test
    Inherits istrkeyvt_case

    Protected Overrides Function create_istrkeyvt() As istrkeyvt
        Return adapt(adapter.ikeyvalue2_ikeyvt2(adapter.isynckeyvalue2_ikeyvalue2(New memory2()),
                                                adapter.isynckeyvalue2_ikeyvalue2(New memory2())))
    End Function
End Class
