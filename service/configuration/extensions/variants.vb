
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public Module _variants
    Public Function create_variants(ByRef r As vector(Of pair(Of String, String)),
                                    ByVal ParamArray kvs() As Object) As Boolean
        r.renew()
        Return append_variants(r, kvs)
    End Function

    Public Function create_variants(ByVal ParamArray kvs() As Object) As vector(Of pair(Of String, String))
        Dim r As vector(Of pair(Of String, String)) = Nothing
        assert(create_variants(r, kvs))
        Return r
    End Function

    <Extension()> Public Function create_variants(ByVal m As map(Of String, String),
                                                  ByRef r As vector(Of pair(Of String, String))) As Boolean
        If m Is Nothing Then
            Return False
        Else
            r.renew()
            Dim i As map(Of String, String).iterator = Nothing
            i = m.begin()
            While i <> m.end()
                If Not append_variant(r, (+i).first, (+i).second) Then
                    Return False
                End If
                i += 1
            End While
            Return True
        End If
    End Function

    'consistant with create_variants with paramarray
    Public Function create_variants(ByRef r As vector(Of pair(Of String, String)),
                                    ByVal m As map(Of String, String)) As Boolean
        Return create_variants(m, r)
    End Function

    <Extension()> Public Function create_variants(ByVal m As map(Of String, String)) _
                                                 As vector(Of pair(Of String, String))
        Dim r As vector(Of pair(Of String, String)) = Nothing
        assert(create_variants(m, r))
        Return r
    End Function

    <Extension()> Public Function append_variant(ByVal r As vector(Of pair(Of String, String)),
                                                 ByVal key As String,
                                                 ByVal value As String) As Boolean
        If r Is Nothing OrElse String.IsNullOrEmpty(key) OrElse String.IsNullOrEmpty(value) Then
            Return False
        Else
            r.emplace_back(make_pair(key, value))
            Return True
        End If
    End Function

    <Extension()> Public Function append_variant(ByVal r As vector(Of pair(Of String, String)),
                                                 ByVal key As Object,
                                                 ByVal value As Object) As Boolean
        Return append_variant(r, Convert.ToString(key), Convert.ToString(value))
    End Function

    <Extension()> Public Function append_variants(ByVal r As vector(Of pair(Of String, String)),
                                                  ByVal ParamArray kvs() As Object) As Boolean
        If r Is Nothing OrElse isemptyarray(kvs) OrElse (array_size(kvs) And uint32_1) = uint32_1 Then
            Return False
        Else
            For i As Int32 = 0 To array_size_i(kvs) - 1 Step 2
                If Not append_variant(r, kvs(i), kvs(i + 1)) Then
                    Return False
                End If
            Next
            Return True
        End If
    End Function
End Module
