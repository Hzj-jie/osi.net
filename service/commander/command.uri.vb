
#If RETIRED
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Text
Imports osi.root.connector
Imports osi.root.connector.uri
Imports osi.root.constants

Public Module _command_uri
    Private Const separator As Char = character.dot
    Private ReadOnly separators() As Char = {separator}
    Private Const escape As Char = character.dollar

    Sub New()
        assert(separator.unreserved())
        assert(escape.unreserved())

        uri_serializer.register(Sub(ByVal i As command, ByVal o As StringWriter)

                                End Sub)
    End Sub

    'only return nothing when b is empty
    Private Function uri_encode(ByVal b() As Byte) As String
        If isemptyarray(b) Then
            Return Nothing
        Else
            Dim o As StringBuilder = Nothing
            o = New StringBuilder()
            For i As Int32 = 0 To array_size_i(b) - 1
                Dim c As Char = Nothing
                c = Convert.ToChar(b(i))
                If c.unreserved() AndAlso
                   c <> separator AndAlso
                   c <> escape Then
                    o.Append(c)
                Else
                    o.Append(escape) _
                     .Append(b(i).hex())
                End If
            Next
            Return Convert.ToString(o)
        End If
    End Function

    'only return nothing when s is an empty string or the input string is invalid, say, with invalid character in uri
    Private Function uri_decode(ByVal s As String) As Byte()
        If String.IsNullOrEmpty(s) Then
            Return Nothing
        Else
            Dim o() As Byte = Nothing
            ReDim o(strlen_i(s) - 1)
            Dim l As Int32 = 0
            For i As Int32 = 0 To strlen_i(s) - 1
                assert(s(i) <> separator)
                If s(i) = escape Then
                    Dim b As Byte = 0
                    If hex_byte(strmid(s, CUInt(i + 1), expected_hex_byte_length), b) Then
                        o(l) = b
                        l += 1
                        i += expected_hex_byte_length
                    Else
                        Return Nothing
                    End If
                ElseIf s(i).unreserved() Then
                    Dim t As Int32 = 0
                    t = Convert.ToInt32(s(i))
                    assert(t >= 0 AndAlso t <= max_uint8)
                    o(l) = CByte(t)
                    l += 1
                Else
                    Return Nothing
                End If
            Next

            assert(l > 0)
            Dim r() As Byte = Nothing
            ReDim r(l - 1)
            memcpy(r, o, CUInt(l))
            Return r
        End If
    End Function

    'only return nothing when input this is nothing
    'start with /, so it's safe to use it directly in http request
    <Extension()> Public Function to_uri(ByVal this As command) As String
        If this Is Nothing Then
            Return Nothing
        Else
            Dim o As StringBuilder = Nothing
            o = New StringBuilder()
            o.Append(root.constants.uri.path_separator)
            o.Append(uri_encode(this.action()))
            assert(this.foreach(Sub(x, y)
                                    o.Append(separator) _
                                     .Append(uri_encode(x)) _
                                     .Append(separator) _
                                     .Append(uri_encode(y))
                                End Sub))
            Return Convert.ToString(o)
        End If
    End Function

    <Extension()> Public Function from_uri(ByVal this As command, ByVal uri As String) As Boolean
        If this Is Nothing OrElse
           String.IsNullOrEmpty(uri) OrElse
           uri(0) <> root.constants.uri.path_separator Then
            Return False
        Else
            Dim ss() As String = Nothing
            ss = strmid(uri, 1).Split(separators)
            If (array_size(ss) And 1) <> 1 Then
                Return False
            Else
                this.clear()
                this.set_action_no_copy(uri_decode(ss(0)))
                For i As Int32 = 1 To array_size_i(ss) - 1 Step 2
                    this.set_parameter_no_copy(uri_decode(ss(i)), uri_decode(ss(i + 1)))
                Next
                Return True
            End If
        End If
    End Function
End Module
#End If
