
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.utt

' Custom attributes should be able to be inherited by the classes which declared the attributes.
Public Class custom_attributes_behavior_test
    Inherits [case]

    Private Class test
        Inherits Attribute

        Public ReadOnly v As Int32

        Public Sub New(ByVal v As Int32)
            Me.v = v
        End Sub
    End Class

    <test(1)>
    Private Class C1
    End Class

    Private Class C2
        Inherits C1
    End Class

    <test(2)>
    Private Class C3
        Inherits C1
    End Class

    <test(3)>
    Private Interface I
    End Interface

    Private Class C4
        Implements I
    End Class

    <test(4)>
    Private Class C5
        Implements I
    End Class

    Private Shared Function run_case(Of T)(Optional has As Boolean = True,
                                           Optional ByVal inherit As Boolean = False,
                                           Optional ByVal v As Int32 = 0) As Boolean
        Dim objs() As Object = Nothing
        objs = GetType(T).GetCustomAttributes(inherit)
        Dim c As UInt32 = 0
        If Not has OrElse assert_more_or_equal(array_size(objs), uint32_1) Then
            Dim i As UInt32 = uint32_0
            While i < array_size(objs)
                Dim x As test = Nothing
                If direct_cast(objs(CInt(i)), x) Then
                    assert_equal(x.v, v)
                    c += uint32_1
                End If
                i += uint32_1
            End While
            assert_equal(If(has, uint32_1, uint32_0), c)
        End If
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return run_case(Of C1)(True, False, 1) AndAlso
               run_case(Of C2)(False, False) AndAlso
               run_case(Of C2)(True, True, 1) AndAlso
               run_case(Of C3)(True, False, 2) AndAlso
               run_case(Of C3)(True, True, 2) AndAlso
               run_case(Of I)(True, False, 3) AndAlso
               run_case(Of C4)(False, False) AndAlso
               run_case(Of C4)(False, True) AndAlso
               run_case(Of C5)(True, False, 4) AndAlso
               run_case(Of C5)(True, True, 4)
    End Function
End Class
