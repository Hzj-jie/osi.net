
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.connector
Imports osi.root.utils
Imports osi.service.argument

<AttributeUsage(AttributeTargets.Constructor, AllowMultiple:=False, Inherited:=False)>
Public NotInheritable Class inject_constructor
    Inherits Attribute

    Public Shared Function invoke(Of T)(ByRef o As T, ByVal ParamArray args() As Object) As Boolean
        Return inject_constructor(Of T).invoke(o, args)
    End Function

    Public Shared Function invoke(Of T)(ByRef o As T, ByVal ParamArray args() As String) As Boolean
        Return inject_constructor(Of T).invoke(o, args)
    End Function

    Public Shared Function invoke(Of T)(ByVal v As var, ByRef o As T) As Boolean
        Return inject_constructor(Of T).invoke(v, o)
    End Function
End Class

' For an implementation to use predefined inject constructor.
Public NotInheritable Class inject_constructor(Of T)
    Private Shared ReadOnly info As ConstructorInfo
    Private Shared ReadOnly f As Func(Of Object(), T)

    Shared Sub New()
        info = type_info(Of T).annotated_constructor_info(Of inject_constructor)()
        f = type_info(Of T).annotated_constructor(Of inject_constructor)()
        assert(Not info Is Nothing)
        assert(Not f Is Nothing)
    End Sub

    Public NotInheritable Class derived_proxy(Of IMPL_T As T)
        Public Shared ReadOnly instance As derived_proxy(Of IMPL_T)

        Shared Sub New()
            instance = New derived_proxy(Of IMPL_T)()
        End Sub

        Public Function invoke(ByRef o As T, ByVal ParamArray args() As Object) As Boolean
            Dim x As IMPL_T = Nothing
            If Not inject_constructor.invoke(x, args) Then
                Return False
            End If
            o = x
            Return True
        End Function

        Public Function invoke(ByVal ParamArray args() As Object) As T
            Return inject_constructor(Of IMPL_T).invoke(args)
        End Function

        Public Function invoke(ByRef o As T, ByVal ParamArray args() As String) As Boolean
            Dim x As IMPL_T = Nothing
            If Not inject_constructor.invoke(x, args) Then
                Return False
            End If
            o = x
            Return True
        End Function

        Public Function invoke(ByVal ParamArray args() As String) As T
            Return inject_constructor(Of IMPL_T).invoke(args)
        End Function

        Public Function invoke(ByVal v As var, ByRef o As T) As Boolean
            Dim x As IMPL_T = Nothing
            If Not inject_constructor.invoke(v, x) Then
                Return False
            End If
            o = x
            Return True
        End Function

        Public Function invoke(ByVal i As var) As T
            Return inject_constructor(Of IMPL_T).invoke(i)
        End Function

        Private Sub New()
        End Sub
    End Class

    Public Shared Function of_derived(Of IMPL_T As T)() As derived_proxy(Of IMPL_T)
        Return derived_proxy(Of IMPL_T).instance
    End Function

    Public Shared Function invoke(ByRef o As T, ByVal ParamArray args() As Object) As Boolean
        Try
            o = f(args)
            Return True
        Catch ex As Exception
            log_unhandled_exception(ex)
            Return False
        End Try
    End Function

    Public Shared Function invoke(ByVal ParamArray args() As Object) As T
        Dim o As T = Nothing
        assert(invoke(o, args))
        Return o
    End Function

    Public Shared Function invoke(ByRef o As T, ByVal ParamArray args() As String) As Boolean
        If array_size(args) <> array_size(info.GetParameters()) Then
            Return False
        End If
        Dim objs() As Object = Nothing
        ReDim objs(array_size_i(args) - 1)
        For i As Int32 = 0 To array_size_i(args) - 1
            objs(i) = type_string_serializer.from_str(info.GetParameters()(i).ParameterType(), args(i))
        Next
        Return invoke(o, objs)
    End Function

    Public Shared Function invoke(ByVal ParamArray args() As String) As T
        Dim o As T = Nothing
        assert(invoke(o, args))
        Return o
    End Function

    Public Shared Function invoke(ByVal v As var, ByRef o As T) As Boolean
        If v Is Nothing Then
            Return False
        End If
        Dim objs() As Object = Nothing
        ReDim objs(array_size_i(info.GetParameters()) - 1)
        For i As Int32 = 0 To array_size_i(info.GetParameters()) - 1
            objs(i) = Nothing
            If Not info.GetParameters()(i).RawDefaultValue() Is DBNull.Value Then
                objs(i) = info.GetParameters()(i).RawDefaultValue()
            End If
            Dim s As String = Nothing
            If v.value(info.GetParameters()(i).Name(), s) Then
                objs(i) = type_string_serializer.from_str(info.GetParameters()(i).ParameterType(), s)
            End If
        Next
        Return invoke(o, objs)
    End Function

    Public Shared Function invoke(ByVal i As var) As T
        Dim o As T = Nothing
        assert(invoke(i, o))
        Return o
    End Function

    Private Sub New()
    End Sub
End Class
