
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector
Imports osi.root.utt

'moved from connector, they are slow
Friend Module _is_nothing_func
    <Extension()> Public Function is_nothing(Of T)(ByVal i As T) As Boolean
        Return Not TypeOf i Is ValueType AndAlso i Is Nothing
    End Function

    <Extension()> Public Function is_valuetype_or_nothing(Of T)(ByVal i As T) As Boolean
        Return TypeOf i Is ValueType OrElse i Is Nothing
    End Function

    <Extension()> Public Function is_not_nothing(Of T)(ByVal i As T) As Boolean
        Return Not is_nothing(i)
    End Function
End Module

Public MustInherit Class type_check_perf
    Inherits performance_case_wrapper

    Protected Sub New(ByVal c As type_check_case, Optional ByVal large_round As Boolean = False)
        Me.New(c, 1024 * 1024 * 8 * If(large_round, 8, 1))
    End Sub

    Protected Sub New(ByVal c As type_check_case, ByVal round As Int32)
        MyBase.New(repeat(c, round))
    End Sub

    Protected Interface int
    End Interface

    Protected Class base_c
    End Class

    Protected Class inh_c
        Inherits base_c
    End Class

    Protected Class base2_c
        Implements int
    End Class

    Protected Class inh2_c
        Inherits base2_c
    End Class

    Protected Class base3_c
    End Class

    Protected Class cloneable
        Implements ICloneable

        Public Function Clone() As Object Implements ICloneable.Clone
            Return Me
        End Function
    End Class

    Protected Structure struct
    End Structure

    Protected Shared Function object_is_nothing(ByVal i As Object) As Boolean
        Dim b As Boolean = False
        b = i Is Nothing
        b = Not i Is Nothing
        Return True
    End Function

    Protected Shared Function generic_is_nothing(Of T)(ByVal i As T) As Boolean
        Dim b As Boolean = False
        b = i Is Nothing
        b = Not i Is Nothing
        Return True
    End Function

    Protected Shared Function is_nothing_func(Of T)(ByVal i As T) As Boolean
        Dim b As Boolean = False
        b = i.is_nothing()
        b = i.is_not_nothing()
        Return True
    End Function

    Protected Shared Function object_is_nothing_logic(ByVal i As Object) As Boolean
        Dim b As Boolean = False
        b = Not TypeOf i Is ValueType AndAlso i Is Nothing
        b = TypeOf i Is ValueType OrElse Not i Is Nothing
        Return True
    End Function

    Protected Shared Function generic_is_nothing_logic(Of T)(ByVal i As T) As Boolean
        Dim b As Boolean = False
        b = Not TypeOf i Is ValueType AndAlso i Is Nothing
        b = TypeOf i Is ValueType OrElse Not i Is Nothing
        Return True
    End Function

    Protected Shared Function object_typeof_is(ByVal i As Object) As Boolean
        Dim b As Boolean = False
        b = TypeOf i Is SByte
        b = TypeOf i Is Byte
        b = TypeOf i Is Int16
        b = TypeOf i Is UInt16
        b = TypeOf i Is Int32
        b = TypeOf i Is UInt32
        b = TypeOf i Is Int64
        b = TypeOf i Is UInt64
        b = TypeOf i Is String
        b = TypeOf i Is int
        b = TypeOf i Is base_c
        b = TypeOf i Is inh_c
        b = TypeOf i Is base2_c
        b = TypeOf i Is inh2_c
        b = TypeOf i Is base3_c
        b = TypeOf i Is cloneable
        b = TypeOf i Is struct
        b = TypeOf i Is IDisposable
        b = TypeOf i Is ICloneable
        b = TypeOf i Is ValueType
        Return True
    End Function

    Protected Shared Function T_typeof_is(Of T)(ByVal i As T) As Boolean
        Dim b As Boolean = False
        b = TypeOf i Is SByte
        b = TypeOf i Is Byte
        b = TypeOf i Is Int16
        b = TypeOf i Is UInt16
        b = TypeOf i Is Int32
        b = TypeOf i Is UInt32
        b = TypeOf i Is Int64
        b = TypeOf i Is UInt64
        b = TypeOf i Is String
        b = TypeOf i Is int
        b = TypeOf i Is base_c
        b = TypeOf i Is inh_c
        b = TypeOf i Is base2_c
        b = TypeOf i Is inh2_c
        b = TypeOf i Is base3_c
        b = TypeOf i Is cloneable
        b = TypeOf i Is struct
        b = TypeOf i Is IDisposable
        b = TypeOf i Is ICloneable
        b = TypeOf i Is ValueType
        Return True
    End Function

    Protected Shared Function is_type_of(Of T)(ByVal i As T) As Boolean
        Dim b As Boolean = False
        b = i.istype(Of SByte)()
        b = i.istype(Of Byte)()
        b = i.istype(Of Int16)()
        b = i.istype(Of UInt16)()
        b = i.istype(Of Int32)()
        b = i.istype(Of UInt32)()
        b = i.istype(Of Int64)()
        b = i.istype(Of UInt64)()
        b = i.istype(Of String)()
        b = i.istype(Of int)()
        b = i.istype(Of base_c)()
        b = i.istype(Of inh_c)()
        b = i.istype(Of base2_c)()
        b = i.istype(Of inh2_c)()
        b = i.istype(Of base3_c)()
        b = i.istype(Of cloneable)()
        b = i.istype(Of struct)()
        b = i.istype(Of IDisposable)()
        b = i.istype(Of ICloneable)()
        b = i.is_valuetype()
        Return True
    End Function

    Protected Shared Function is_type_of(Of T)() As Boolean
        Dim b As Boolean = False
        b = type_info(Of T, type_info_operators.is, SByte).v
        b = type_info(Of T, type_info_operators.is, Byte).v
        b = type_info(Of T, type_info_operators.is, Int16).v
        b = type_info(Of T, type_info_operators.is, UInt16).v
        b = type_info(Of T, type_info_operators.is, Int32).v
        b = type_info(Of T, type_info_operators.is, UInt32).v
        b = type_info(Of T, type_info_operators.is, Int64).v
        b = type_info(Of T, type_info_operators.is, UInt64).v
        b = type_info(Of T, type_info_operators.is, String).v
        b = type_info(Of T, type_info_operators.is, int).v
        b = type_info(Of T, type_info_operators.is, base_c).v
        b = type_info(Of T, type_info_operators.is, inh_c).v
        b = type_info(Of T, type_info_operators.is, base2_c).v
        b = type_info(Of T, type_info_operators.is, inh2_c).v
        b = type_info(Of T, type_info_operators.is, base3_c).v
        b = type_info(Of T, type_info_operators.is, cloneable).v
        b = type_info(Of T, type_info_operators.is, struct).v
        b = type_info(Of T, type_info_operators.is, IDisposable).v
        b = type_info(Of T, type_info_operators.is, ICloneable).v
        b = type_info(Of T, type_info_operators.is, ValueType).v
        Return True
    End Function

    Protected MustInherit Class type_check_case
        Inherits [case]

        Protected MustOverride Function int_case(ByVal i As Int32) As Boolean
        Protected MustOverride Function string_case(ByVal i As String) As Boolean
        Protected MustOverride Function base_c_case(ByVal i As base_c) As Boolean
        Protected MustOverride Function inh_c_case(ByVal i As inh_c) As Boolean
        Protected MustOverride Function base2_c_case(ByVal i As base2_c) As Boolean
        Protected MustOverride Function inh2_c_case(ByVal i As inh2_c) As Boolean
        Protected MustOverride Function base3_c_case(ByVal i As base3_c) As Boolean
        Protected MustOverride Function cloneable_case(ByVal i As cloneable) As Boolean
        Protected MustOverride Function struct_case(ByVal i As struct) As Boolean

        Private Function int_case() As Boolean
            Return int_case(0) AndAlso
                   int_case(max_int32) AndAlso
                   int_case(min_int32)
        End Function

        Private Function string_case() As Boolean
            Return string_case(Nothing) AndAlso
                   string_case("abc")
        End Function

        Private Function base_c_case() As Boolean
            Return base_c_case(Nothing) AndAlso
                   base_c_case(New base_c())
        End Function

        Private Function inh_c_case() As Boolean
            Return inh_c_case(Nothing) AndAlso
                   inh_c_case(New inh_c())
        End Function

        Private Function base2_c_case() As Boolean
            Return base2_c_case(Nothing) AndAlso
                   base2_c_case(New base2_c())
        End Function

        Private Function inh2_c_case() As Boolean
            Return inh2_c_case(Nothing) AndAlso
                   inh2_c_case(New inh2_c())
        End Function

        Private Function base3_c_case() As Boolean
            Return base3_c_case(Nothing) AndAlso
                   base3_c_case(New base3_c())
        End Function

        Private Function cloneable_case() As Boolean
            Return cloneable_case(Nothing) AndAlso
                   cloneable_case(New cloneable())
        End Function

        Private Function struct_case() As Boolean
            Return struct_case(Nothing) AndAlso
                   struct_case(New struct())
        End Function

        Public NotOverridable Overrides Function run() As Boolean
            Return int_case() AndAlso
                   string_case() AndAlso
                   base_c_case() AndAlso
                   inh_c_case() AndAlso
                   base2_c_case() AndAlso
                   inh2_c_case() AndAlso
                   base3_c_case() AndAlso
                   cloneable_case() AndAlso
                   struct_case()
        End Function
    End Class
