
Option Explicit On
Option Infer Off
Option Strict On

Imports osi.root.connector
Imports osi.root.constants

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

        Public Shared Function percentile(ByVal p As Double) As Func(Of T, T, T)
            assert(p >= 0)
            assert(p <= 1)
            Dim v As vector(Of T) = New vector(Of T)()
            Return Function(ByVal last As T, ByVal current As T) As T
                       If v.empty() Then
                           v.emplace_back(last)
                       End If
                       v.emplace_back(current)
                       Dim index As UInt32 = CUInt(p * v.size())
                       If index = v.size() Then
                           index -= uint32_1
                       End If
                       Return v(index)
                   End Function
        End Function

        Private Sub New()
        End Sub
    End Class
End Class
