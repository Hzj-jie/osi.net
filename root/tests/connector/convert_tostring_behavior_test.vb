
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class convert_tostring_behavior_test
    Inherits [case]

    Private Shared Function to_string_null_case() As Boolean
        assertion.equal(Convert.ToString(direct_cast(Of Object)(Nothing)), String.Empty)
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return to_string_null_case()
    End Function
End Class