End Class

Public Class generic_typeof_perf
    Inherits type_check_perf

    Public Sub New()
        MyBase.New(New generic_typeof_case())
    End Sub

    Private Class generic_typeof_case
        Inherits type_check_case

        Protected Overrides Function int_case(ByVal i As Int32) As Boolean
            Return T_typeof_is(i)
        End Function

        Protected Overrides Function string_case(ByVal i As String) As Boolean
            Return T_typeof_is(i)
        End Function

        Protected Overrides Function base_c_case(ByVal i As base_c) As Boolean
            Return T_typeof_is(i)
        End Function

        Protected Overrides Function inh_c_case(ByVal i As inh_c) As Boolean
            Return T_typeof_is(i)
        End Function

        Protected Overrides Function base2_c_case(ByVal i As base2_c) As Boolean
            Return T_typeof_is(i)
        End Function

        Protected Overrides Function inh2_c_case(ByVal i As inh2_c) As Boolean
            Return T_typeof_is(i)
        End Function

        Protected Overrides Function base3_c_case(ByVal i As base3_c) As Boolean
            Return T_typeof_is(i)
        End Function

        Protected Overrides Function cloneable_case(ByVal i As cloneable) As Boolean
            Return T_typeof_is(i)
        End Function

        Protected Overrides Function struct_case(ByVal i As struct) As Boolean
            Return T_typeof_is(i)
        End Function
    End Class
