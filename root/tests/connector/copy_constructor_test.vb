
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class copy_constructor_test
    Inherits [case]

    Private Class test_class
        <copy_constructor()>
        Private Sub New()
        End Sub
    End Class

    Private Class test_class2
        Public ReadOnly s As String
        Public ReadOnly i As UInt32
        Public ReadOnly o As Object

        <copy_constructor()>
        Protected Sub New(ByVal s As String, ByVal i As UInt32, ByVal o As Object)
            Me.s = s
            Me.i = i
            Me.o = o
        End Sub
    End Class

    Private Shared Function test_class_case() As Boolean
        Dim x As test_class = Nothing
        x = copy_constructor(Of test_class).invoke()
        assert_not_nothing(x)
        Return True
    End Function

    Private Shared Function test_class2_case() As Boolean
        For j As UInt32 = 0 To 100
            Dim s As String = Nothing
            Dim i As UInt32 = 0
            Dim o As Object = Nothing
            s = guid_str()
            i = rnd_uint()
            o = New Object()
            Dim x As test_class2 = Nothing
            x = copy_constructor(Of test_class2).invoke(s, i, o)
            assert_not_nothing(x)
            assert_equal(s, x.s)
            assert_equal(i, x.i)
            assert_equal(o, x.o)
        Next
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return test_class_case() AndAlso
               test_class2_case()
    End Function
End Class
