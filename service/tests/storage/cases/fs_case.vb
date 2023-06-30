
Imports osi.root.procedure
Imports osi.root.utt
Imports osi.service.storage

Public Interface ifs_case

End Interface

'MustInherit for utt
Public MustInherit Class fs_case
    Inherits event_comb_case

    Public Overrides Function create() As event_comb

    End Function
End Class