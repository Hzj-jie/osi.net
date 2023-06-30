
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.utt
Imports osi.root.utt.attributes

<test>
Public NotInheritable Class enum_to_string_behavior_test
    Private Enum E As Byte
        a
        b
        c
    End Enum

    Private Shared Function convert_to_string(Of T)(ByVal x As T) As String
        Return Convert.ToString(x)
    End Function

    Private Shared Function object_to_string(Of T)(ByVal x As T) As String
        Return x.ToString()
    End Function

    Private Shared Function convert_to_string_obj(ByVal x As Object) As String
        Return Convert.ToString(x)
    End Function

    Private Shared Function object_to_string_obj(ByVal x As Object) As String
        Return x.ToString()
    End Function

    <test>
    Private Shared Sub to_string_vs_convert_to_string()
        assertions.of(E.a.ToString()).Equals("a")
        assertions.of(Convert.ToString(E.a)).Equals("0")
        assertions.of(object_to_string(E.a)).Equals("a")
        assertions.of(convert_to_string(E.a)).Equals("0")
        assertions.of(object_to_string_obj(E.a)).Equals("a")
        assertions.of(convert_to_string_obj(E.a)).Equals("0")
    End Sub

    Private Sub New()
    End Sub
End Class
