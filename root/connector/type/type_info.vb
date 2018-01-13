
Option Explicit On
Option Infer Off
Option Strict On

Partial Public NotInheritable Class type_info(Of T)
    ' Do not add type, refer to gettype_perf test, since this is a template class, caching GetType(T) result cannot
    ' provide benefit.
    ' Public Shared ReadOnly type As Type
    Public Shared ReadOnly fullname As String
    Public Shared ReadOnly name As String

    ' => GetType(T) Is GetType(Object)
    Public Shared ReadOnly is_object As Boolean
    ' => GetType(T).IsValueType
    Public Shared ReadOnly is_valuetype As Boolean
    ' => GetType(T).IsInterface
    Public Shared ReadOnly is_interface As Boolean
    Public Shared ReadOnly is_cloneable As Boolean
    Public Shared ReadOnly is_cloneable_T As Boolean
    Public Shared ReadOnly is_delegate As Boolean
    ' => GetType(T).IsPrimitive
    Public Shared ReadOnly is_primitive As Boolean
    Public Shared ReadOnly is_nullable As Boolean
    ' => GetType(T) Is GetType(String)
    Public Shared ReadOnly is_string As Boolean

    Shared Sub New()
        fullname = GetType(T).FullName()
        name = GetType(T).Name()
        is_object = (GetType(T) Is GetType(Object))
        is_valuetype = GetType(T).IsValueType()
        is_interface = GetType(T).IsInterface()
        is_cloneable = If(is_interface, type_info(Of T, type_info_operators.interface_inherit, ICloneable).v,
                                        type_info(Of T, type_info_operators.implement, ICloneable).v)
        is_cloneable_T = If(is_interface, type_info(Of T, type_info_operators.interface_inherit, ICloneable(Of T)).v,
                                          type_info(Of T, type_info_operators.implement, ICloneable(Of T)).v)
        is_delegate = type_info(Of T, type_info_operators.inherit, [Delegate]).v
        is_primitive = GetType(T).IsPrimitive()
        is_nullable = GetType(T).is(GetType(Nullable(Of )))
        is_string = GetType(T) Is GetType(String)
    End Sub

    Public Shared Function has_finalizer() As Boolean
        Return finalizer_cache.has()
    End Function

    Public Shared Function finalizer() As Action(Of T)
        Return finalizer_cache.v
    End Function

    Public Shared Function annotated_constructor(Of ATTR)() As Func(Of Object(), T)
        Return annotated_constructor_cache(Of ATTR).v.typed
    End Function

    Public Shared Function public_parameterless_constructor() As Func(Of T)
        Return constructor_cache.parameterless_public.typed
    End Function

    Public Shared Function parameterless_constructor() As Func(Of T)
        Return constructor_cache.parameterless.typed
    End Function

    Public Shared Function parameters_constructor() As Func(Of Object(), T)
        Return constructor_cache.parameters.typed
    End Function

    Public Shared Function default_parameters_constructor() As Func(Of T)
        Return constructor_cache.default_parameters.typed
    End Function

    Public Shared Function dominated_constructor() As Func(Of T)
        Return constructor_cache.dominated.typed
    End Function

    Public Shared Function size() As Int32
        Return size_cache.size
    End Function

    Public Shared Function size_uint32() As UInt32
        assert(size() >= 0)
        Return CUInt(size())
    End Function

    Private Sub New()
    End Sub
End Class
