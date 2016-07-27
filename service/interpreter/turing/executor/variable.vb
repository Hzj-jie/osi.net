
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utils

Namespace turing.executor
    Public Class variable
        Implements instruction

        Public ReadOnly type As type
        Private value As Object

        Public Sub New(ByVal type As type, ByVal value As Object)
            assert(Not type Is Nothing)
            Me.type = type
            assert(set_value(value))
        End Sub

        Public Sub New(ByVal value As Byte)
            Me.New(type.byte, value)
        End Sub

        Public Sub New(ByVal value As Boolean)
            Me.New(type.bool, value)
        End Sub

        Public Sub New(ByVal value As Int32)
            Me.New(type.int, value)
        End Sub

        Public Sub New(ByVal value As Double)
            Me.New(type.float, value)
        End Sub

        Public Sub New(ByVal value As Char)
            Me.New(type.char, value)
        End Sub

        Public Sub New(ByVal value As String)
            Me.New(type.string, value)
        End Sub

        Public Function set_value(ByVal var As variable) As Boolean
            If Not var Is Nothing AndAlso
               type.match(var.type) Then
                Me.value = var.get_value()
                Return True
            Else
                Return False
            End If
        End Function

        Public Function set_value(ByVal value As Object) As Boolean
            If type.match(value) Then
                Me.value = value
                Return True
            Else
                Return False
            End If
        End Function

        Public Function get_value() As Object
            Return value
        End Function

        Public Function execute(ByVal processor As processor) As Boolean Implements instruction.execute
            Return processor.move_next()
        End Function
    End Class

    Public Module _variable
        <Extension()> Public Function is_byte(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_byte()
        End Function

        <Extension()> Public Function is_bool(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_bool()
        End Function

        <Extension()> Public Function is_int(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_int()
        End Function

        <Extension()> Public Function is_float(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_float()
        End Function

        <Extension()> Public Function is_char(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_char()
        End Function

        <Extension()> Public Function is_string(ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            Return i.type.is_string()
        End Function

        <Extension()> Public Function [byte](ByVal i As variable, ByRef o As Byte) As Boolean
            Return Not i Is Nothing AndAlso
                   i.is_byte() AndAlso
                   eva(o, DirectCast(i.get_value(), Byte))
        End Function

        <Extension()> Public Function [byte](ByVal i As variable) As Byte
            Dim o As Byte = 0
            assert([byte](i, o))
            Return o
        End Function

        <Extension()> Public Function bool(ByVal i As variable, ByRef o As Boolean) As Boolean
            Return Not i Is Nothing AndAlso
                   i.is_bool() AndAlso
                   eva(o, DirectCast(i.get_value(), Boolean))
        End Function

        <Extension()> Public Function bool(ByVal i As variable) As Boolean
            Dim o As Boolean = False
            assert(bool(i, o))
            Return o
        End Function

        <Extension()> Public Function int(ByVal i As variable, ByRef o As Int32) As Boolean
            Return Not i Is Nothing AndAlso
                   i.is_int() AndAlso
                   eva(o, DirectCast(i.get_value(), Int32))
        End Function

        <Extension()> Public Function int(ByVal i As variable) As Int32
            Dim o As Int32 = 0
            assert(int(i, o))
            Return o
        End Function

        <Extension()> Public Function float(ByVal i As variable, ByRef o As Double) As Boolean
            Return Not i Is Nothing AndAlso
                   i.is_float() AndAlso
                   eva(o, DirectCast(i.get_value(), Double))
        End Function

        <Extension()> Public Function float(ByVal i As variable) As Double
            Dim o As Double = 0
            assert(float(i, o))
            Return o
        End Function

        <Extension()> Public Function [char](ByVal i As variable, ByRef o As Char) As Boolean
            Return Not i Is Nothing AndAlso
                   i.is_char() AndAlso
                   eva(o, DirectCast(i.get_value(), Char))
        End Function

        <Extension()> Public Function [char](ByVal i As variable) As Char
            Dim o As Char = Nothing
            assert([char](i, o))
            Return o
        End Function

        <Extension()> Public Function [string](ByVal i As variable, ByRef o As String) As Boolean
            Return Not i Is Nothing AndAlso
                   i.is_string() AndAlso
                   eva(o, DirectCast(i.get_value(), String))
        End Function

        <Extension()> Public Function [string](ByVal i As variable) As String
            Dim o As String = Nothing
            assert([string](i, o))
            Return o
        End Function

        <Extension()> Public Function [true](ByVal i As variable) As Boolean
            assert(Not i Is Nothing)
            If i.is_byte() Then
                Return i.byte() <> 0
            ElseIf i.is_bool() Then
                Return i.bool()
            ElseIf i.is_int() Then
                Return i.int() <> 0
            ElseIf i.is_float() Then
                Return i.float() <> 0
            ElseIf i.is_char() Then
                Return i.char() <> character.null
            ElseIf i.is_string() Then
                Return Not i.string() Is Nothing
            Else
                Return assert(False)
            End If
        End Function

        <Extension()> Public Function [false](ByVal i As variable) As Boolean
            Return Not i.true()
        End Function

        <Extension()> Public Function number(ByVal i As variable) As Double
            assert(Not i Is Nothing)
            If i.is_float() Then
                Return i.float()
            ElseIf i.is_byte() Then
                Return i.byte()
            ElseIf i.is_int() Then
                Return i.int()
            ElseIf i.is_bool() Then
                Return If(i.bool(), 1, 0)
            ElseIf i.is_char() Then
                Return Convert.ToInt32(i.char())
            Else
                assert(False)
                Return 0
            End If
        End Function
    End Module
End Namespace