End Class

Public Class object_typeof_perf
    Inherits type_check_perf

    Public Sub New()
        MyBase.New(New object_typeof_case())
    End Sub

    Private Class object_typeof_case
        Inherits type_check_case

        Protected Overrides Function int_case(ByVal i As Int32) As Boolean
            Return object_typeof_is(i)
        End Function

        Protected Overrides Function string_case(ByVal i As String) As Boolean
            Return object_typeof_is(i)
        End Function

        Protected Overrides Function base_c_case(ByVal i As base_c) As Boolean
            Return object_typeof_is(i)
        End Function

        Protected Overrides Function inh_c_case(ByVal i As inh_c) As Boolean
            Return object_typeof_is(i)
        End Function

        Protected Overrides Function base2_c_case(ByVal i As base2_c) As Boolean
            Return object_typeof_is(i)
        End Function

        Protected Overrides Function inh2_c_case(ByVal i As inh2_c) As Boolean
            Return object_typeof_is(i)
        End Function

        Protected Overrides Function base3_c_case(ByVal i As base3_c) As Boolean
            Return object_typeof_is(i)
        End Function

        Protected Overrides Function cloneable_case(ByVal i As cloneable) As Boolean
            Return object_typeof_is(i)
        End Function

        Protected Overrides Function struct_case(ByVal i As struct) As Boolean
            Return object_typeof_is(i)
        End Function
    End Class
End Class

Public Class istype_perf
    Inherits type_check_perf

    Public Sub New()
        MyBase.New(New istype_case())
    End Sub

    Private Class istype_case
        Inherits type_check_case

        Protected Overrides Function base_c_case(ByVal i As base_c) As Boolean
            Return is_type_of(i)
        End Function

        Protected Overrides Function base2_c_case(ByVal i As base2_c) As Boolean
            Return is_type_of(i)
        End Function

        Protected Overrides Function base3_c_case(ByVal i As base3_c) As Boolean
            Return is_type_of(i)
        End Function

        Protected Overrides Function cloneable_case(ByVal i As cloneable) As Boolean
            Return is_type_of(i)
        End Function

        Protected Overrides Function inh_c_case(ByVal i As inh_c) As Boolean
            Return is_type_of(i)
        End Function

        Protected Overrides Function inh2_c_case(ByVal i As inh2_c) As Boolean
            Return is_type_of(i)
        End Function

        Protected Overrides Function int_case(ByVal i As Int32) As Boolean
            Return is_type_of(i)
        End Function

        Protected Overrides Function string_case(ByVal i As String) As Boolean
            Return is_type_of(i)
        End Function

        Protected Overrides Function struct_case(ByVal i As struct) As Boolean
            Return is_type_of(i)
        End Function
    End Class
