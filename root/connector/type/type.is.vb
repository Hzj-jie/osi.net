
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Runtime.CompilerServices

Public Module _is
    <Extension()> Public Function [is](ByVal this As Type, ByVal base As Type) As Boolean
        If this Is Nothing AndAlso base Is Nothing Then
            Return True
        End If
        If this Is Nothing OrElse base Is Nothing Then
            Return False
        End If
        If this Is base Then
            Return True
        End If
        If base.IsGenericTypeDefinition() AndAlso this.is_unspecified_generic_type() Then
            Return (this.GetGenericTypeDefinition().is(base))
        End If
        If base.IsInterface() Then
            If this.IsInterface() Then
                Return interface_inherit(this, base)
            End If
            Return implement(this, base)
        End If
        Return inherit(this, base)
    End Function

    <Extension()> Public Function generic_type_is(ByVal this As Type, ByVal base As Type) As Boolean
        Return base.IsGenericTypeDefinition() AndAlso
               this.IsGenericType() AndAlso
               Not this.IsGenericTypeDefinition() AndAlso
               this.GetGenericTypeDefinition() Is base
    End Function

    <Extension()> Public Function [is](Of T)(ByVal this As Type) As Boolean
        Return [is](this, GetType(T))
    End Function

    <Extension()> Public Function is_unspecified_generic_type(ByVal this As Type) As Boolean
        If this Is Nothing Then
            Return False
        End If
        Return this.IsGenericType() AndAlso Not this.IsGenericTypeDefinition()
    End Function

    <Extension()> Public Function generic_type_definition_is(ByVal this As Type,
                                                             ByVal generic_type_definition As Type) As Boolean
        If this Is Nothing OrElse generic_type_definition Is Nothing Then
            Return False
        End If
        If Not generic_type_definition.IsGenericTypeDefinition() Then
            Return False
        End If
        Return this.IsGenericType() AndAlso this.GetGenericTypeDefinition() Is generic_type_definition
    End Function
End Module