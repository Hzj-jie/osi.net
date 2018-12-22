
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.utt
Imports osi.root.utt.attributes

Friend Module _extension_behavior_test
    <Extension()> Public Function execute(ByVal i As Int32) As Int32
        Return i + 1
    End Function

    <Extension()> Public Function execute(ByVal i As Object) As Int32
        Return 1
    End Function

    <Extension()> Public Function execute(ByVal i As Boolean) As Int32
        Return 2
    End Function

    <Extension()> Public Function execute(Of T)(ByVal i As T) As Int32
        Return 3
    End Function

    <Extension()> Public Function execute2(ByVal i As Object) As Int32
        Return 4
    End Function
End Module

<test>
Public NotInheritable Class extension_behavior_test
    <test>
    Private Shared Sub extension_selection_test()
        assert_equal(100.execute(), 101)
        assert_equal(True.execute(), 2)
        assert_equal("".execute(), 3)
        assert_equal(1.0.execute(), 3)
    End Sub

    <test>
    Private Shared Sub object_extension_test()
        assert_equal(100.execute2(), 4)
        assert_equal(True.execute2(), 4)
        assert_equal("".execute2(), 4)
        assert_equal(1.0.execute2(), 4)
    End Sub

    Private Sub New()
    End Sub
End Class
