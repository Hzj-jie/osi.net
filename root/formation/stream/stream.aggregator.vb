﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector

Partial Public Class stream(Of T)
    Public NotInheritable Class aggregators
        Public Shared ReadOnly sum As Func(Of T, T, T) = Function(ByVal l As T, ByVal r As T) As T
                                                             Return binary_operator.add(l, r)
                                                         End Function

        Public Shared ReadOnly max As Func(Of T, T, T) = Function(ByVal l As T, ByVal r As T) As T
                                                             Return _minmax.max(l, r)
                                                         End Function

        Public Shared ReadOnly min As Func(Of T, T, T) = Function(ByVal l As T, ByVal r As T) As T
                                                             Return _minmax.min(l, r)
                                                         End Function

        Private Sub New()
        End Sub
    End Class
End Class