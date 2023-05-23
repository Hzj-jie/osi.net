
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.delegates

Partial Public NotInheritable Class type_info(Of T)
    ' Do not add type, refer to gettype_perf test, since this is a template class, caching GetType(T) result cannot
    ' provide benefit.
    ' Public Shared ReadOnly type As Type
    Public Shared ReadOnly assembly_qualified_name As String = GetType(T).AssemblyQualifiedName()
    Public Shared ReadOnly fullname As String = GetType(T).FullName()
    Public Shared ReadOnly name As String = GetType(T).Name()
    Public Shared ReadOnly name_without_generic_arity As String = Function() As String
                                                                      If name.IndexOf("`") = npos Then
                                                                          Return name
                                                                      End If
                                                                      Return name.Substring(0, name.IndexOf("`"))
                                                                  End Function()

    ' => GetType(T) Is GetType(Object)
    Public Shared ReadOnly is_object As Boolean = (GetType(T) Is GetType(Object))
    ' => GetType(T).IsValueType
    Public Shared ReadOnly is_valuetype As Boolean = GetType(T).IsValueType()
    ' => GetType(T).IsInterface
    Public Shared ReadOnly is_interface As Boolean = GetType(T).IsInterface()
    Public Shared ReadOnly is_cloneable As Boolean =
        If(is_interface, type_info(Of T, type_info_operators.interface_inherit, ICloneable).v,
                         type_info(Of T, type_info_operators.implement, ICloneable).v)
    Public Shared ReadOnly is_cloneable_T As Boolean =
        If(is_interface, type_info(Of T, type_info_operators.interface_inherit, ICloneable(Of T)).v,
                         type_info(Of T, type_info_operators.implement, ICloneable(Of T)).v)
    Public Shared ReadOnly is_delegate As Boolean = type_info(Of T, type_info_operators.inherit, [Delegate]).v
    ' => GetType(T).IsPrimitive
    Public Shared ReadOnly is_primitive As Boolean = GetType(T).IsPrimitive()
    Public Shared ReadOnly is_nullable As Boolean = GetType(T).is(GetType(Nullable(Of )))
    ' => GetType(T) Is GetType(String)
    Public Shared ReadOnly is_string As Boolean = GetType(T) Is GetType(String)
    ' Check is_primitive to reduce cost.
    Public Shared ReadOnly is_integral As Boolean =
        is_primitive AndAlso (
            GetType(T) Is GetType(Byte) OrElse
            GetType(T) Is GetType(SByte) OrElse
            GetType(T) Is GetType(UInt16) OrElse
            GetType(T) Is GetType(Int16) OrElse
            GetType(T) Is GetType(UInt32) OrElse
            GetType(T) Is GetType(Int32) OrElse
            GetType(T) Is GetType(UInt64) OrElse
            GetType(T) Is GetType(Int64))
    Public Shared ReadOnly is_number As Boolean =
        is_integral OrElse (
            is_primitive AndAlso (
                GetType(T) Is GetType(Single) OrElse
                GetType(T) Is GetType(Double) OrElse
                GetType(T) Is GetType(Decimal)))
    Public Shared ReadOnly is_enum As Boolean = GetType(T).IsEnum()
    Public Shared ReadOnly is_array As Boolean = GetType(T).IsArray()
    Public Shared ReadOnly is_array_type As Boolean = GetType(T) Is GetType(Array)
    Public Shared ReadOnly can_cast_to_array_type As Boolean = is_array OrElse is_array_type

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function has_finalizer() As Boolean
        Return finalizer_cache.has()
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function finalizer() As Action(Of T)
        Return finalizer_cache.v
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function annotated_constructor_info(Of ATTR)() As ConstructorInfo
        Return annotated_constructor_cache(Of ATTR).info
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function annotated_constructor(Of ATTR)() As Func(Of Object(), T)
        Return annotated_constructor_cache(Of ATTR).v.typed
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function dominated_constructor() As Func(Of T)
        Return constructor_cache.dominated.typed
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function size() As Int32
        Return size_cache.size
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function size_uint32() As UInt32
        assert(size() >= 0)
        Return CUInt(size())
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function new_object_clone() As _do_val_ref(Of T, T, Boolean)
        Return clone_cache.new_object_clone
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function dominated_clone() As _do_val_ref(Of T, T, Boolean)
        Return clone_cache.dominated_clone
    End Function

    <MethodImpl(method_impl_options.aggressive_inlining)>
    Public Shared Function static_constructor() As static_constructor
        Return static_constructor_cache.s
    End Function

    Private Sub New()
    End Sub
End Class
