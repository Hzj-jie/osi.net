
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.storage

Public Class isynckeyvalue2_isynckeyvalue_test
    Inherits istrkeyvt_case

    Protected Overrides Function create_istrkeyvt() As istrkeyvt
        Return memory2.ctor()
    End Function
End Class
