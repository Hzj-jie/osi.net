
Imports osi.root.connector
Imports osi.root.utils
Imports osi.root.utt

Public Class type_attribute_forward_test
    Inherits [case]

    <type_attribute(type_attribute.init_mode.must_be_set)>
    Private Class a
        Public Shared Sub set_type_property()
            type_attribute.of(Of a).set(guid_str())
        End Sub

        Shared Sub New()
            set_type_property()
        End Sub
    End Class

    <type_attribute(type_attribute.forward_mode.any)>
    Private Class b
        Public Shared Sub set_type_property()
            type_attribute.of(Of b).set(guid_str())
        End Sub

        Shared Sub New()
            type_attribute.of(Of b).forward_from(Of a)()
        End Sub
    End Class

    Shared Sub New()
        Dim x As a = Nothing
        x = New a()
        Dim y As b = Nothing
        y = New b()
    End Sub

    Private Shared Function run_case() As Boolean
        assertion.equal(type_attribute.of(Of a).get(Of String)(),
                     type_attribute.of(Of b).get(Of String)())
        Return True
    End Function

    Public Overrides Function run() As Boolean
        For i As Int32 = 0 To 15
            If Not run_case() Then
                Return False
            End If
            If i Mod 2 = 0 Then
                a.set_type_property()
            Else
                b.set_type_property()
            End If
        Next
        Return True
    End Function
End Class
