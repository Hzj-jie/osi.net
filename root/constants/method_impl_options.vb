
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public NotInheritable Class method_impl_options
#If DEBUG Then
    Public Const aggressive_inlining As MethodImplOptions = DirectCast(0, MethodImplOptions)
#Else
    'aggressive-inline + aggressive-optimization
    Public Const aggressive_inlining As MethodImplOptions = DirectCast(256 Or 512, MethodImplOptions)
#End If

    Private Sub New()
    End Sub
End Class
