
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public NotInheritable Class method_impl_options
#If DEBUG Then
    Public Const aggressive_inlining As MethodImplOptions = zero
#Else
    Public Const aggressive_inlining As MethodImplOptions = MethodImplOptions.NoInlining
    'aggressive-inline + aggressive-optimization
    'Public Const aggressive_inlining As MethodImplOptions = DirectCast(256 Or 512, MethodImplOptions)
#End If

    Public Const zero As MethodImplOptions = DirectCast(0, MethodImplOptions)

    Public Const no_inlining As MethodImplOptions = MethodImplOptions.NoInlining

    Private Sub New()
    End Sub
End Class
