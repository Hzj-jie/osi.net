﻿
Option Explicit On
Option Infer Off
Option Strict On

Public NotInheritable Class invoker
    Public NotInheritable Class builder(Of delegate_t)
        Inherits builder_base(Of builder(Of delegate_t))

        Private type As Type
        Private obj As Object

        Public Function with_type(ByVal type As Type) As builder(Of delegate_t)
            Me.type = type
            Return Me
        End Function

        Public Function with_type(Of T)() As builder(Of delegate_t)
            Return with_type(GetType(T))
        End Function

        Public Function with_object(ByVal obj As Object) As builder(Of delegate_t)
            Me.obj = obj
            If Not type Is Nothing OrElse obj Is Nothing Then
                Return Me
            End If
            Return with_type(obj.GetType())
        End Function

        Public Function build(ByRef o As invoker(Of delegate_t)) As Boolean
            Return invoker(Of delegate_t).[New](type, binding_flags, obj, name, suppress_error, o)
        End Function

        Public Function build() As invoker(Of delegate_t)
            Return invoker(Of delegate_t).[New](type, binding_flags, obj, name, suppress_error)
        End Function
    End Class

    Public Shared Function [of](Of delegate_t)() As builder(Of delegate_t)
        Return New builder(Of delegate_t)()
    End Function

    Public Shared Function [of](Of delegate_t)(ByVal i As invoker(Of delegate_t)) As builder(Of delegate_t)
        Return [of](Of delegate_t)()
    End Function

    Private Sub New()
    End Sub
End Class
