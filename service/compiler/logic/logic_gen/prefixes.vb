﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants
Imports osi.root.formation
Imports osi.service.compiler.logic

Public Interface prefix
    Sub export(ByVal o As writer)
End Interface

Public NotInheritable Class prefixes
    Private ReadOnly v As vector(Of prefix)

    Public Sub New()
        v = New vector(Of prefix)()
    End Sub

    Public Sub register(ByVal p As prefix)
        v.emplace_back(p)
    End Sub

    Public Sub export(ByVal o As writer)
        assert(Not o Is Nothing)
        Dim i As UInt32 = 0
        While i < v.size()
            v(i).export(o)
            i += uint32_1
        End While
    End Sub
End Class