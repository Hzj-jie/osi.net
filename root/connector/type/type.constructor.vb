
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants

Public Module _constructor
    <Extension()> Public Function allocatable(ByVal t As Type) As Boolean
        Return Not t Is Nothing AndAlso
               Not t.IsArray() AndAlso
               Not t.IsAbstract() AndAlso
               Not t.IsEnum() AndAlso
               Not t.is_unspecified_generic_type() AndAlso
               Not t.IsInterface() AndAlso
               Not t.[is](GetType([Delegate]))
    End Function

    <Extension()> Public Function constructors(ByVal t As Type) As ConstructorInfo()
        Try
            Return t.GetConstructors(BindingFlags.Instance Or
                                     BindingFlags.Public Or
                                     BindingFlags.NonPublic)
        Catch ex As Exception
            If isdebugmode() AndAlso Not is_suppressed.alloc_error() Then
                raise_error(error_type.warning,
                            "failed to get constructors of type ", t.AssemblyQualifiedName(),
                            ", ex ", ex.Message)
            End If
            Return Nothing
        End Try
    End Function

    <Extension()> Public Function annotated_constructor_info(Of ATTR)(ByVal t As Type) As ConstructorInfo
        If t Is Nothing Then
            Return Nothing
        End If
        Dim constructors() As ConstructorInfo = t.constructors()
        For i As Int32 = 0 To array_size_i(constructors) - 1
            If constructors(i).has_custom_attribute(Of ATTR)() Then
                Return constructors(i)
            End If
        Next
        Return Nothing
    End Function

    <Extension()> Public Function annotated_constructor(Of ATTR)(ByVal t As Type) As Func(Of Object(), Object)
        Dim info As ConstructorInfo = annotated_constructor_info(Of ATTR)(t)
        If info Is Nothing Then
            Return Nothing
        End If
        Return AddressOf info.Invoke
    End Function

    <Extension()> Public Function has_parameterless_constructor(ByVal t As Type,
                                                                ByVal accept_private_constructor As Boolean) As Boolean
        If t Is Nothing Then
            Return False
        End If

        Dim cs() As ConstructorInfo = t.constructors()
        For i As Int32 = 0 To array_size_i(cs) - 1
            If isemptyarray(cs(i).GetParameters()) AndAlso
                   (accept_private_constructor OrElse cs(i).IsPublic()) Then
                Return True
            End If
        Next
        Return False
    End Function

    <Extension()> Public Function has_parameterless_constructor(ByVal t As Type) As Boolean
        Return t.has_parameterless_constructor(True)
    End Function

    <Extension()> Public Function has_parameterless_public_constructor(ByVal t As Type) As Boolean
        Return t.has_parameterless_constructor(False)
    End Function

    <Extension()> Public Function first_constructor(ByVal t As Type) As ConstructorInfo
        Dim cs() As ConstructorInfo = Nothing
        cs = t.constructors()
        If isemptyarray(cs) Then
            Return Nothing
        End If
        Return cs(0)
    End Function

    <Extension()> Public Function execute(ByVal this As ConstructorInfo) As Object
        If this Is Nothing Then
            Return Nothing
        End If

        Dim parameters(this.GetParameters().Length - 1) As Object
        'assert vb.net will clear the array here
        'arrays.clear(parameters, 0, parameters.Length())
        Try
            Return this.Invoke(parameters)
        Catch ex As Exception
            If isdebugmode() AndAlso Not is_suppressed.alloc_error() Then
                raise_error(error_type.warning,
                            "Failed to invoke constructors of type ",
                            this.DeclaringType().AssemblyQualifiedName(),
                            ", ex ",
                            ex.details())
            End If
        End Try
        Return Nothing
    End Function

    <Extension()> Public Function alloc_with_parameters_constructor(ByVal t As Type) As Object
        Dim c As ConstructorInfo = Nothing
        c = t.first_constructor()
        Return c.execute()
    End Function

    Private Function alloc_with_parameterless_constructor(ByVal t As Type,
                                                          ByRef o As Object,
                                                          ByVal non_public As Boolean) As Boolean
        Try
            o = Activator.CreateInstance(t, non_public)
            Return True
        Catch ex As Exception
            If isdebugmode() AndAlso Not is_suppressed.alloc_error() Then
                raise_error(error_type.warning,
                            "Failed to create instance of type ",
                            t.AssemblyQualifiedName(),
                            " by Activator with non_public = ",
                            non_public,
                            ", ex ",
                            ex.details())
            End If
            Return False
        End Try
    End Function

    Private Function alloc_with_parameterless_constructor(ByVal t As Type,
                                                          ByVal non_public As Boolean) As Object
        Dim o As Object = Nothing
        If alloc_with_parameterless_constructor(t, o, non_public) Then
            Return o
        End If
        Return Nothing
    End Function

    <Extension()> Public Function alloc_with_public_parameterless_constructor(ByVal t As Type,
                                                                              ByRef o As Object) As Boolean
        Return alloc_with_parameterless_constructor(t, o, False)
    End Function

    <Extension()> Public Function alloc_with_public_parameterless_constructor(ByVal t As Type) As Object
        Return alloc_with_parameterless_constructor(t, False)
    End Function

    <Extension()> Public Function alloc_with_parameterless_constructor(ByVal t As Type,
                                                                       ByRef o As Object) As Boolean
        Return alloc_with_parameterless_constructor(t, o, True)
    End Function

    <Extension()> Public Function alloc_with_parameterless_constructor(ByVal t As Type) As Object
        Return alloc_with_parameterless_constructor(t, True)
    End Function

    <Extension()> Public Function public_parameterless_constructor(ByVal t As Type) As Func(Of Object)
        If t.has_parameterless_public_constructor() Then
            Return AddressOf t.alloc_with_public_parameterless_constructor
        End If
        Return Nothing
    End Function

    <Extension()> Public Function parameterless_constructor(ByVal t As Type) As Func(Of Object)
        If t.has_parameterless_constructor() Then
            Return AddressOf t.alloc_with_parameterless_constructor
        End If
        Return Nothing
    End Function

    <Extension()> Public Function parameters_constructor(ByVal t As Type) As Func(Of Object(), Object)
        Dim c As ConstructorInfo = Nothing
        c = t.first_constructor()
        If c Is Nothing Then
            Return Nothing
        End If
        Return AddressOf c.Invoke
    End Function

    <Extension()> Public Function default_parameters_constructor(ByVal t As Type) As Func(Of Object)
        Dim c As ConstructorInfo = Nothing
        c = t.first_constructor()
        If c Is Nothing Then
            Return Nothing
        End If
        Return AddressOf c.execute
    End Function

    Public Function allocate(Of T)() As T
        Return alloc(Of T)()
    End Function

    <Extension()> Public Function allocate(ByVal t As Type) As Object
        If Not allocatable(t) Then
            Return Nothing
        End If

        Dim o As Object = Nothing
        If alloc_with_public_parameterless_constructor(t, o) Then
            Return o
        End If

        If alloc_with_parameterless_constructor(t, o) Then
            Return o
        End If

        Return t.alloc_with_parameters_constructor()
    End Function

    <Extension()> Public Function allocate(Of RT)(ByVal t As Type) As RT
        Return direct_cast(Of RT)(allocate(t))
    End Function

    Public Function allocate_instance_of(Of T)(ByVal obj As T) As T
        If obj Is Nothing Then
            Return alloc(Of T)()
        End If
        Return direct_cast(Of T)(allocate(obj.GetType()))
    End Function
End Module
