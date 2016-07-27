
Imports System.Runtime.CompilerServices
Imports osi.root.connector

Public Module _conv
    Private Function set_to_vector(Of T)(ByVal i As [set](Of T),
                                         ByRef o As vector(Of T),
                                         ByVal emplace As Boolean) As Boolean
        If i Is Nothing Then
            Return False
        Else
            o.renew()
            Dim it As [set](Of T).iterator = Nothing
            it = i.begin()
            If emplace Then
                While it <> i.end()
                    o.emplace_back(+it)
                    it += 1
                End While
            Else
                While it <> i.end()
                    o.push_back(+it)
                    it += 1
                End While
            End If
            Return True
        End If
    End Function

    <Extension()> Public Function emplace_to_vector(Of T)(ByVal i As [set](Of T), ByRef o As vector(Of T)) As Boolean
        Return set_to_vector(i, o, True)
    End Function

    <Extension()> Public Function to_vector(Of T)(ByVal i As [set](Of T), ByRef o As vector(Of T)) As Boolean
        Return set_to_vector(i, o, False)
    End Function

    <Extension()> Public Function emplace_to_vector(Of T)(ByVal i As [set](Of T)) As vector(Of T)
        Dim r As vector(Of T) = Nothing
        assert(i.emplace_to_vector(r))
        Return r
    End Function

    <Extension()> Public Function to_vector(Of T)(ByVal i As [set](Of T)) As vector(Of T)
        Dim r As vector(Of T) = Nothing
        assert(i.to_vector(r))
        Return r
    End Function
End Module
