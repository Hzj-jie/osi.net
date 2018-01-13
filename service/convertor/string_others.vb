
' TODO Remove
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public Module _string_others
    Private ReadOnly default_array_separators() As Char = {character.comma}

    <Extension()> Public Function to_string(ByVal s As String,
                                            Optional ByVal default_value As String = Nothing) As String
        If String.IsNullOrEmpty(s) Then
            Return default_value
        Else
            Return s
        End If
    End Function

    <Extension()> Public Function to_bytes(ByVal s As String,
                                           Optional ByVal default_value As Byte() = Nothing) As Byte()
        If String.IsNullOrEmpty(s) Then
            Return default_value
        Else
            Return str_bytes(s)
        End If
    End Function

    <Extension()> Public Function to_bool(ByVal s As String,
                                          Optional ByVal default_value As Boolean = False) As Boolean
        Dim o As Boolean = False
        If str_bool(s, o) Then
            Return o
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_sbyte(ByVal s As String,
                                           Optional ByVal default_value As SByte = 0) As SByte
        Dim i As SByte = 0
        If SByte.TryParse(s, i) Then
            Return i
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_int16(ByVal s As String,
                                           Optional ByVal default_value As Int16 = 0) As Int16
        Dim i As Int16 = 0
        If Int16.TryParse(s, i) Then
            Return i
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_int32(ByVal s As String,
                                           Optional ByVal default_value As Int32 = 0) As Int32
        Dim i As Int32 = 0
        If Int32.TryParse(s, i) Then
            Return i
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_int64(ByVal s As String,
                                           Optional ByVal default_value As Int64 = 0) As Int64
        Dim i As Int64 = 0
        If Int64.TryParse(s, i) Then
            Return i
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_single(ByVal s As String,
                                            Optional ByVal default_value As Single = 0) As Single
        Dim i As Single = 0
        If Single.TryParse(s, i) Then
            Return i
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_double(ByVal s As String,
                                            Optional ByVal default_value As Double = 0) As Double
        Dim i As Double = 0
        If Double.TryParse(s, i) Then
            Return i
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_byte(ByVal s As String,
                                          Optional ByVal default_value As Byte = 0) As Byte
        Dim i As Byte = 0
        If Byte.TryParse(s, i) Then
            Return i
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_uint16(ByVal s As String,
                                            Optional ByVal default_value As UInt16 = 0) As UInt16
        Dim i As UInt16 = 0
        If UInt16.TryParse(s, i) Then
            Return i
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_uint32(ByVal s As String,
                                            Optional ByVal default_value As UInt32 = 0) As UInt32
        Dim i As UInt32 = 0
        If UInt32.TryParse(s, i) Then
            Return i
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_uint64(ByVal s As String,
                                            Optional ByVal default_value As UInt64 = 0) As UInt64
        Dim i As UInt64 = 0
        If UInt64.TryParse(s, i) Then
            Return i
        Else
            Return default_value
        End If
    End Function

    <Extension()> Public Function to_string_array(ByVal s As String,
                                                  ByVal ParamArray separators() As Char) As vector(Of String)
        If String.IsNullOrEmpty(s) Then
            Return Nothing
        Else
            Dim r As vector(Of String) = Nothing
            r = New vector(Of String)()
            Dim ss() As String = Nothing
            If isemptyarray(separators) Then
                separators = default_array_separators
            End If
            ss = s.Split(separators)
            r.emplace_back(ss)
            Return r
        End If
    End Function

    Private Function to_any_array(Of T)(ByVal v As vector(Of String),
                                        ByVal c As Func(Of String, T)) As vector(Of T)
        assert(Not c Is Nothing)
        If v Is Nothing Then
            Return Nothing
        ElseIf v.empty() Then
            Return New vector(Of T)()
        Else
            Dim r As vector(Of T) = Nothing
            r = New vector(Of T)()
            For i As UInt32 = 0 To v.size() - uint32_1
                r.emplace_back(c(v(i)))
            Next
            Return r
        End If
    End Function

    <Extension()> Public Function to_any_array(Of T)(ByVal v As vector(Of String),
                                                     Optional ByVal string_T As string_serializer(Of T) = Nothing) _
                                                    As vector(Of T)
        Return to_any_array(v,
                            Function(s As String) As T
                                Dim o As T = Nothing
                                If (+string_T).from_str(s, o) Then
                                    Return o
                                Else
                                    Return Nothing
                                End If
                            End Function)
    End Function

    Private Function to_any_array(Of T)(ByVal s As String,
                                        ByVal separators() As Char,
                                        ByVal c As Func(Of String, T)) As vector(Of T)
        Return to_any_array(to_string_array(s, separators), c)
    End Function

    <Extension()> Public Function to_any_array(Of T)(ByVal s As String,
                                                     ByVal separators() As Char,
                                                     Optional ByVal string_T As string_serializer(Of T) = Nothing) _
                                                    As vector(Of T)
        Return to_any_array(s, separators, Function(x As String) As T
                                               Dim o As T = Nothing
                                               If (+string_T).from_str(x, o) Then
                                                   Return o
                                               Else
                                                   Return Nothing
                                               End If
                                           End Function)
    End Function

    <Extension()> Public Function to_bool_array(ByVal s As String,
                                                ByVal ParamArray separators() As Char) As vector(Of Boolean)
        Return to_any_array(s, separators, AddressOf to_bool)
    End Function

    <Extension()> Public Function to_int32_array(ByVal s As String,
                                                 ByVal ParamArray separators() As Char) As vector(Of Int32)
        Return to_any_array(s, separators, AddressOf to_int32)
    End Function

    <Extension()> Public Function to_uint32_array(ByVal s As String,
                                                  ByVal ParamArray separators() As Char) As vector(Of UInt32)
        Return to_any_array(s, separators, AddressOf to_uint32)
    End Function
End Module
