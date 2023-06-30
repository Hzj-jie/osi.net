
Imports osi.root.connector

Public Module _formation
    Public Sub rndarray(ByRef a() As String, ByVal size_upper_bound As Int32)
        ReDim a(rnd_int(0, size_upper_bound))
        Dim i As Int64
        For i = 0 To a.Length() - 1
            a(i) = guid_str()
        Next
    End Sub

    Public Sub rndarray(ByRef a() As String)
        rndarray(a, 1024)
    End Sub
End Module
