
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.storage

Public NotInheritable Class file_index_test
    Inherits temp_drive_istrkeyvt_case

    Public Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(i)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Function create_valid_istrkeyvt(ByVal p As ref(Of istrkeyvt)) As event_comb
        Return file_index.ctor(p, data_dir)
    End Function
End Class

Public NotInheritable Class file_index_specified_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New file_index_test(New short_key_istrkeyvt_case2()))
    End Sub
End Class