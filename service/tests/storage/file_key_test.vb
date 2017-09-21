
Imports osi.root.template
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.service.storage
Imports osi.root.utt

Public Class file_key_test
    Inherits temp_drive_istrkeyvt_case

    Public Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(i)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Function create_valid_istrkeyvt() As istrkeyvt
        Return file_key.ctor(data_dir)
    End Function
End Class

Public Class file_key_specific_test
    Inherits commandline_specified_event_comb_case_wrapper

    Public Sub New()
        MyBase.New(New file_key_test(New default_istrkeyvt_case2()))
    End Sub
End Class