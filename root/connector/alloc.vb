
#Const cached_alloc = True
Imports System.Reflection
Imports System.Runtime.CompilerServices
Imports osi.root.constants
Imports osi.root.connector

Public Module _alloc
    Private ReadOnly suppress_alloc_error_binder As binder(Of Func(Of Boolean), suppress_alloc_error_binder_protector)

    Sub New()
        suppress_alloc_error_binder = New binder(Of Func(Of Boolean), suppress_alloc_error_binder_protector)()
    End Sub

    Private Function suppress_alloc_error() As Boolean
        Return suppress_alloc_error_binder.has_value() AndAlso
               (+suppress_alloc_error_binder)()
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

    <Extension()> Public Function alloc_with_parameters_constructor(ByVal t As Type) As Object
        Dim cs() As ConstructorInfo = Nothing
        cs = t.constructors()
        If isemptyarray(cs) Then
            Return Nothing
        Else
            Dim parameters(cs(0).GetParameters().Length - 1) As Object
            'assert vb.net will clear the array here
            'memclr(parameters, 0, parameters.Length())
            Try
                Return cs(0).Invoke(parameters)
            Catch ex As Exception
                If isdebugmode() AndAlso Not suppress_alloc_error() Then
                    raise_error(error_type.warning,
                                 "failed to invoke constructors of type ", t.AssemblyQualifiedName(),
                                 ", ex ", ex.Message)
                End If
            End Try
            'assert(Not obj Is Nothing, "allocate of type " + t.FullName + " failed")
            Return Nothing
        End If
    End Function

    Private Function alloc_with_parameterless_constructor(ByVal t As Type,
                                                          ByRef o As Object,
                                                          ByVal non_public As Boolean) As Boolean
        Try
            o = Activator.CreateInstance(t, non_public)
            Return True
        Catch ex As Exception
            If isdebugmode() AndAlso Not suppress_alloc_error() Then
                raise_error(error_type.warning,
                             "failed to create instance of type ",
                             t.AssemblyQualifiedName(),
                             " by Activator with non_public = ",
                             non_public,
                             ", ex ",
                             ex.Message)
            End If
            Return False
        End Try
    End Function

    Private Function alloc_with_parameterless_constructor(ByVal t As Type,
                                                          ByVal non_public As Boolean) As Object
        Dim o As Object = Nothing
        If alloc_with_parameterless_constructor(t, o, non_public) Then
            Return o
        Else
            Return Nothing
        End If
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

    <Extension()> Public Function alloc(ByVal t As Type) As Object
        If allocatable(t) Then
            Dim o As Object = Nothing
            If alloc_with_public_parameterless_constructor(t, o) Then
                Return o
            ElseIf alloc_with_parameterless_constructor(t, o) Then
                Return o
            Else
                Return t.alloc_with_parameters_constructor()
            End If
        Else
            Return Nothing
            End If
    End Function

    Public Function alloc(Of T)(ByVal i As Type) As T
        Return cast(Of T)(alloc(i))
    End Function

    Public Function alloc(Of T)(ByVal obj As T) As T
        If obj Is Nothing Then
            Return alloc(Of T)()
        Else
            Return alloc(obj.GetType())
        End If
    End Function

    Private Class alloc_cache(Of T)
        Private Shared ReadOnly a As Func(Of T)

        Shared Sub New()
            Dim null_allocator As Func(Of T) =
                    Function() As T
                        Return Nothing
                    End Function
            Dim type As Type = Nothing
            type = GetType(T)
            If type.allocatable() Then
                If type.has_parameterless_public_constructor() Then
                    a = Function() As T
                            Return type.alloc_with_public_parameterless_constructor()
                        End Function
                ElseIf type.has_parameterless_constructor() Then
                    a = Function() As T
                            Return type.alloc_with_parameterless_constructor()
                        End Function
                Else
                    Dim cs() As ConstructorInfo = Nothing
                    cs = type.constructors()
                    If isemptyarray(cs) Then
                        a = null_allocator
                    Else
                        a = Function() As T
                                Return type.alloc_with_parameters_constructor()
                            End Function
                    End If
                End If
            Else
                a = null_allocator
            End If
        End Sub

        Public Shared Function allocate() As T
            Return a()
        End Function
    End Class

    Public Function use_cached_alloc() As Boolean
#If cached_alloc Then
        Return True
#Else
        Return False
#End If
    End Function

    Public Function alloc(Of T)() As T
#If cached_alloc Then
        Return alloc_cache(Of T).allocate()
#Else
        Try
            'T As New
            Return Activator.CreateInstance(Of T)()
        Catch
            'there is not a constructor without parameters
            Return alloc(GetType(T))
        End Try
#End If
    End Function

    ' Provides a shortcut to write
    ' Dim x As a_very_long_type(Of some_parameter_1, some_parameter_2) = Nothing
    ' x = _new(x)
    ' Or even,
    ' _new(x)
    Public Function _new(Of T As New)(ByRef i As T) As T
        i = New T()
        Return i
    End Function
End Module
