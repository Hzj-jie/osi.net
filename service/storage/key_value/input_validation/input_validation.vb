
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.root.utils

Friend Module input_validation
    Private ReadOnly dummy_byte() As Byte

    Sub New()
        ReDim dummy_byte(0)
        assert(Not isemptyarray(dummy_byte))
    End Sub

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal result As ref(Of Boolean)) As Boolean
        Return Not isemptyarray(key) AndAlso
               Not isemptyarray(value) AndAlso
               Not result Is Nothing AndAlso
               eva(result, False)
    End Function

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByRef result As Boolean) As Boolean
        Return Not isemptyarray(key) AndAlso
               Not isemptyarray(value) AndAlso
               eva(result, False)
    End Function

    Public Function append(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As Boolean
        Return append(key, value, result) AndAlso ts >= 0
    End Function

    Public Function append(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As Boolean
        Return Not key.null_or_empty() AndAlso
               append(dummy_byte, value, ts, result)
    End Function

    Public Function capacity(ByVal result As ref(Of Int64)) As Boolean
        Return Not result Is Nothing AndAlso
               eva(result, npos)
    End Function

    Public Function capacity(ByRef result As Int64) As Boolean
        Return eva(result, npos)
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByVal result As ref(Of Boolean)) As Boolean
        Return Not isemptyarray(key) AndAlso
               Not result Is Nothing AndAlso
               eva(result, False)
    End Function

    Public Function delete(ByVal key() As Byte,
                           ByRef result As Boolean) As Boolean
        Return Not isemptyarray(key) AndAlso
               eva(result, False)
    End Function

    Public Function delete(ByVal key As String,
                           ByVal result As ref(Of Boolean)) As Boolean
        Return Not key.null_or_empty() AndAlso
               delete(dummy_byte, result)
    End Function

    Public Function empty(ByVal result As ref(Of Boolean)) As Boolean
        Return Not result Is Nothing AndAlso
               eva(result, False)
    End Function

    Public Function empty(ByRef result As Boolean) As Boolean
        Return eva(result, False)
    End Function

    Public Function full(ByVal result As ref(Of Boolean)) As Boolean
        Return Not result Is Nothing AndAlso
               eva(result, False)
    End Function

    Public Function full(ByRef result As Boolean) As Boolean
        Return eva(result, False)
    End Function

    Public Function keycount(ByVal result As ref(Of Int64)) As Boolean
        Return Not result Is Nothing AndAlso
               eva(result, npos)
    End Function

    Public Function keycount(ByRef result As Int64) As Boolean
        Return eva(result, npos)
    End Function

    Public Function list(ByVal result As ref(Of vector(Of Byte()))) As Boolean
        Return Not result Is Nothing AndAlso
               eva(result, DirectCast(Nothing, vector(Of Byte())))
    End Function

    Public Function list(ByRef result As vector(Of Byte())) As Boolean
        Return eva(result, DirectCast(Nothing, vector(Of Byte())))
    End Function

    Public Function list(ByVal result As ref(Of vector(Of String))) As Boolean
        Return Not result Is Nothing AndAlso
               eva(result, DirectCast(Nothing, vector(Of String)))
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal result As ref(Of Boolean)) As Boolean
        Return Not isemptyarray(key) AndAlso
               Not isemptyarray(value) AndAlso
               Not result Is Nothing AndAlso
               eva(result, False)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByRef result As Boolean) As Boolean
        Return Not isemptyarray(key) AndAlso
               Not isemptyarray(value) AndAlso
               eva(result, False)
    End Function

    Public Function modify(ByVal key() As Byte,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As Boolean
        Return modify(key, value, result) AndAlso ts >= 0
    End Function

    Public Function modify(ByVal key As String,
                           ByVal value() As Byte,
                           ByVal ts As Int64,
                           ByVal result As ref(Of Boolean)) As Boolean
        Return Not key.null_or_empty() AndAlso
               modify(dummy_byte, value, ts, result)
    End Function

    Public Function read(ByVal key() As Byte,
                         ByVal value As ref(Of Byte())) As Boolean
        Return Not isemptyarray(key) AndAlso
               Not value Is Nothing AndAlso
               eva(value, DirectCast(Nothing, Byte()))
    End Function

    Public Function read(ByVal key() As Byte,
                         ByRef value() As Byte) As Boolean
        Return Not isemptyarray(key) AndAlso
               eva(value, DirectCast(Nothing, Byte()))
    End Function

    Public Function read(ByVal key() As Byte,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As Boolean
        Return read(key, result) AndAlso
               Not ts Is Nothing AndAlso
               eva(ts, npos)
    End Function

    Public Function read(ByVal key As String,
                         ByVal result As ref(Of Byte()),
                         ByVal ts As ref(Of Int64)) As Boolean
        Return Not key.null_or_empty() AndAlso
               read(dummy_byte, result, ts)
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByVal result As ref(Of Boolean)) As Boolean
        Return Not isemptyarray(key) AndAlso
               Not result Is Nothing AndAlso
               eva(result, False)
    End Function

    Public Function seek(ByVal key() As Byte,
                         ByRef result As Boolean) As Boolean
        Return Not isemptyarray(key) AndAlso
               eva(result, False)
    End Function

    Public Function seek(ByVal key As String,
                         ByVal result As ref(Of Boolean)) As Boolean
        Return Not key.null_or_empty() AndAlso
               seek(dummy_byte, result)
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByVal result As ref(Of Int64)) As Boolean
        Return Not isemptyarray(key) AndAlso
               Not result Is Nothing AndAlso
               eva(result, npos)
    End Function

    Public Function sizeof(ByVal key() As Byte,
                           ByRef result As Int64) As Boolean
        Return Not isemptyarray(key) AndAlso
               eva(result, npos)
    End Function

    Public Function sizeof(ByVal key As String,
                           ByVal result As ref(Of Int64)) As Boolean
        Return Not key.null_or_empty() AndAlso
               sizeof(dummy_byte, result)
    End Function

    Public Function unique_write(ByVal key() As Byte,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As Boolean
        Return modify(key, value, ts, result)
    End Function

    Public Function unique_write(ByVal key As String,
                                 ByVal value() As Byte,
                                 ByVal ts As Int64,
                                 ByVal result As ref(Of Boolean)) As Boolean
        Return Not key.null_or_empty() AndAlso
               unique_write(dummy_byte, value, ts, result)
    End Function

    Public Function valuesize(ByVal result As ref(Of Int64)) As Boolean
        Return Not result Is Nothing AndAlso
               eva(result, npos)
    End Function

    Public Function valuesize(ByRef result As Int64) As Boolean
        Return eva(result, npos)
    End Function
End Module
