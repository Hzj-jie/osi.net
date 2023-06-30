
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class generic_type_behavior_test
    Private Interface inf(Of T)
    End Interface

    <test>
    Private Shared Sub generic_type_definition_case()
        assertion.is_true(GetType(inf(Of )).IsGenericType())
        assertion.is_true(GetType(inf(Of )).IsGenericTypeDefinition())
        assertion.is_true(GetType(inf(Of Int32)).IsGenericType())
        assertion.is_false(GetType(inf(Of Int32)).IsGenericTypeDefinition())
    End Sub

    Private Sub New()
    End Sub
End Class
