
Option Explicit On
Option Infer Off
Option Strict On

#Const use_thread_static = False
Imports osi.root.connector
Imports osi.root.constants

Public Class instance_stack(Of T)
    'the implementation of threadstatic valuetype object is not consistent between mono and .net
    'while .net has boxing by default
#If use_thread_static Then
    Private Shared ReadOnly c As thread_static(Of stack(Of T))

    Shared Sub New()
        c = New thread_static(Of stack(Of T))()
    End Sub
#Else
    <ThreadStatic()> Private Shared c As stack(Of T)
#End If

    Public Shared Property current() As T
        Get
#If use_thread_static Then
#If DEBUG Then
            assert(Not (+c) Is Nothing)
#End If
            Return (+c).back()
#Else
#If DEBUG Then
            assert(Not c Is Nothing)
#End If
            Return c.back()
#End If
        End Get
        Set(ByVal value As T)
            Dim s As stack(Of T) = Nothing
#If use_thread_static Then
            s = (+c)
#Else
            s = c
#End If
            If s Is Nothing Then
                s = New stack(Of T)()
#If use_thread_static Then
                c.set(s)
#Else
                c = s
#End If
            End If
            If value Is Nothing Then
                s.pop()
            Else
                s.emplace(value)
            End If
        End Set
    End Property

    Public Shared Function empty() As Boolean
        Dim s As stack(Of T) = Nothing
#If use_thread_static Then
        s = (+c)
#Else
        s = c
#End If
        Return s Is Nothing OrElse s.empty()
    End Function

    Public Shared Function size() As UInt32
        Dim s As stack(Of T) = Nothing
#If use_thread_static Then
        s = (+c)
#Else
        s = c
#End If
        Return If(s Is Nothing, uint32_0, s.size())
    End Function
End Class
