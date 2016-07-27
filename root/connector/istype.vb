
Imports System.Reflection
Imports System.Runtime.CompilerServices

Public Module _istype
    <Extension()> Public Function get_type_or_null(ByVal this As Object) As Type
        Return If(this Is Nothing, Nothing, this.GetType())
    End Function

    <Extension()> Public Function has_finalizer(ByVal this As Type) As Boolean
        Return Not this.GetMethod("Finalize",
                                  BindingFlags.NonPublic Or
                                  BindingFlags.Instance Or
                                  BindingFlags.DeclaredOnly) Is Nothing
    End Function

    <Extension()> Public Function [is](ByVal this As Type, ByVal base As Type) As Boolean
        If this Is Nothing AndAlso base Is Nothing Then
            Return True
        ElseIf this Is Nothing OrElse base Is Nothing Then
            Return False
        ElseIf this Is base Then
            Return True
        ElseIf base.IsGenericTypeDefinition() AndAlso
               this.IsGenericType() AndAlso
               Not this.IsGenericTypeDefinition() Then
            Return (this.GetGenericTypeDefinition().is(base))
        ElseIf base.IsInterface() Then
            Return implement(this, base)
        Else
            Return inherit(this, base)
        End If
    End Function

    <Extension()> Public Function [is](Of T)(ByVal this As Type) As Boolean
        Return [is](this, GetType(T))
    End Function

    Private Structure istype_cache(Of T, T2)
        Public Shared ReadOnly v As Boolean

        Shared Sub New()
            v = GetType(T).is(GetType(T2))
        End Sub
    End Structure

    Public Function istype(Of T, T2)() As Boolean
        Return istype_cache(Of T, T2).v
    End Function

    Public Function is_cloneable(Of T)() As Boolean
        Return istype(Of T, ICloneable)()
    End Function

    Public Function is_valuetype(Of T)() As Boolean
        Return istype(Of T, ValueType)()
    End Function

    Public Function is_delegate(Of T)() As Boolean
        Return istype(Of T, [Delegate])()
    End Function

    Private Structure type_equals_cache(Of T, T2)
        Public Shared ReadOnly v As Boolean

        Shared Sub New()
            v = GetType(T).Equals(GetType(T2))
        End Sub
    End Structure

    Public Function type_equals(Of T, T2)() As Boolean
        Return type_equals_cache(Of T, T2).v
    End Function
End Module
