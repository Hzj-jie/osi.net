
Imports System.Runtime.CompilerServices
Imports System.Text

Public Module _encoding
    Public Function try_get_encoding(ByVal name As String, ByRef o As Encoding) As Boolean
        If String.IsNullOrEmpty(name) Then
            Return False
        Else
            Try
                o = Encoding.GetEncoding(name)
                Return True
            Catch
                Return False
            End Try
        End If
    End Function

    Public Function try_get_encoding(ByVal codepage As Int32, ByRef o As Encoding) As Boolean
        Try
            o = Encoding.GetEncoding(codepage)
            Return True
        Catch
            Return False
        End Try
    End Function

    <Extension()> Public Function try_get_string(ByVal e As Encoding,
                                                 ByVal buff() As Byte,
                                                 ByVal offset As Int32,
                                                 ByVal count As Int32,
                                                 ByRef o As String) As Boolean
        If e Is Nothing OrElse
           buff Is Nothing OrElse
           offset < 0 OrElse
           count < 0 OrElse
           array_size(buff) < offset + count Then
            Return False
        Else
            Try
                o = e.GetString(buff, offset, count)
                Return True
            Catch
                Return False
            End Try
        End If
    End Function

    <Extension()> Public Function try_get_string(ByVal e As Encoding,
                                                 ByVal buff() As Byte,
                                                 ByRef o As String) As Boolean
        If e Is Nothing OrElse
           buff Is Nothing Then
            Return False
        Else
            Try
                o = e.GetString(buff)
                Return True
            Catch
                Return False
            End Try
        End If
    End Function
End Module
