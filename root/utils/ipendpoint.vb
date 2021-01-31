
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Net
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

<global_init(global_init_level.runtime_assertions)>
Public Module _ipendpoint
    Private Sub init()
        assert(Not type_info(Of IPAddress).is_cloneable AndAlso Not type_info(Of IPAddress).is_cloneable_T)
    End Sub

    <Extension()> Public Function to_pair(ByVal this As IPEndPoint) As pair(Of IPAddress, UInt16)
        If this Is Nothing Then
            Return Nothing
        End If
        assert(this.Port() >= min_uint16 AndAlso this.Port() <= max_uint16)
        Return pair.emplace_of(this.Address(), CUShort(this.Port()))
    End Function

    <Extension()> Public Function to_const_pair(ByVal this As IPEndPoint) As const_pair(Of IPAddress, UInt16)
        Dim r As pair(Of IPAddress, UInt16) = to_pair(this)
        If r Is Nothing Then
            Return Nothing
        End If
        Return r.emplace_to_const_pair()
    End Function

    <Extension()> Public Function to_string_pair(ByVal this As IPEndPoint) As pair(Of String, UInt16)
        Dim r As pair(Of IPAddress, UInt16) = to_pair(this)
        If r Is Nothing Then
            Return Nothing
        End If
        Return pair.emplace_of(Convert.ToString(r.first), r.second)
    End Function

    <Extension()> Public Function to_string_const_pair(ByVal this As IPEndPoint) As const_pair(Of String, UInt16)
        Dim r As pair(Of String, UInt16) = to_string_pair(this)
        If r Is Nothing Then
            Return Nothing
        End If
        Return r.emplace_to_const_pair()
    End Function

    <Extension()> Public Function to_ipendpoint(ByVal this As pair(Of IPAddress, UInt16)) As IPEndPoint
        If this Is Nothing OrElse this.first Is Nothing Then
            Return Nothing
        End If
        Return New IPEndPoint(this.first, this.second)
    End Function

    <Extension()> Public Function to_ipendpoint(ByVal this As const_pair(Of IPAddress, UInt16)) As IPEndPoint
        If this Is Nothing OrElse this.first Is Nothing Then
            Return Nothing
        End If
        Return New IPEndPoint(this.first, this.second)
    End Function
End Module
