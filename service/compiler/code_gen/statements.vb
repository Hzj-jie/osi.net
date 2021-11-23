
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation

Public Interface statement(Of WRITER)
    Sub export(ByVal o As WRITER)
End Interface

Public Class statements(Of WRITER)
    Private ReadOnly v As New vector(Of statement(Of WRITER))()

    Public Sub register(ByVal p As statement(Of WRITER))
        v.emplace_back(p)
    End Sub

    Public Sub export(ByVal o As WRITER)
        assert(Not o Is Nothing)
        Dim i As UInt32 = 0
        While i < v.size()
            v(i).export(o)
            i += uint32_1
        End While
    End Sub
End Class
