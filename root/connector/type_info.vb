﻿
Public Class type_info_operators
    ' T1 == T2 or T1 is a subclass or an implementation of T2.
    Public NotInheritable Class [is]
        Inherits type_info_operators

        Private Sub New()
        End Sub
    End Class

    ' T1 == T2
    Public NotInheritable Class equal
        Inherits type_info_operators

        Private Sub New()
        End Sub
    End Class

    ' T1 inherits T2
    Public NotInheritable Class inherit
        Inherits type_info_operators

        Private Sub New()
        End Sub
    End Class

    ' T1 implements T2
    Public NotInheritable Class implement
        Inherits type_info_operators

        Private Sub New()
        End Sub
    End Class

    ' T1 is an interface and it inherits T2
    Public NotInheritable Class interface_inherit
        Inherits type_info_operators

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class type_info(Of T1, _OP As type_info_operators, T2)
    Public Shared ReadOnly v As Boolean

    Shared Sub New()
        If GetType(_OP).Equals(GetType(type_info_operators.[is])) Then
            v = GetType(T1).is(GetType(T2))
        ElseIf GetType(_OP).Equals(GetType(type_info_operators.equal)) Then
            v = GetType(T1).Equals(GetType(T2))
        ElseIf GetType(_OP).Equals(GetType(type_info_operators.inherit)) Then
            v = GetType(T1).inherit(GetType(T2))
        ElseIf GetType(_OP).Equals(GetType(type_info_operators.implement)) Then
            v = GetType(T1).implement(GetType(T2))
        ElseIf GetType(_OP).Equals(GetType(type_info_operators.interface_inherit)) Then
            v = GetType(T1).interface_inherit(GetType(T2))
        Else
            assert(False)
        End If
    End Sub

    Private Sub New()
    End Sub
End Class

Public NotInheritable Class type_info(Of T)
    ' Do not add type, refer to gettype_perf test, since this is a template class, cache GetType(T) result cannot
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
    End Sub

    Private NotInheritable Class has_finalizer_cache
        Public Shared ReadOnly v As Boolean

        Shared Sub New()
            v = GetType(T).has_finalizer()
        End Sub

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class
