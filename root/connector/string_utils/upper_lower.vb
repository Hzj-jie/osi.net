
Imports System.Runtime.CompilerServices

Public Module _upper_lower
    <Extension()> Public Function strtolower(ByRef s As String) As String
        If Not s Is Nothing Then
            s = s.ToLower()
        End If

        Return s
    End Function

    <Extension()> Public Function strtoupper(ByRef s As String) As String
        If Not s Is Nothing Then
            s = s.ToUpper()
        End If

        Return s
    End Function

    Public Function strtolower(Of T)(ByVal i As T) As String
        Return strtolower(Convert.ToString(i))
    End Function

    Public Function strtoupper(Of T)(ByVal i As T) As String
        Return strtoupper(Convert.ToString(i))
    End Function

    <Extension()> Public Function str_initial_upper(ByRef s As String) As String
        If Not s.null_or_empty() Then
            Dim a() As Char = Nothing
            a = s.ToCharArray()
            a(0) = Char.ToUpper(a(0))
            s = New String(a)
        End If

        Return s
    End Function

    <Extension()> Public Function strtoupper(ByVal i() As String) As String()
        For j As Int32 = 0 To array_size(i) - 1
            strtoupper(i(j))
        Next
        Return i
    End Function

    <Extension()> Public Function strtolower(ByVal i() As String) As String()
        For j As Int32 = 0 To array_size(i) - 1
            strtolower(i(j))
        Next
        Return i
    End Function

    <Extension()> Public Function str_initial_upper(ByVal i() As String) As String()
        For j As Int32 = 0 To array_size(i) - 1
            str_initial_upper(i(j))
        Next
        Return i
    End Function
End Module
