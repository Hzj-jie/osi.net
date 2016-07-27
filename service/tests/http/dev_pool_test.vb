
Imports osi.root.connector
Imports osi.root.utt

Public Class dev_pool_test
    Inherits multi_procedure_case_wrapper

    Public Sub New()
        MyBase.New(repeat(New dev_pool_single_test.dev_pool_case(max(Environment.ProcessorCount() >> 1, 2)), 4096),
                   max(Environment.ProcessorCount() << 2, 8))
    End Sub

    Public Overrides Function preserved_processors() As Int16
        Return Environment.ProcessorCount()
    End Function
End Class
