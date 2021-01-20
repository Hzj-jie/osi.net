
Option Explicit On
Option Infer Off
Option Strict On

'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with thread_static_resolver.vbp ----------
'so change thread_static_resolver.vbp instead of this file



'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with resolver_wrapper.vbp ----------
'so change resolver_wrapper.vbp instead of this file



Imports System.Runtime.CompilerServices
Imports osi.root.constants

' Auto-infer is error-prone. E.g.
' thread_static_resolver.register(Of T)(ByVal i As Func(Of T))
' cannot work as expected, the compiler automatically infers to
' thread_static_resolver.register(Of T As Func(Of ?))(ByVal i As T).

Public NotInheritable Class thread_static_resolver(Of T As Class)
    Inherits thread_static_resolver(Of T, Object)

    Private Sub New()
    End Sub
End Class

Public Class thread_static_resolver(Of T As Class, PROTECTOR)
    Private NotInheritable Class unregister_delegate
        Implements IDisposable

        Public Shared ReadOnly instance As unregister_delegate = New unregister_delegate()

        Private Sub New()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            unregister()
        End Sub
    End Class


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with thread_static_resolver_impl.vbp ----------
'so change thread_static_resolver_impl.vbp instead of this file


    <ThreadStatic()> Private Shared resolver As resolver(Of T)

    Private Shared Function create_resolver() As resolver(Of T)
        Dim r As resolver(Of T) = Nothing
        r = resolver
        If r Is Nothing Then
            r = New resolver(Of T)()
            resolver = r
        End If
        assert(Not r Is Nothing)
        Return r
    End Function

'finish thread_static_resolver_impl.vbp --------
    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub register(ByVal i As T)
        create_resolver().register(i)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub register(ByVal i As Func(Of T))
        create_resolver().register(i)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub assert_first_register(ByVal i As T)
        create_resolver().assert_first_register(i)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub assert_first_register(ByVal i As Func(Of T))
        create_resolver().assert_first_register(i)
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Sub unregister()
        create_resolver().unregister()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared sub assert_unregister()
        create_resolver().assert_unregister()
    End Sub

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function resolve(ByRef o As T) As Boolean
        Return create_resolver().resolve(o)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function resolve(Of RT As T)(ByRef r As RT) As Boolean
        Dim o As T = Nothing
        Return resolve(o) AndAlso direct_cast(o, r)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function registered() As Boolean
        Return create_resolver().registered()
    End Function


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with resolver_wrapper_resolve.vbp ----------
'so change resolver_wrapper_resolve.vbp instead of this file



    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function resolve(Of RT As T)() As RT
        Dim o As RT = Nothing
        assert(resolve(o))
        Return o
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function resolve_or_null(Of RT As T)() As RT
        Return resolve_or_default([default](Of RT).null)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function resolve_or_default(Of RT As T)(ByVal [default] As RT) As RT
        Dim o As RT = Nothing
        If resolve(o) Then
            Return o
        End If
        Return [default]
    End Function

'finish resolver_wrapper_resolve.vbp --------


'the following code is generated by /osi/root/codegen/precompile/precompile.exe
'with resolver_wrapper_resolve.vbp ----------
'so change resolver_wrapper_resolve.vbp instead of this file



    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function resolve() As T
        Dim o As T = Nothing
        assert(resolve(o))
        Return o
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function resolve_or_null() As T
        Return resolve_or_default([default](Of T).null)
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function resolve_or_default(ByVal [default] As T) As T
        Dim o As T = Nothing
        If resolve(o) Then
            Return o
        End If
        Return [default]
    End Function

'finish resolver_wrapper_resolve.vbp --------

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function scoped_register(ByVal i As T) As IDisposable
        assert_first_register(i)
        Return unregister_delegate.instance
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function scoped_register(ByVal i As Func(Of T)) As IDisposable
        assert_first_register(i)
        Return unregister_delegate.instance
    End Function

    Protected Sub New()
    End Sub
End Class

'finish resolver_wrapper.vbp --------
'finish thread_static_resolver.vbp --------