End Class

Public Class istype_no_object_perf
    Inherits type_check_perf

    Public Sub New()
        MyBase.New(New istype_case())
    End Sub

    Private Class istype_case
        Inherits type_check_case

        Protected Overrides Function base_c_case(ByVal i As base_c) As Boolean
            Return is_type_of(Of base_c)()
        End Function

        Protected Overrides Function base2_c_case(ByVal i As base2_c) As Boolean
            Return is_type_of(Of base2_c)()
        End Function

        Protected Overrides Function base3_c_case(ByVal i As base3_c) As Boolean
            Return is_type_of(Of base3_c)()
        End Function

        Protected Overrides Function cloneable_case(ByVal i As cloneable) As Boolean
            Return is_type_of(Of cloneable)()
        End Function

        Protected Overrides Function inh_c_case(ByVal i As inh_c) As Boolean
            Return is_type_of(Of inh_c)()
        End Function

        Protected Overrides Function inh2_c_case(ByVal i As inh2_c) As Boolean
            Return is_type_of(Of inh2_c)()
        End Function

        Protected Overrides Function int_case(ByVal i As Int32) As Boolean
            Return is_type_of(Of Int32)()
        End Function

        Protected Overrides Function string_case(ByVal i As String) As Boolean
            Return is_type_of(Of String)()
        End Function

        Protected Overrides Function struct_case(ByVal i As struct) As Boolean
            Return is_type_of(Of struct)()
        End Function
    End Class
End Class

Public Class is_nothing_func_perf
    Inherits type_check_perf

    Public Sub New()
        MyBase.New(New is_nothing_func_case(), True)
    End Sub

    Private Class is_nothing_func_case
        Inherits type_check_case

        Protected Overrides Function base_c_case(ByVal i As base_c) As Boolean
            Return is_nothing_func(i)
        End Function

        Protected Overrides Function base2_c_case(ByVal i As base2_c) As Boolean
            Return is_nothing_func(i)
        End Function

        Protected Overrides Function base3_c_case(ByVal i As base3_c) As Boolean
            Return is_nothing_func(i)
        End Function

        Protected Overrides Function cloneable_case(ByVal i As cloneable) As Boolean
            Return is_nothing_func(i)
        End Function

        Protected Overrides Function inh_c_case(ByVal i As inh_c) As Boolean
            Return is_nothing_func(i)
        End Function

        Protected Overrides Function inh2_c_case(ByVal i As inh2_c) As Boolean
            Return is_nothing_func(i)
        End Function

        Protected Overrides Function int_case(ByVal i As Integer) As Boolean
            Return is_nothing_func(i)
        End Function

        Protected Overrides Function string_case(ByVal i As String) As Boolean
            Return is_nothing_func(i)
        End Function

        Protected Overrides Function struct_case(ByVal i As struct) As Boolean
            Return is_nothing_func(i)
        End Function
    End Class
End Class

Public Class generic_is_nothing_perf
    Inherits type_check_perf

    Public Sub New()
        MyBase.New(New generic_is_nothing_case(), True)
    End Sub

    Private Class generic_is_nothing_case
        Inherits type_check_case

        Protected Overrides Function base_c_case(ByVal i As base_c) As Boolean
            Return generic_is_nothing(i)
        End Function

        Protected Overrides Function base2_c_case(ByVal i As base2_c) As Boolean
            Return generic_is_nothing(i)
        End Function

        Protected Overrides Function base3_c_case(ByVal i As base3_c) As Boolean
            Return generic_is_nothing(i)
        End Function

        Protected Overrides Function cloneable_case(ByVal i As cloneable) As Boolean
            Return generic_is_nothing(i)
        End Function

        Protected Overrides Function inh_c_case(ByVal i As inh_c) As Boolean
            Return generic_is_nothing(i)
        End Function

        Protected Overrides Function inh2_c_case(ByVal i As inh2_c) As Boolean
            Return generic_is_nothing(i)
        End Function

        Protected Overrides Function int_case(ByVal i As Integer) As Boolean
            Return generic_is_nothing(i)
        End Function

        Protected Overrides Function string_case(ByVal i As String) As Boolean
            Return generic_is_nothing(i)
        End Function

        Protected Overrides Function struct_case(ByVal i As struct) As Boolean
            Return generic_is_nothing(i)
        End Function
    End Class
