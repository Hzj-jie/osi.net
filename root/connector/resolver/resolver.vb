
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with resolver.vbp ----------
'so change resolver.vbp instead of this file



Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public NotInheritable Class resolver(Of T As Class)
#If False Then
    Private ReadOnly l As New Object()
#End If
    Private f As Func(Of T)
    Private cached As T

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function registered() As Boolean
        Return cached IsNot Nothing OrElse f IsNot Nothing
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub register(ByVal f As Func(Of T),
                         ByVal cached As T,
                         ByVal when_registered As Boolean,
                         ByVal when_not_registered As Boolean)
#If False Then
        SyncLock l
#End If
        If registered() Then
            assert(when_registered)
        Else
            assert(when_not_registered)
        End If
        Me.f = f
        Me.cached = cached
#If False Then
        End SyncLock
#End If
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub register(ByVal i As T)
        register(Nothing, i, True, True)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub register(ByVal i As Func(Of T))
        register(i, Nothing, True, True)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub unregister()
        register(Nothing, Nothing, True, True)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_first_register(ByVal i As T)
        assert(i IsNot Nothing)
        register(Nothing, i, False, True)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_first_register(ByVal i As Func(Of T))
        assert(i IsNot Nothing)
        register(i, Nothing, False, True)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_unregister()
        register(Nothing, Nothing, True, False)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function resolve(ByRef o As T) As Boolean
        If cached Is Nothing Then
#If False Then
            SyncLock l
#End If
            If cached Is Nothing Then
                If f Is Nothing Then
                    Return False
                End If
                cached = f()
            End If
#If False Then
            End SyncLock
#End If
        End If
        assert(cached IsNot Nothing)
        o = cached
        Return True
    End Function
End Class
'finish resolver.vbp --------
