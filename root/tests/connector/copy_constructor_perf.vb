
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class copy_constructor_perf
    Inherits performance_comparison_case_wrapper

    Private Shared Function R(ByVal c As [case]) As [case]
        Return repeat(c, 1024 * 1024)
    End Function

    Public Sub New()
        MyBase.New(R(New copy_constructor_case()), R(New new_case()))
    End Sub

    Protected Overrides Function min_rate_upper_bound(ByVal i As UInt32, ByVal j As UInt32) As Double
        Return loosen_bound({1041, 860}, i, j)
    End Function

    Private Class test_class
        Private ReadOnly a As String
        Private ReadOnly b As UInt32
        Private ReadOnly c As Object

        Public Sub New(ByVal a As String, ByVal b As UInt32, ByVal c As Object, ByVal d As Boolean)
            Me.New(a, b, c)
        End Sub

        <copy_constructor()>
        Private Sub New(ByVal a As String, ByVal b As UInt32, ByVal c As Object)
            Me.a = a
            Me.b = b
            Me.c = c
        End Sub
    End Class

    Private Class copy_constructor_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim o As test_class = Nothing
            o = copy_constructor(Of test_class).invoke(rnd_utf8_chars(100), rnd_uint(), New Object())
            Return True
        End Function
    End Class

    Private Class new_case
        Inherits [case]

        Public Overrides Function run() As Boolean
            Dim o As test_class = Nothing
            o = New test_class(rnd_utf8_chars(100), rnd_uint(), New Object(), True)
            Return True
        End Function
    End Class
End Class
