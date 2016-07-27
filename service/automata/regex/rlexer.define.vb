
Partial Public Class rlexer
    Public Function define(ByVal regex As regex) As Boolean
        If regex Is Nothing Then
            Return False
        Else
            rs.emplace_back(regex)
            Return True
        End If
    End Function

    Public Function define(ByVal regex As regex, ByVal type As UInt32) As Boolean
        If regex Is Nothing Then
            Return False
        Else
            If rs.size() <= type Then
                rs.resize(type + 1)
            End If
            rs(type) = regex
            Return True
        End If
    End Function
End Class
