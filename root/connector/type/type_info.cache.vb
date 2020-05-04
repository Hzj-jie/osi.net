
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Partial Public NotInheritable Class type_info(Of T)
    Private NotInheritable Class finalizer_cache
        Public Shared ReadOnly f As MethodInfo
        Public Shared ReadOnly v As Action(Of T)

        Shared Sub New()
            f = GetType(T).finalizer()
            If Not f Is Nothing Then
                v = Sub(ByVal i As Object)
                        f.Invoke(i, Nothing)
                    End Sub
            End If
        End Sub

        <MethodImpl(method_impl_options.aggressive_inlining)>
        Public Shared Function has() As Boolean
            Return Not f Is Nothing
        End Function

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class annotated_constructor_cache(Of ATTR)
        Public Shared ReadOnly info As ConstructorInfo
        Public Shared ReadOnly v As typed_parameters_constructor

        Shared Sub New()
            info = GetType(T).annotated_constructor_info(Of ATTR)()
            v = New typed_parameters_constructor(GetType(T).annotated_constructor(Of ATTR)())
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class constructor_cache
        Public Shared ReadOnly parameterless_public As typed_constructor
        Public Shared ReadOnly parameterless As typed_constructor
        Public Shared ReadOnly parameters As typed_parameters_constructor
        Public Shared ReadOnly default_parameters As typed_constructor
        Public Shared ReadOnly dominated As typed_constructor

        Shared Sub New()
            Dim type As Type = Nothing
            type = GetType(T)
            If type.allocatable() Then
                parameterless_public = New typed_constructor(type.public_parameterless_constructor())
                parameterless = New typed_constructor(type.parameterless_constructor())
                parameters = New typed_parameters_constructor(type.parameters_constructor())
                default_parameters = New typed_constructor(type.default_parameters_constructor())
                If Not parameterless_public.empty() Then
                    dominated = parameterless_public
                ElseIf Not parameterless.empty() Then
                    dominated = parameterless
                ElseIf Not default_parameters.empty() Then
                    dominated = default_parameters
                End If
            End If
            If dominated Is Nothing Then
                dominated = New typed_constructor(Function() As Object
                                                      Return Nothing
                                                  End Function)
            End If
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private NotInheritable Class size_cache
        Public Shared ReadOnly size As Int32

        Shared Sub New()
            size = sizeof(GetType(T))
        End Sub
    End Class
End Class
