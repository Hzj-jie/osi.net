
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.utt

Public Class nothing_test
    Inherits [case]

    Private Structure test_structure
    End Structure

    Private Interface test_interface
    End Interface

    Private Structure test_structure2
        Implements test_interface
    End Structure

    Private Shared Function is_nothing_object(ByVal i As Object) As Boolean
        Return i Is Nothing
    End Function

    Private Shared Function is_nothing(Of T)(ByVal i As T) As Boolean
        If i Is Nothing Then
            assert_true(is_nothing_object(i))
            Return True
        Else
            assert_false(is_nothing_object(i))
            Return False
        End If
    End Function

    Private Shared Sub is_nothing_test(Of T)()
        Dim x As T = Nothing
        assert_true(is_nothing(x))
        assert_true(x Is Nothing)
    End Sub

    Private Shared Sub is_not_nothing_test(Of T)()
        Dim x As T = Nothing
        assert_false(is_nothing(x))
        assert_false(x Is Nothing)
    End Sub

    Private Shared Function value_type_is_not_nothing() As Boolean
        is_not_nothing_test(Of Int16)()
        is_not_nothing_test(Of Int32)()
        is_not_nothing_test(Of Double)()
        is_not_nothing_test(Of UInt32)()
        Using code_block
            Dim i As Int32 = 0
            assert_true(type_info(Of Int32).is_valuetype)
            i = Nothing
            assert_false(is_nothing(i))
            i = 100
            assert_false(is_nothing(i))
            i = alloc(Of Int32)()
            assert_false(is_nothing(i))
        End Using
        Using code_block
            Dim i As Double = 0
            assert_true(type_info(Of Double).is_valuetype)
            i = Nothing
            assert_false(is_nothing(i))
            i = 100
            assert_false(is_nothing(i))
            i = alloc(Of Double)()
            assert_false(is_nothing(i))
        End Using
        Return True
    End Function

    ' A structure should always be value_type.
    Private Shared Function structure_is_not_nothing() As Boolean
        is_not_nothing_test(Of test_structure)()
        Using code_block
            Dim i As test_structure = Nothing
            assert_true(type_info(Of test_structure).is_valuetype)
            i = Nothing
            assert_false(is_nothing(i))
            i = New test_structure()
            assert_false(is_nothing(i))
            i = alloc(Of test_structure)()
            assert_false(is_nothing(i))
        End Using
        Return True
    End Function

    Private Shared Function reference_type_is_nullable() As Boolean
        is_nothing_test(Of String)()
        Using code_block
            Dim x As String = Nothing
            assert_false(type_info(Of String).is_valuetype)
            assert_true(is_nothing(x))
            assert_true(x Is Nothing)
            x = String.Empty
            assert_false(is_nothing(x))
            assert_false(x Is Nothing)
        End Using
        Return True
    End Function

    Private Shared Function interface_is_nullable() As Boolean
        is_nothing_test(Of test_interface)()
        Using code_block
            Dim i As test_structure2 = Nothing
            assert_true(type_info(Of test_structure2).is_valuetype)
            assert_false(type_info(Of test_interface).is_valuetype)
            assert_false(is_nothing(i))
            assert_false(is_nothing(Of test_interface)(i))
            assert_true(is_nothing(Of test_interface)(Nothing))
            Dim a As test_interface = Nothing
            assert_true(is_nothing(a))
            a = i
            assert_false(is_nothing(a))

            i = New test_structure2()
            assert_false(is_nothing(i))
            assert_false(is_nothing(Of test_interface)(i))
            i = direct_cast(Of test_structure2)(alloc(Of test_interface)())
            assert_false(is_nothing(i))
            assert_false(is_nothing(Of test_interface)(i))
            i = alloc(Of test_structure2)()
            assert_false(is_nothing(i))
            assert_false(is_nothing(Of test_interface)(i))
        End Using
        Return True
    End Function

    Private Shared Function array_is_nullable() As Boolean
        is_nothing_test(Of Int32())()
        Using code_block
            Dim a() As Int32 = Nothing
            assert_false(type_info(Of Int32()).is_valuetype)
            assert_true(is_nothing(a))
            ReDim a(-1)
            assert_false(is_nothing(a))
            ReDim a(0)
            assert_false(is_nothing(a))
            a = Nothing
            assert_true(is_nothing(a))
        End Using
        is_nothing_test(Of test_interface())()
        Using code_block
            Dim a() As test_interface = Nothing
            assert_false(type_info(Of test_interface()).is_valuetype)
            assert_true(is_nothing(a))
            ReDim a(-1)
            assert_false(is_nothing(a))
            ReDim a(0)
            assert_false(is_nothing(a))
            a = Nothing
            assert_true(is_nothing(a))
        End Using
        is_nothing_test(Of String())()
        Using code_block
            Dim a() As String = Nothing
            assert_false(type_info(Of String()).is_valuetype)
            assert_true(is_nothing(a))
            ReDim a(-1)
            assert_false(is_nothing(a))
            ReDim a(0)
            assert_false(is_nothing(a))
            a = Nothing
            assert_true(is_nothing(a))
        End Using
        Return True
    End Function

    Public Overrides Function run() As Boolean
        Return value_type_is_not_nothing() AndAlso
               reference_type_is_nullable() AndAlso
               structure_is_not_nothing() AndAlso
               interface_is_nullable() AndAlso
               array_is_nullable()
    End Function
End Class
