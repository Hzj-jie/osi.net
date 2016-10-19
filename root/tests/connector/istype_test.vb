
Imports System.Runtime.CompilerServices
Imports osi.root.connector
Imports osi.root.delegates
Imports osi.root.utt

'moved from istype.vb, these functions are slow <10 times, even with istype<T, T2>()>
Friend Module _type_case_functions
    'ATTENTION, if this is a reference type, and the value is nothing, this function will always return false
    'the TypeOf is a little bit tricky in .net
    'consistent with the behavior of TypeOf Is operator
    <Extension()> Public Function istype(Of T, T2)(ByVal i As T) As Boolean
        Return (type_info(Of T, type_info_operators.is, T2).v AndAlso
                (i.is_valuetype() OrElse Not i Is Nothing)) OrElse
               TypeOf i Is T2
    End Function

    <Extension()> Public Function is_cloneable(Of T)(ByVal i As T) As Boolean
        Return i.istype(Of ICloneable)()
    End Function

    <Extension()> Public Function is_valuetype(Of T)(ByVal i As T) As Boolean
        Return type_info(Of T).is_valuetype OrElse
               TypeOf i Is ValueType
    End Function
End Module

Public Class istype_test
    Inherits [case]

    Private Class base
    End Class

    Private Class inh
        Inherits base
    End Class

    Private Interface int
    End Interface

    Private Class imp
        Implements int
    End Class

    Private Class inh_imp
        Inherits base
        Implements int
    End Class

    Private Class cloneable
        Implements ICloneable

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me
        End Function
    End Class

    Private Class not_cloneable
    End Class

    Private Structure struct
        Implements int
    End Structure

    Private Delegate Function f() As Boolean
    Private Delegate Sub s(ByVal x As int)

    Public Overrides Function run() As Boolean
        assert_true((New base()).istype(Of base)())
        assert_true(type_info(Of base, type_info_operators.is, base).v)
        assert_false((New base()).istype(Of inh)())
        assert_true((New base()).GetType().is(GetType(base)))
        assert_false(type_info(Of base, type_info_operators.is, inh).v)
        assert_true((New inh()).GetType().is(GetType(base)))
        assert_true(type_info(Of inh, type_info_operators.is, base).v)
        assert_false(DirectCast(Nothing, inh).get_type_or_null().is(GetType(base)))
        assert_true((New inh()).istype(Of base)())
        assert_false(DirectCast(Nothing, inh).istype(Of base)())
        assert_true((New inh()).istype(Of base)())
        assert_false((New inh()).istype(Of int)())
        assert_false(type_info(Of inh, type_info_operators.is, int).v)
        assert_true(type_info(Of cloneable).is_cloneable())
        assert_true(type_info(Of cloneable, type_info_operators.is, ICloneable).v)
        assert_false(DirectCast(Nothing, cloneable).is_cloneable())
        assert_true((New cloneable()).is_cloneable())
        assert_false((New not_cloneable()).is_cloneable())
        assert_false(type_info(Of not_cloneable, type_info_operators.is, ICloneable).v)
        assert_true((New imp()).istype(Of int)())
        assert_true(type_info(Of imp, type_info_operators.is, int).v)
        assert_false(DirectCast(Nothing, imp).istype(Of int)())
        assert_true((New inh_imp()).istype(Of int)())
        assert_true((New inh_imp()).istype(Of base)())
        assert_true(type_info(Of inh_imp, type_info_operators.is, int).v)
        assert_true(type_info(Of inh_imp, type_info_operators.is, base).v)
        assert_false(DirectCast(Nothing, inh_imp).istype(Of int)())
        assert_false(DirectCast(Nothing, inh_imp).istype(Of base)())
        assert_true(type_info(Of Int32).is_valuetype)
        assert_true(type_info(Of Int32, type_info_operators.is, ValueType).v)
        assert_true(type_info(Of Boolean).is_valuetype)
        assert_true(type_info(Of Boolean, type_info_operators.is, ValueType).v)
        assert_true(type_info(Of struct).is_valuetype)
        assert_true(type_info(Of struct, type_info_operators.is, ValueType).v)
        assert_true(DirectCast(Nothing, Int32).is_valuetype())
        assert_true(DirectCast(Nothing, Boolean).is_valuetype())
        'bug bug in .net 3.5, type_info_operators.is, DirectCast(Nothing, type_info_operators.is, A_STRUCTURE) throw NullReferenceException
        'assert_true(DirectCast(Nothing, type_info_operators.is, struct).is_valuetype())
        assert_true(CType(Nothing, struct).is_valuetype())
        assert_true(3.is_valuetype())
        assert_true(True.is_valuetype())
        assert_true((New struct()).is_valuetype())
        assert_false(type_info(Of String).is_valuetype)
        assert_false(type_info(Of String, type_info_operators.is, ValueType).v)
        assert_false(type_info(Of inh).is_valuetype)
        assert_false(type_info(Of inh, type_info_operators.is, ValueType).v)
        assert_false(type_info(Of inh_imp).is_valuetype)
        assert_false(type_info(Of inh_imp, type_info_operators.is, ValueType).v)
        assert_false(DirectCast(Nothing, String).is_valuetype())
        assert_false(DirectCast(Nothing, inh).is_valuetype())
        assert_false(DirectCast(Nothing, inh_imp).is_valuetype())
        assert_false("string".is_valuetype())
        assert_false((New inh()).is_valuetype())
        assert_false((New inh_imp()).is_valuetype())
        assert_true(type_info(Of f).is_delegate())
        assert_true(type_info(Of s).is_delegate())
        assert_true(type_info(Of Func(Of int, Boolean)).is_delegate())
        assert_true(type_info(Of Action(Of int)).is_delegate())
        assert_true(type_info(Of _do(Of int, Boolean)).is_delegate())
        assert_true(type_info(Of _do_val_val_ref(Of int, Int32, Int64, Boolean)).is_delegate())
        assert_false(type_info(Of int).is_delegate())
        assert_false(type_info(Of Int32).is_delegate())
        assert_true(type_info(Of Int32, type_info_operators.equal, Integer).v)
        assert_true(type_info(Of inh, type_info_operators.equal, inh).v)
        assert_false(type_info(Of inh, type_info_operators.equal, inh_imp).v)
        Return True
    End Function
End Class
