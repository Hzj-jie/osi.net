
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.storage

Public Class fces_ondisk_test
    Inherits temp_drive_istrkeyvt_case

    Public Sub New(ByVal i As iistrkeyvt_case)
        MyBase.New(i)
    End Sub

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Function create_valid_istrkeyvt(ByVal p As pointer(Of istrkeyvt)) As event_comb
        Return fces.ctor(p, IO.Path.Combine(data_dir, guid_str()))
    End Function
End Class

Public Class fces_ondisk_specified_test
    Inherits commandline_specified_case_wrapper

    Public Sub New()
        MyBase.New(New fces_ondisk_test(New default_istrkeyvt_case2()))
    End Sub
End Class
