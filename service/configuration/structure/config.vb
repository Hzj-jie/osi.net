
Imports osi.root.formation

Friend Class raw_config
    Inherits filtered_raw_value(Of section)
End Class

Public Class config
    Inherits filtered_value(Of section)

    Friend Sub New(ByVal fs As filter_selector, ByVal m As raw_config)
        MyBase.New(fs, m)
    End Sub
End Class
