
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with thread_safe_resolver.vbp ----------
'so change thread_safe_resolver.vbp instead of this file


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with resolver.vbp ----------
'so change resolver.vbp instead of this file



Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public NotInheritable Class thread_safe_resolver(Of T As Class)
#If True Then
    Private ReadOnly l As New Object()
#End If
    Private f As Func(Of T)
    Private cached As T

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function registered() As Boolean
        Return Not cached Is Nothing OrElse Not f Is Nothing
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Sub register(ByVal f As Func(Of T),
                         ByVal cached As T,
                         ByVal when_registered As Boolean,
                         ByVal when_not_registered As Boolean)
#If True Then
        SyncLock l
#End If
        If registered() Then
            assert(when_registered)
        Else
            assert(when_not_registered)
        End If
        Me.f = f
        Me.cached = cached
#If True Then
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
        assert(Not i Is Nothing)
        register(Nothing, i, False, True)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_first_register(ByVal i As Func(Of T))
        assert(Not i Is Nothing)
        register(i, Nothing, False, True)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Sub assert_unregister()
        register(Nothing, Nothing, True, False)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Function resolve(ByRef o As T) As Boolean
        If cached Is Nothing Then
#If True Then
            SyncLock l
#End If
            If cached Is Nothing Then
                If f Is Nothing Then
                    Return False
                End If
                cached = f()
            End If
#If True Then
            End SyncLock
#End If
        End If
        assert(Not cached Is Nothing)
        o = cached
        Return True
    End Function
End Class
'finish resolver.vbp --------
'finish thread_safe_resolver.vbp --------
