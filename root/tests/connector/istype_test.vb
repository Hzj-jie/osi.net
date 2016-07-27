
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
        Return (_istype.istype(Of T, T2)() AndAlso
                (i.is_valuetype() OrElse Not i Is Nothing)) OrElse
               TypeOf i Is T2
    End Function

    <Extension()> Public Function is_cloneable(Of T)(ByVal i As T) As Boolean
        Return i.istype(Of ICloneable)()
    End Function

    <Extension()> Public Function is_valuetype(Of T)(ByVal i As T) As Boolean
        Return _istype.is_valuetype(Of T)() OrElse
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
        assert_true(_istype.istype(Of base, base)())
        assert_false((New base()).istype(Of inh)())
        assert_true((New base()).GetType().is(GetType(base)))
        assert_false(_istype.istype(Of base, inh)())
        assert_true((New inh()).GetType().is(GetType(base)))
        assert_true(_istype.istype(Of inh, base)())
        assert_false(DirectCast(Nothing, inh).get_type_or_null().is(GetType(base)))
        assert_true((New inh()).istype(Of base)())
        assert_false(DirectCast(Nothing, inh).istype(Of base)())
        assert_true((New inh()).istype(Of base)())
        assert_false((New inh()).istype(Of int)())
        assert_false(_istype.istype(Of inh, int)())
        assert_true(_istype.is_cloneable(Of cloneable)())
        assert_true(_istype.istype(Of cloneable, ICloneable)())
        assert_false(DirectCast(Nothing, cloneable).is_cloneable())
        assert_true((New cloneable()).is_cloneable())
        assert_false((New not_cloneable()).is_cloneable())
        assert_false(_istype.istype(Of not_cloneable, ICloneable)())
        assert_true((New imp()).istype(Of int)())
        assert_true(_istype.istype(Of imp, int)())
        assert_false(DirectCast(Nothing, imp).istype(Of int)())
        assert_true((New inh_imp()).istype(Of int)())
        assert_true((New inh_imp()).istype(Of base)())
        assert_true(_istype.istype(Of inh_imp, int)())
        assert_true(_istype.istype(Of inh_imp, base)())
        assert_false(DirectCast(Nothing, inh_imp).istype(Of int)())
        assert_false(DirectCast(Nothing, inh_imp).istype(Of base)())
        assert_true(_istype.is_valuetype(Of Int32)())
        assert_true(_istype.istype(Of Int32, ValueType)())
        assert_true(_istype.is_valuetype(Of Boolean)())
        assert_true(_istype.istype(Of Boolean, ValueType)())
        assert_true(_istype.is_valuetype(Of struct)())
        assert_true(_istype.istype(Of struct, ValueType)())
        assert_true(DirectCast(Nothing, Int32).is_valuetype())
        assert_true(DirectCast(Nothing, Boolean).is_valuetype())
        'bug bug in .net 3.5, DirectCast(Nothing, A_STRUCTURE) throw NullReferenceException
        'assert_true(DirectCast(Nothing, struct).is_valuetype())
        assert_true(CType(Nothing, struct).is_valuetype())
        assert_true(3.is_valuetype())
        assert_true(True.is_valuetype())
        assert_true((New struct()).is_valuetype())
        assert_false(_istype.is_valuetype(Of String)())
        assert_false(_istype.istype(Of String, ValueType)())
        assert_false(_istype.is_valuetype(Of inh)())
        assert_false(_istype.istype(Of inh, ValueType)())
        assert_false(_istype.is_valuetype(Of inh_imp)())
        assert_false(_istype.istype(Of inh_imp, ValueType)())
        assert_false(DirectCast(Nothing, String).is_valuetype())
        assert_false(DirectCast(Nothing, inh).is_valuetype())
        assert_false(DirectCast(Nothing, inh_imp).is_valuetype())
        assert_false("string".is_valuetype())
        assert_false((New inh()).is_valuetype())
        assert_false((New inh_imp()).is_valuetype())
        assert_true(is_delegate(Of f)())
        assert_true(is_delegate(Of s)())
        assert_true(is_delegate(Of Func(Of int, Boolean))())
        assert_true(is_delegate(Of Action(Of int))())
        assert_true(is_delegate(Of _do(Of int, Boolean))())
        assert_true(is_delegate(Of _do_val_val_ref(Of int, Int32, Int64, Boolean))())
        assert_false(is_delegate(Of int)())
        assert_false(is_delegate(Of Int32)())
        assert_true(type_equals(Of Int32, Integer)())
        assert_true(type_equals(Of inh, inh)())
        assert_false(type_equals(Of inh, inh_imp)())
        Return True
    End Function
End Class
