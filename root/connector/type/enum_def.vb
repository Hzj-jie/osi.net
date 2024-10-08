
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.constants

' Use instance to allow extension methods in other projects.
Public NotInheritable Class enum_definition(Of T)
    Public Shared ReadOnly instance As enum_definition(Of T) = New enum_definition(Of T)()
    Private Shared ReadOnly type As Type = GetType(T)
    Private Shared ReadOnly _underlying_type As Type = type.GetEnumUnderlyingType()
    Private Shared ReadOnly values As Array = [Enum].GetValues(type)
    Private Shared ReadOnly _width As Byte = CByte(sizeof(_underlying_type))

    Shared Sub New()
        assert(type_info(Of T).is_enum)
        assert(Not values Is Nothing AndAlso values.Length() >= 0)
        bytes_serializer.fixed.register(Of T)(AddressOf instance.append_to, AddressOf instance.consume_from)
        string_serializer.register(Of T)(AddressOf instance.write_to, AddressOf instance.read_from)
    End Sub

    Public Function count() As UInt32
        Return CUInt(count_i())
    End Function

    Public Function count_i() As Int32
        Return values.Length()
    End Function

    Public Function underlying_type() As Type
        Return _underlying_type
    End Function

    Public Function width() As Byte
        Return _width
    End Function

    Public Sub foreach(ByVal d As Action(Of T, String))
        assert(Not d Is Nothing)
        For Each value As T In values
            Try
                d(value, [Enum].GetName(type, value))
            Catch ex As break_lambda
                Exit For
            End Try
        Next
    End Sub

    Public Sub foreach(ByVal d As Action(Of T))
        assert(Not d Is Nothing)
        foreach(Sub(ByVal i As T, ByVal name As String)
                    d(i)
                End Sub)
    End Sub

    Public Sub foreach(ByVal d As Action(Of String))
        assert(Not d Is Nothing)
        foreach(Sub(ByVal i As T, ByVal name As String)
                    d(name)
                End Sub)
    End Sub

    Public Function has(Of VT)(ByVal i As VT) As Boolean
        If Not type_info(Of VT).is_integral Then
            Return False
        End If
        Dim i2 As T = Nothing
        If Not from(i, i2) Then
            Return False
        End If
        Dim r As Boolean = False
        foreach(Sub(ByVal x As T)
                    If compare(x, i2) = 0 Then
                        r = True
                        break_lambda.at_here()
                    End If
                End Sub)
        Return r
    End Function

    Public Function has(ByVal i As String) As Boolean
        Dim r As Boolean = False
        foreach(Sub(ByVal x As String)
                    If strsame(x, i) Then
                        r = True
                        break_lambda.at_here()
                    End If
                End Sub)
        Return r
    End Function

    Public Function from(Of VT)(ByVal i As VT, ByRef o As T) As Boolean
        ' IsDefined accepts only the same type as underlying type, which does not match the requirement of this
        ' function.
        Dim j As Object = Nothing
        Try
            j = [Enum].ToObject(type, i)
        Catch ex As Exception
            log_unhandled_exception(strcat("enum_def.from(Of ", GetType(VT), ")"), ex)
            Return False
        End Try
        o = cast_from(j).to(Of T)()
        Return True
    End Function

    Public Function from(Of VT)(ByVal i As VT) As T
        Dim o As T = Nothing
        assert(from(i, o))
        Return o
    End Function

    Public Function from(ByVal i As String, ByRef o As T) As Boolean
        If Not has(i) Then
            Return False
        End If
        o = cast_from([Enum].Parse(type, i)).to(Of T)()
        Return True
    End Function

    Public Function from(ByVal i As String) As T
        Dim o As T = Nothing
        assert(cast(i, o))
        Return o
    End Function

    Public Function [to](Of VT)(ByVal i As T, ByRef o As VT) As Boolean
        If Not type_info(Of VT).is_number Then
            Return False
        End If
        Try
            o = direct_cast(Of VT)(Convert.ChangeType(i, GetType(VT)))
            Return True
        Catch ex As Exception
            log_unhandled_exception(ex)
            Return False
        End Try
    End Function

    Public Function [to](Of VT)(ByVal i As T) As VT
        Dim o As VT = Nothing
        assert([to](i, o))
        Return o
    End Function

    Public Function [to](ByVal i As T, ByRef o As String) As Boolean
        o = i.ToString()
        Return True
    End Function

    Public Function [to](ByVal i As T) As String
        Dim o As String = Nothing
        assert([to](i, o))
        Return o
    End Function

    ' Use string_serializer instead of directly calling this function.
    Private Function read_from(ByVal i As StringReader, ByRef o As T) As Boolean
        assert(Not i Is Nothing)
        Return from(i.ReadToEnd(), o)
    End Function

    Private Function write_to(ByVal i As T, ByVal o As StringWriter) As Boolean
        assert(Not o Is Nothing)
        Dim s As String = Nothing
        If Not [to](i, s) Then
            Return False
        End If
        o.Write(s)
        Return True
    End Function

    ' Use byte_serializer instead of directly calling this function.
    Private Function consume_from(ByVal i As MemoryStream, ByRef o As T) As Boolean
        Select Case width()
            Case 1
                Dim x As SByte = 0
                If Not bytes_serializer.consume_from(i, x) Then
                    Return False
                End If
                Return from(x, o)
            Case 2
                Dim x As Int16 = 0
                If Not bytes_serializer.consume_from(i, x) Then
                    Return False
                End If
                Return from(x, o)
            Case 4
                Dim x As Int32 = 0
                If Not bytes_serializer.consume_from(i, x) Then
                    Return False
                End If
                Return from(x, o)
            Case 8
                Dim x As Int64 = 0
                If Not bytes_serializer.consume_from(i, x) Then
                    Return False
                End If
                Return from(x, o)
            Case Else
                Return assert(False)
        End Select
    End Function

    Private Function append_to(ByVal i As T, ByVal o As MemoryStream) As Boolean
        Select Case width()
            Case 1
                Dim x As SByte = 0
                If Not [to](i, x) Then
                    Return False
                End If
                Return bytes_serializer.append_to(x, o)
            Case 2
                Dim x As Int16 = 0
                If Not [to](i, x) Then
                    Return False
                End If
                Return bytes_serializer.append_to(x, o)
            Case 4
                Dim x As Int32 = 0
                If Not [to](i, x) Then
                    Return False
                End If
                Return bytes_serializer.append_to(x, o)
            Case 8
                Dim x As Int64 = 0
                If Not [to](i, x) Then
                    Return False
                End If
                Return bytes_serializer.append_to(x, o)
            Case Else
                Return assert(False)
        End Select
    End Function

    Private Sub New()
    End Sub
End Class

' C#
Public NotInheritable Class enum_def
    Public Shared Function [of](Of T)() As enum_definition(Of T)
        Return enum_def(Of T)()
    End Function

    Public Shared Function from(Of T, VT)(ByVal i As VT, ByRef o As T) As Boolean
        Return [of](Of T)().from(i, o)
    End Function

    Public Shared Function from(Of T)(ByVal i As String, ByRef o As T) As Boolean
        Return [of](Of T)().from(i, o)
    End Function

    Public Shared Function from(Of T)(ByVal i As MemoryStream, ByRef o As T) As Boolean
        Return [of](Of T)().from(i, o)
    End Function

    Public Shared Function [to](Of T)(ByVal i As T, ByVal o As MemoryStream) As Boolean
        Return [of](Of T)().to(i, o)
    End Function

    Private Sub New()
    End Sub
End Class

' VB.net
Public Module _enum_def
    Public Function enum_def(Of T)() As enum_definition(Of T)
        Return enum_definition(Of T).instance
    End Function
End Module