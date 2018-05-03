
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector

Partial Public NotInheritable Class invoker
    Public Class builder_base(Of T As builder_base(Of T))
        Protected binding_flags As BindingFlags = constants.binding_flags.all_method
        Protected name As String
        Protected suppress_error As Boolean = +suppress.invoker_error

        Private Function this() As T
            Return direct_cast(Of T)(Me)
        End Function

        Public Function with_binding_flags(ByVal binding_flags As BindingFlags) As T
            Me.binding_flags = binding_flags
            Return this()
        End Function

        Public Function for_instance_public_methods() As T
            Return with_binding_flags(constants.binding_flags.instance_public_method)
        End Function

        Public Function for_instance_methods() As T
            Return with_binding_flags(constants.binding_flags.instance_all_method)
        End Function

        Public Function for_static_public_methods() As T
            Return with_binding_flags(constants.binding_flags.static_public_method)
        End Function

        Public Function for_static_methods() As T
            Return with_binding_flags(constants.binding_flags.static_all_method)
        End Function

        Public Function with_name(ByVal name As String) As T
            Me.name = name
            Return this()
        End Function

        Public Function with_suppress_error(ByVal suppress_error As Boolean) As T
            Me.suppress_error = suppress_error
            Return this()
        End Function
    End Class
End Class
