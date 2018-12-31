
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
        If base.is_unspecified_generic_type() AndAlso this.is_specified_generic_type() Then
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
        Return base.is_unspecified_generic_type() AndAlso
               this.is_specified_generic_type() AndAlso
               this.GetGenericTypeDefinition() Is base
    End Function

    <Extension()> Public Function [is](Of T)(ByVal this As Type) As Boolean
        Return [is](this, GetType(T))
    End Function

    <Extension()> Public Function is_specified_generic_type(ByVal this As Type) As Boolean
        If this Is Nothing Then
            Return False
        End If
        Return this.IsGenericType() AndAlso Not this.IsGenericTypeDefinition()
    End Function

    <Extension()> Public Function is_unspecified_generic_type(ByVal this As Type) As Boolean
        If this Is Nothing Then
            Return False
        End If
        Return this.IsGenericType() AndAlso this.IsGenericTypeDefinition()
    End Function

    <Extension()> Public Function generic_type_of_one_interface_is(ByVal this As Type, ByVal base As Type) As Boolean
        If this Is Nothing OrElse base Is Nothing Then
            Return False
        End If
        If Not base.is_unspecified_generic_type() Then
            Return False
        End If
        Dim ints() As Type = Nothing
        ints = this.GetInterfaces()
        For i As Int32 = 0 To array_size_i(ints) - 1
            If ints(i).generic_type_is(base) Then
                Return True
            End If
        Next
        Return False
    End Function
End Module