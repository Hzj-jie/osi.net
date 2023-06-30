
Option Explicit On
Option Infer Off
Option Strict On

#Const AGRESSIVE_INLINING = True

Imports System.Runtime.CompilerServices

Public NotInheritable Class math_debug
#If AGRESSIVE_INLINING Then
    Public Const aggressive_inlining As MethodImplOptions = root.constants.method_impl_options.aggressive_inlining
#Else
    Public Const aggressive_inlining As MethodImplOptions = MethodImplOptions.NoInlining
#End If

    Private Sub New()
    End Sub
End Class
