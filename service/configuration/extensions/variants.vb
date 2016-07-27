
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.formation
Imports osi.root.utils

Public Module _variants
    Public Function create_variants(ByRef r As vector(Of pair(Of String, String)),
                                    ByVal ParamArray kvs() As String) As Boolean
        If isemptyarray(kvs) OrElse (array_size(kvs) & 1) = 1 Then
            Return False
        Else
            If r Is Nothing Then
                r = New vector(Of pair(Of String, String))()
            Else
                r.clear()
            End If
            For i As Int32 = 0 To array_size(kvs) - 1 Step 2
                r.emplace_back(make_pair(kvs(i), kvs(i + 1)))
            Next
            Return True
        End If
    End Function

    Public Function create_variants(ByVal ParamArray kvs() As String) As vector(Of pair(Of String, String))
        Dim r As vector(Of pair(Of String, String)) = Nothing
        assert(create_variants(r, kvs))
        Return r
    End Function

    <Extension()> Public Function create_variants(ByVal m As map(Of String, String),
                                                  ByRef r As vector(Of pair(Of String, String))) As Boolean
        If m Is Nothing Then
            Return False
        Else
            If r Is Nothing Then
                r = New vector(Of pair(Of String, String))()
            Else
                r.clear()
            End If
            Dim i As map(Of String, String).iterator = Nothing
            i = m.begin()
            While i <> m.end()
                r.push_back(make_pair((+i).first, (+i).second))
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
End Module
