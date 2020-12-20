
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class json_serializer_test
    <test>
    Private Shared Sub predefined_types()
        assertion.equal(json_serializer.to_str("ABC"), """ABC""")
        assertion.equal(json_serializer.to_str(True), "True")
        assertion.equal(json_serializer.to_str(100), "100")
        assertion.equal(json_serializer.to_str(100.11), "100.11")
    End Sub

    <test>
    Private Shared Sub type_cases()
        assertion.equal(type_json_serializer.r.to_str("ABC"), """ABC""")
        assertion.equal(type_json_serializer.r.to_str(True), "True")
        assertion.equal(type_json_serializer.r.to_str(100), "100")
        assertion.equal(type_json_serializer.r.to_str(100.11), "100.11")
    End Sub

    <test>
    Private Shared Sub arrays()
        assertion.equal(json_serializer.to_str({"ABC", "BCD"}), "[""ABC"",""BCD""]")
        assertion.equal(json_serializer.to_str({True, False}), "[True,False]")
        assertion.equal(json_serializer.to_str({100, 200}), "[100,200]")
        assertion.equal(json_serializer.to_str({100.11, 100.12}), "[100.11,100.12]")
    End Sub

    <test>
    Private Shared Sub mixed_arrays()
        assertion.equal(json_serializer.to_str(New Object() {"ABC", True, 100, 100.12}),
                        "[""ABC"",True,100,100.12]")
    End Sub

    <test>
    Private Shared Sub type_arrays()
        assertion.equal(type_json_serializer.r.to_str({"ABC", "BCD"}), "[""ABC"",""BCD""]")
        assertion.equal(type_json_serializer.r.to_str({True, False}), "[True,False]")
        assertion.equal(type_json_serializer.r.to_str({100, 200}), "[100,200]")
        assertion.equal(type_json_serializer.r.to_str({100.11, 100.12}), "[100.11,100.12]")
    End Sub

    <test>
    Private Shared Sub type_mixed_arrays()
        assertion.equal(type_json_serializer.r.to_str(New Object() {"ABC", True, 100, 100.12}),
                        "[""ABC"",True,100,100.12]")
    End Sub

    Private Sub New()
    End Sub
End Class
