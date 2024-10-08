
Option Explicit On
Option Infer Off
Option Strict On

#Const use_thread_static = False
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.constants

Public NotInheritable Class instance_stack(Of T)
    Inherits instance_stack(Of T, Object)

    Private Sub New()
    End Sub
End Class

Public Class instance_stack(Of T, PROTECTOR)
    'the implementation of threadstatic valuetype object is not consistent between mono and .net
    'while .net has boxing by default
#If use_thread_static Then
    Private Shared ReadOnly c As New thread_static(Of stack(Of T))()
#Else
    <ThreadStatic()> Private Shared c As stack(Of T)
#End If

    Public Shared Function back(ByRef o As T) As Boolean
        Dim s As stack(Of T) = thread_stack()
        If s Is Nothing OrElse s.empty() Then
            Return False
        End If
        o = s.back()
        Return True
    End Function

    Public Shared Property current() As T
        Get
            Dim s As stack(Of T) = thread_stack()
            assert(Not s Is Nothing)
            Return s.back()
        End Get
        Set(ByVal value As T)
            If value Is Nothing Then
                pop()
            Else
                push(value)
            End If
        End Set
    End Property

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Function thread_stack() As stack(Of T)
        Dim s As stack(Of T) = Nothing
#If use_thread_static Then
        s = (+c)
#Else
        s = c
#End If
        Return s
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Private Shared Function thread_stack_or_new() As stack(Of T)
        Dim s As stack(Of T) = thread_stack()
        If s Is Nothing Then
            s = New stack(Of T)()
#If use_thread_static Then
            c.set(s)
#Else
            c = s
#End If
        End If
        Return s
    End Function

    Public Shared Sub push(ByVal v As T)
        assert(Not v Is Nothing)
        thread_stack_or_new().emplace(v)
    End Sub

    Public Shared Sub pop()
        Dim s As stack(Of T) = thread_stack()
        assert(Not s Is Nothing)
        s.pop()
    End Sub

    Public Shared Function empty() As Boolean
        Dim s As stack(Of T) = thread_stack()
        Return s Is Nothing OrElse s.empty()
    End Function

    Public Shared Function size() As UInt32
        Dim s As stack(Of T) = thread_stack()
        Return If(s Is Nothing, uint32_0, s.size())
    End Function

    Public Shared Function [with](ByVal v As T) As IDisposable
        push(v)
        Return defer.to(AddressOf pop)
    End Function

    Protected Sub New()
    End Sub
End Class