End Class

Public Class object_is_nothing_perf
    Inherits type_check_perf

    Public Sub New()
        MyBase.New(New object_is_nothing_case(), True)
    End Sub

    Private Class object_is_nothing_case
        Inherits type_check_case

        Protected Overrides Function base_c_case(ByVal i As base_c) As Boolean
            Return object_is_nothing(i)
        End Function

        Protected Overrides Function base2_c_case(ByVal i As base2_c) As Boolean
            Return object_is_nothing(i)
        End Function

        Protected Overrides Function base3_c_case(ByVal i As base3_c) As Boolean
            Return object_is_nothing(i)
        End Function

        Protected Overrides Function cloneable_case(ByVal i As cloneable) As Boolean
            Return object_is_nothing(i)
        End Function

        Protected Overrides Function inh_c_case(ByVal i As inh_c) As Boolean
            Return object_is_nothing(i)
        End Function

        Protected Overrides Function inh2_c_case(ByVal i As inh2_c) As Boolean
            Return object_is_nothing(i)
        End Function

        Protected Overrides Function int_case(ByVal i As Integer) As Boolean
            Return object_is_nothing(i)
        End Function

        Protected Overrides Function string_case(ByVal i As String) As Boolean
            Return object_is_nothing(i)
        End Function

        Protected Overrides Function struct_case(ByVal i As struct) As Boolean
            Return object_is_nothing(i)
        End Function
    End Class
End Class

Public Class object_is_nothing_logic_perf
    Inherits type_check_perf

    Public Sub New()
        MyBase.New(New object_is_nothing_logic_case(), True)
    End Sub

    Private Class object_is_nothing_logic_case
        Inherits type_check_case

        Protected Overrides Function base_c_case(ByVal i As base_c) As Boolean
            Return object_is_nothing_logic(i)
        End Function

        Protected Overrides Function base2_c_case(ByVal i As base2_c) As Boolean
            Return object_is_nothing_logic(i)
        End Function

        Protected Overrides Function base3_c_case(ByVal i As base3_c) As Boolean
            Return object_is_nothing_logic(i)
        End Function

        Protected Overrides Function cloneable_case(ByVal i As cloneable) As Boolean
            Return object_is_nothing_logic(i)
        End Function

        Protected Overrides Function inh_c_case(ByVal i As inh_c) As Boolean
            Return object_is_nothing_logic(i)
        End Function

        Protected Overrides Function inh2_c_case(ByVal i As inh2_c) As Boolean
            Return object_is_nothing_logic(i)
        End Function

        Protected Overrides Function int_case(ByVal i As Integer) As Boolean
            Return object_is_nothing_logic(i)
        End Function

        Protected Overrides Function string_case(ByVal i As String) As Boolean
            Return object_is_nothing_logic(i)
        End Function

        Protected Overrides Function struct_case(ByVal i As struct) As Boolean
            Return object_is_nothing_logic(i)
        End Function
    End Class
End Class

Public Class generic_is_nothing_logic_perf
    Inherits type_check_perf

    Public Sub New()
        MyBase.New(New generic_is_nothing_logic_case(), True)
    End Sub

    Private Class generic_is_nothing_logic_case
        Inherits type_check_case

        Protected Overrides Function base_c_case(ByVal i As base_c) As Boolean
            Return generic_is_nothing_logic(i)
        End Function

        Protected Overrides Function base2_c_case(ByVal i As base2_c) As Boolean
            Return generic_is_nothing_logic(i)
        End Function

        Protected Overrides Function base3_c_case(ByVal i As base3_c) As Boolean
            Return generic_is_nothing_logic(i)
        End Function

        Protected Overrides Function cloneable_case(ByVal i As cloneable) As Boolean
            Return generic_is_nothing_logic(i)
        End Function

        Protected Overrides Function inh_c_case(ByVal i As inh_c) As Boolean
            Return generic_is_nothing_logic(i)
        End Function

        Protected Overrides Function inh2_c_case(ByVal i As inh2_c) As Boolean
            Return generic_is_nothing_logic(i)
        End Function

        Protected Overrides Function int_case(ByVal i As Integer) As Boolean
            Return generic_is_nothing_logic(i)
        End Function

        Protected Overrides Function string_case(ByVal i As String) As Boolean
            Return generic_is_nothing_logic(i)
        End Function

        Protected Overrides Function struct_case(ByVal i As struct) As Boolean
            Return generic_is_nothing_logic(i)
        End Function
    End Class
End Class
