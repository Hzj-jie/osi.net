
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.formation

Namespace logic
    Partial Public NotInheritable Class scope
        Public Class anchor_ref
            Public ReadOnly name As String
            Public ReadOnly return_type As String
            Public ReadOnly parameters As const_array(Of builders.parameter)

            Public Sub New(ByVal name As String,
                           ByVal return_type As String,
                           ByVal parameters() As builders.parameter)
                assert(Not name.null_or_whitespace())
                assert(Not return_type.null_or_whitespace())
                Me.name = name
                Me.return_type = return_type
                Me.parameters = const_array.of(parameters)
            End Sub
        End Class

        Private NotInheritable Class anchor_refs

        End Class

        Public Structure anchor_ref_proxy

        End Structure
    End Class
End Namespace
