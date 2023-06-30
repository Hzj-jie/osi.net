
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.constants

Partial Public NotInheritable Class rlexer
    Public Function define(ByVal regex As regex) As Boolean
        If regex Is Nothing Then
            Return False
        End If
        rs.emplace_back(regex)
        Return True
    End Function

    Public Function define(ByVal regex As regex, ByVal type As UInt32) As Boolean
        If regex Is Nothing Then
            Return False
        End If
        If rs.size() <= type Then
            rs.resize(type + uint32_1)
        End If
        rs(type) = regex
        Return True
    End Function
End Class
