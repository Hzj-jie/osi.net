
Imports System.Net
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Public Module _ipendpoint
    Sub New()
        assert(Not type_info(Of IPAddress).is_cloneable AndAlso Not type_info(Of IPAddress).is_cloneable_T)
    End Sub

    <Extension()> Public Function to_pair(ByVal this As IPEndPoint) As pair(Of IPAddress, UInt16)
        If this Is Nothing Then
            Return Nothing
        Else
            assert(this.Port() >= min_uint16 AndAlso this.Port() <= max_uint16)
            Return emplace_make_pair(this.Address(), CUShort(this.Port()))
        End If
    End Function

    <Extension()> Public Function to_const_pair(ByVal this As IPEndPoint) As const_pair(Of IPAddress, UInt16)
        Dim r As pair(Of IPAddress, UInt16) = Nothing
        r = to_pair(this)
        If r Is Nothing Then
            Return Nothing
        Else
            Return r.emplace_to_const_pair()
        End If
    End Function

    <Extension()> Public Function to_string_pair(ByVal this As IPEndPoint) As pair(Of String, UInt16)
        Dim r As pair(Of IPAddress, UInt16) = Nothing
        r = to_pair(this)
        If r Is Nothing Then
            Return Nothing
        Else
            Return emplace_make_pair(Convert.ToString(r.first), r.second)
        End If
    End Function

    <Extension()> Public Function to_string_const_pair(ByVal this As IPEndPoint) As const_pair(Of String, UInt16)
        Dim r As pair(Of String, UInt16) = Nothing
        r = to_string_pair(this)
        If r Is Nothing Then
            Return Nothing
        Else
            Return r.emplace_to_const_pair()
        End If
    End Function

    <Extension()> Public Function to_ipendpoint(ByVal this As pair(Of IPAddress, UInt16)) As IPEndPoint
        If this Is Nothing OrElse this.first Is Nothing Then
            Return Nothing
        Else
            Return New IPEndPoint(this.first, this.second)
        End If
    End Function

    <Extension()> Public Function to_ipendpoint(ByVal this As const_pair(Of IPAddress, UInt16)) As IPEndPoint
        If this Is Nothing OrElse this.first Is Nothing Then
            Return Nothing
        Else
            Return New IPEndPoint(this.first, this.second)
        End If
    End Function
End Module
