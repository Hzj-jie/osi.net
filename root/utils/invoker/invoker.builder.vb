
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection

Public NotInheritable Class invoker
    Public NotInheritable Class builder(Of delegate_t)
        Private binding_flags As BindingFlags = constants.binding_flags.all
        Private type As Type
        Private obj As Object
        Private name As String
        Private suppress_error As Boolean = suppress.invoker_error

        Public Function with_binding_flags(ByVal binding_flags As BindingFlags) As builder(Of delegate_t)
            Me.binding_flags = binding_flags
            Return Me
        End Function

        Public Function for_instance_public_methods() As builder(Of delegate_t)
            Return with_binding_flags(constants.binding_flags.instance_public_method)
        End Function

        Public Function for_instance_methods() As builder(Of delegate_t)
            Return with_binding_flags(constants.binding_flags.instance_all_method)
        End Function

        Public Function for_static_public_methods() As builder(Of delegate_t)
            Return with_binding_flags(constants.binding_flags.static_public_method)
        End Function

        Public Function for_static_methods() As builder(Of delegate_t)
            Return with_binding_flags(constants.binding_flags.static_all_method)
        End Function

        Public Function with_type(ByVal type As Type) As builder(Of delegate_t)
            Me.type = type
            Return Me
        End Function

        Public Function with_type(Of T)() As builder(Of delegate_t)
            Return with_type(GetType(T))
        End Function

        Public Function with_object(ByVal obj As Object) As builder(Of delegate_t)
            Me.obj = obj
            Return Me
        End Function

        Public Function with_name(ByVal name As String) As builder(Of delegate_t)
            Me.name = name
            Return Me
        End Function

        Public Function with_suppress_error(ByVal suppress_error As Boolean) As builder(Of delegate_t)
            Me.suppress_error = suppress_error
            Return Me
        End Function

        Public Function build() As invoker(Of delegate_t)
            Return invoker(Of delegate_t).[New](type, binding_flags, obj, name, suppress_error)
        End Function
    End Class

    Public Shared Function [of](Of delegate_t)() As builder(Of delegate_t)
        Return New builder(Of delegate_t)()
    End Function

    Private Sub New()
    End Sub
End Class
