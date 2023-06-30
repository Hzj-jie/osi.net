
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.envs
Imports osi.root.template
Imports osi.root.utt
Imports osi.service.storage

Public NotInheritable Class file_key_test
    Inherits temp_drive_istrkeyvt_case

    Private NotInheritable Class should_use_low_res_timestamp
        Inherits _boolean

        Protected Overrides Function at() As Boolean
            Return vmware_virtual_machine
        End Function
    End Class

    Public Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(i)
    End Sub

    Public Sub New()
        MyBase.New(New default_istrkeyvt_case(Of should_use_low_res_timestamp)())
    End Sub

    Protected Overrides Function create_valid_istrkeyvt() As istrkeyvt
        Return file_key.ctor(data_dir)
    End Function
End Class

Public NotInheritable Class file_key_specified_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New file_key_test(New default_istrkeyvt_case2()))
    End Sub
End Class