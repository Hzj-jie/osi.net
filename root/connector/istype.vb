
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants

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

    <Extension()> Public Function allocatable(ByVal t As Type) As Boolean
        Return Not t Is Nothing AndAlso
               Not t.IsArray() AndAlso
               Not t.IsValueType() AndAlso
               Not t.IsAbstract() AndAlso
               Not t.IsEnum() AndAlso
               Not t.IsGenericTypeDefinition() AndAlso
               Not t.IsInterface() AndAlso
               Not t.[is](GetType([Delegate]))
    End Function

    <Extension()> Public Function constructors(ByVal t As Type) As ConstructorInfo()
        Try
            Return t.GetConstructors(Reflection.BindingFlags.Instance Or
                                     Reflection.BindingFlags.Public Or
                                     Reflection.BindingFlags.NonPublic)
        Catch ex As Exception
            If isdebugmode() AndAlso Not suppress_alloc_error() Then
                raise_error(error_type.warning,
                            "failed to get constructors of type ", t.AssemblyQualifiedName(),
                            ", ex ", ex.Message)
            End If
            Return Nothing
        End Try
    End Function

    <Extension()> Public Function has_parameterless_constructor(ByVal t As Type,
                                                                ByVal accept_private_constructor As Boolean) As Boolean
        If t Is Nothing Then
            Return False
        Else
            Dim cs() As ConstructorInfo = Nothing
            cs = t.constructors()
            For i As Int32 = 0 To array_size(cs) - 1
                If isemptyarray(cs(i).GetParameters()) AndAlso
                   (accept_private_constructor OrElse cs(i).IsPublic()) Then
                    Return True
                End If
            Next
            Return False
        End If
    End Function

    <Extension()> Public Function has_parameterless_constructor(ByVal t As Type) As Boolean
        Return t.has_parameterless_constructor(True)
    End Function

    <Extension()> Public Function has_parameterless_public_constructor(ByVal t As Type) As Boolean
        Return t.has_parameterless_constructor(False)
    End Function
End Module
