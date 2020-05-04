
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _debug_mode
    Private PAUSEWHENINDEBUGMODE As Boolean = True

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function strongassert() As Boolean
        Return Not PAUSEWHENINDEBUGMODE
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub setstrongassert()
        PAUSEWHENINDEBUGMODE = False
    End Sub

    Private INDEBUGMODE As Boolean = False
    Private NOTINDEBUGMODE As Boolean = False

    Public Sub set_debug_mode()
        INDEBUGMODE = True
        NOTINDEBUGMODE = False
    End Sub

    Public Sub set_not_debug_mode()
        INDEBUGMODE = False
        NOTINDEBUGMODE = True
    End Sub

    Public Sub set_normal_mode()
        INDEBUGMODE = False
        NOTINDEBUGMODE = False
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function isreleasebuild() As Boolean
        Return Not isdebugbuild()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function isdebugbuild() As Boolean
#If DEBUG Then
        Return True
#Else
        Return False
#End If
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function isdebugmode() As Boolean
        If INDEBUGMODE Then
            Return True
        End If
        If NOTINDEBUGMODE Then
            Return False
        End If
        Return isdebugbuild()
    End Function
End Module
