﻿
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection

Public NotInheritable Class copy_constructorAttribute
    Inherits Attribute

    Public Shared ReadOnly type As Type

    Shared Sub New()
        type = GetType(copy_constructorAttribute)
    End Sub
End Class

' Retrieve the copy or move constructor of type T.
Public NotInheritable Class copy_constructor(Of T)
    Private Shared ReadOnly v As Func(Of Object(), Object)

    Shared Sub New()
        Dim constructors() As ConstructorInfo = Nothing
        constructors = GetType(T).constructors()
        For i As Int32 = 0 To array_size_i(constructors) - 1
            Dim cc As copy_constructorAttribute = Nothing
            If constructors(i).custom_attribute(cc) Then
                v = AddressOf constructors(i).Invoke
                Return
            End If
        Next
        assert(False)
    End Sub

    Public Shared Function invoke(ByVal ParamArray args() As Object) As T
        Try
            Return direct_cast(Of T)(v(args))
        Catch ex As Exception
            log_unhandled_exception(ex)
            assert(False)
            Return Nothing
        End Try
    End Function

    Private Sub New()
    End Sub
End Class
