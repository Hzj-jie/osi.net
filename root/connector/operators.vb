
Imports osi.root.constants

Public Class accumulatable(Of T)
    Public Shared ReadOnly v As Boolean
    Public Shared ReadOnly ex As Exception

    Shared Sub New()
        Dim i As Object = Nothing
        Dim j As Object = Nothing
        i = alloc(Of T)()
        j = alloc(Of T)()
        Try
            Dim k As T = Nothing
            k = CType(i + j, T)
            accumulatable(Of T).v = True
        Catch ex As Exception
            accumulatable(Of T).ex = ex
            accumulatable(Of T).v = False
        End Try
    End Sub

    Private Sub New()
    End Sub
End Class

<global_init(global_init_level.foundamental)>
Friend Module operators_binder_register
    Sub New()
        bind_default_binary_operator_add()
        bind_default_binary_operator_minus()
    End Sub

    Private Sub init()
    End Sub

    Private Sub bind_default_binary_operator_minus()
        binder(Of Func(Of SByte, SByte, SByte), binary_operator_minus_protector).set_global(
            Function(x As SByte, y As SByte) As SByte
                Return x - y
            End Function)

        binder(Of Func(Of Byte, Byte, Byte), binary_operator_minus_protector).set_global(
            Function(x As Byte, y As Byte) As Byte
                Return x - y
            End Function)

        binder(Of Func(Of Int16, Int16, Int16), binary_operator_minus_protector).set_global(
            Function(x As Int16, y As Int16) As Int16
                Return x - y
            End Function)

        binder(Of Func(Of UInt16, UInt16, UInt16), binary_operator_minus_protector).set_global(
            Function(x As UInt16, y As UInt16) As UInt16
                Return x - y
            End Function)

        binder(Of Func(Of Int32, Int32, Int32), binary_operator_minus_protector).set_global(
            Function(x As Int32, y As Int32) As Int32
                Return x - y
            End Function)

        binder(Of Func(Of UInt32, UInt32, UInt32), binary_operator_minus_protector).set_global(
            Function(x As UInt32, y As UInt32) As UInt32
                Return x - y
            End Function)

        binder(Of Func(Of Int64, Int64, Int64), binary_operator_minus_protector).set_global(
            Function(x As Int64, y As Int64) As Int64
                Return x - y
            End Function)

        binder(Of Func(Of UInt64, UInt64, UInt64), binary_operator_minus_protector).set_global(
            Function(x As UInt64, y As UInt64) As UInt64
                Return x - y
            End Function)

        binder(Of Func(Of Single, Single, Single), binary_operator_minus_protector).set_global(
            Function(x As Single, y As Single) As Single
                Return x - y
            End Function)

        binder(Of Func(Of Double, Double, Double), binary_operator_minus_protector).set_global(
            Function(x As Double, y As Double) As Double
                Return x - y
            End Function)
    End Sub

    Private Sub bind_default_binary_operator_add()
        binder(Of Func(Of SByte, SByte, SByte), binary_operator_add_protector).set_global(
            Function(x As SByte, y As SByte) As SByte
                Return x + y
            End Function)

        binder(Of Func(Of Byte, Byte, Byte), binary_operator_add_protector).set_global(
            Function(x As Byte, y As Byte) As Byte
                Return x + y
            End Function)

        binder(Of Func(Of Int16, Int16, Int16), binary_operator_add_protector).set_global(
            Function(x As Int16, y As Int16) As Int16
                Return x + y
            End Function)

        binder(Of Func(Of UInt16, UInt16, UInt16), binary_operator_add_protector).set_global(
            Function(x As UInt16, y As UInt16) As UInt16
                Return x + y
            End Function)

        binder(Of Func(Of Int32, Int32, Int32), binary_operator_add_protector).set_global(
            Function(x As Int32, y As Int32) As Int32
                Return x + y
            End Function)

        binder(Of Func(Of UInt32, UInt32, UInt32), binary_operator_add_protector).set_global(
            Function(x As UInt32, y As UInt32) As UInt32
                Return x + y
            End Function)

        binder(Of Func(Of Int64, Int64, Int64), binary_operator_add_protector).set_global(
            Function(x As Int64, y As Int64) As Int64
                Return x + y
            End Function)

        binder(Of Func(Of UInt64, UInt64, UInt64), binary_operator_add_protector).set_global(
            Function(x As UInt64, y As UInt64) As UInt64
                Return x + y
            End Function)

        binder(Of Func(Of Single, Single, Single), binary_operator_add_protector).set_global(
            Function(x As Single, y As Single) As Single
                Return x + y
            End Function)

        binder(Of Func(Of Double, Double, Double), binary_operator_add_protector).set_global(
            Function(x As Double, y As Double) As Double
                Return x + y
            End Function)

        binder(Of Func(Of String, String, String), binary_operator_add_protector).set_global(
            Function(x As String, y As String) As String
                Return x + y
            End Function)
    End Sub
End Module