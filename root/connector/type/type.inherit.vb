
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _inherit
    <Extension()> Public Function interface_inherit(ByVal t As Type, ByVal base As Type) As Boolean
        If Not t.IsInterface() OrElse Not base.IsInterface() Then
            Return False
        ElseIf t Is base Then
            Return False
        Else
            Return base.IsAssignableFrom(t)
        End If
    End Function

    <Extension()> Public Function inherit(ByVal t As Type, ByVal base As Type) As Boolean
        If t Is Nothing AndAlso base Is Nothing Then
            Return True
        End If
        If t Is Nothing OrElse base Is Nothing Then
            Return False
        End If
        If t Is base Then
            Return False
        End If
        If t.IsInterface() AndAlso base.IsInterface() Then
            Return interface_inherit(t, base)
        End If
        If t.IsInterface() OrElse base.IsInterface() Then
            Return False
        End If
        If t.IsGenericType() AndAlso base.IsGenericTypeDefinition() Then
            'object is not a generic type
            While t.IsGenericType() 'AndAlso Not t Is GetType(Object)
                If t.GetGenericTypeDefinition() Is base Then
                    Return True
                End If
                t = t.BaseType()
            End While
            Return False
        End If
        Return t.IsSubclassOf(base)
    End Function

    <Extension()> Public Function inherit(Of base)(ByVal t As Type) As Boolean
        Return inherit(t, GetType(base))
    End Function

    <Extension()> Public Function implement(ByVal t As Type, ByVal base As Type) As Boolean
        If t Is Nothing AndAlso base Is Nothing Then
            Return True
        End If
        If t Is Nothing OrElse base Is Nothing Then
            Return False
        End If
        If Not base.IsInterface() Then
            Return False
        End If
        If base.IsGenericTypeDefinition() Then
            Dim ints() As Type = Nothing
            ints = t.GetInterfaces()
            For i As Int32 = 0 To array_size_i(ints) - 1
                If ints(i).IsGenericType() AndAlso ints(i).GetGenericTypeDefinition() Is base Then
                    Return True
                End If
            Next
            Return False
        End If
#If RETIRED Then
        Try
            t.GetInterfaceMap(base)
        Catch
            Return False
        End Try
        Return True
#End If
        Return base.IsAssignableFrom(t)
    End Function

    <Extension()> Public Function implement(Of base)(ByVal t As Type) As Boolean
        Return implement(t, GetType(base))
    End Function
End Module
