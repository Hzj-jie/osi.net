
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class invoker
    Public MustInherit Class invoker_builder(Of RT)
        Inherits builder_base(Of RT, invoker_builder(Of RT))

        Protected type As Type
        Protected obj As Object

        Public Function with_type(ByVal type As Type) As invoker_builder(Of RT)
            Me.type = type
            Return Me
        End Function

        Public Function with_type(Of T)() As invoker_builder(Of RT)
            Return with_type(GetType(T))
        End Function

        Public Function with_object(ByVal obj As Object) As invoker_builder(Of RT)
            Me.obj = obj
            If type IsNot Nothing OrElse obj Is Nothing Then
                Return Me
            End If
            Return with_type(obj.GetType())
        End Function
    End Class

    Public NotInheritable Class builder
        Inherits invoker_builder(Of invoker)

        Public Overrides Function build(ByRef o As invoker) As Boolean
            Return invoker.[New](type, binding_flags, obj, name, suppress_error, o)
        End Function
    End Class

    Public NotInheritable Class builder(Of delegate_t)
        Inherits invoker_builder(Of invoker(Of delegate_t))

        Public Overrides Function build(ByRef o As invoker(Of delegate_t)) As Boolean
            Return invoker(Of delegate_t).[New](type, binding_flags, obj, name, suppress_error, o)
        End Function
    End Class

    Public Shared Function [of]() As builder
        Return New builder()
    End Function

    Public Shared Function [of](ByVal i As invoker) As builder
        Return [of]()
    End Function

    Public Shared Function [of](Of delegate_t)() As builder(Of delegate_t)
        Return New builder(Of delegate_t)()
    End Function

    Public Shared Function [of](Of delegate_t)(ByVal i As invoker(Of delegate_t)) As builder(Of delegate_t)
        Return [of](Of delegate_t)()
    End Function
End Class
