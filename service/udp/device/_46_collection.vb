
Imports System.Net
Imports osi.root.template
Imports osi.root.delegates
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.formation

Public MustInherit Class _46_collection(Of T, __NEW As __do(Of powerpoint, UInt16, T, Boolean))
    Public Const port_count As Int32 = IPEndPoint.MaxPort - IPEndPoint.MinPort + 1
    Private Shared ReadOnly _new As _do(Of powerpoint, UInt16, T, Boolean)
    Private Shared ReadOnly v4 As arrayless(Of T)
    Private Shared ReadOnly v6 As arrayless(Of T)

    Shared Sub New()
        _new = +alloc(Of __NEW)()
        v4 = New arrayless(Of T)(port_count)
        v6 = New arrayless(Of T)(port_count)
    End Sub

    Private Shared Function _new_T(ByVal p As powerpoint) As Func(Of UInt32, T)
        Return Function(ByVal local_port As UInt32) As T
                   Dim r As T = Nothing
                   If _new(unref(p), unref(local_port), r) Then
                       Return r
                   Else
                       Return Nothing
                   End If
               End Function
    End Function

    Public Shared Function [New](ByVal p As powerpoint, ByVal local_port As UInt16, ByRef o As T) As Boolean
        If p IsNot Nothing Then
            If p.local_defined() Then
                local_port = p.local_port
            End If
            If local_port = socket_invalid_port Then
                Return False
            Else
                Return assert(If(p.ipv4, v4, v6).[New](local_port, _new_T(p), o))
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function [New](ByVal p As powerpoint, ByRef o As T) As Boolean
        Return [New](p, socket_invalid_port, o)
    End Function

    Public Shared Function [New](ByVal p As powerpoint, ByVal local_port As UInt16) As T
        Dim c As T = Nothing
        assert([New](p, local_port, c))
        Return c
    End Function

    Public Shared Function [New](ByVal p As powerpoint) As T
        Dim c As T = Nothing
        assert([New](p, c))
        Return c
    End Function

    Public Shared Function [next](ByVal p As powerpoint, ByRef port As UInt16, ByRef o As T) As Boolean
        If p IsNot Nothing Then
            Return If(p.ipv4, v4, v6).next(_new_T(p), port, o)
        Else
            Return False
        End If
    End Function

    Public Shared Function [next](ByVal p As powerpoint, ByRef port As UInt16) As T
        Dim c As T = Nothing
        assert([next](p, port, c))
        Return c
    End Function

    Public Shared Function [erase](ByVal p As powerpoint, ByVal port As UInt16) As Boolean
        If p IsNot Nothing Then
            If p.local_defined() Then
                port = p.local_port
            End If
            If port = socket_invalid_port Then
                Return False
            Else
                Return If(p.ipv4, v4, v6).erase(port)
            End If
        Else
            Return False
        End If
    End Function

    Public Shared Function [erase](ByVal p As powerpoint) As Boolean
        Return [erase](p, socket_invalid_port)
    End Function

    Protected Sub New()
    End Sub
End Class
