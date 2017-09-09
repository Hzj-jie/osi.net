
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.formation

Friend Class raw_config
    Inherits filtered_raw_value(Of section)
End Class

Public Class config
    Inherits filtered_value(Of section)

    Friend Sub New(ByVal fs As filter_selector, ByVal m As raw_config)
        MyBase.New(fs, m)
    End Sub

    Default Public Shadows ReadOnly Property v(
             ByVal key As String,
             Optional ByVal variants As vector(Of pair(Of String, String)) = Nothing) As sections
        Get
            Return New sections([get](key, variants))
        End Get
    End Property
End Class
