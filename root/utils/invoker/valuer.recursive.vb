
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Reflection
Imports osi.root.constants
Imports osi.root.connector

Partial Public NotInheritable Class valuer
    Public Shared Function create_recursively(Of T, VT)(ByVal obj As T,
                                                        ByVal bindingflags As BindingFlags,
                                                        ByVal name As String,
                                                        ByRef o As valuer(Of VT)) As Boolean
        Using scoped.atomic_bool(suppress.valuer_error)
            Dim type As Type = Nothing
            type = get_type(Of T)(obj)
            While Not type Is GetType(Object)
                o = New valuer(Of VT)(type, bindingflags, obj, name)
                If o.valid() Then
                    Return True
                Else
                    type = type.BaseType()
                End If
            End While
            o = Nothing
            Return False
        End Using
    End Function

    Public Shared Function create_recursively(Of T, VT)(ByVal bindingflags As BindingFlags,
                                                        ByVal name As String,
                                                        ByRef o As valuer(Of VT)) As Boolean
        Return create_recursively(Of T, VT)(Nothing, bindingflags, name, o)
    End Function

    Public Shared Function create_recursively(Of T, VT)(ByVal obj As T,
                                                        ByVal bindingflags As BindingFlags,
                                                        ByVal name As String) As valuer(Of VT)
        Dim o As valuer(Of VT) = Nothing
        assert(create_recursively(obj, bindingflags, name, o))
        Return o
    End Function

    Public Shared Function create_recursively(Of T, VT)(ByVal bindingflags As BindingFlags,
                                                        ByVal name As String) As valuer(Of VT)
        Return create_recursively(Of T, VT)(Nothing, bindingflags, name)
    End Function

    Public Shared Function create_recursively(Of T, VT)(ByVal obj As T,
                                                        ByVal name As String,
                                                        ByRef o As valuer(Of VT)) As Boolean
        Return create_recursively(obj, binding_flags.instance_public, name, o)
    End Function

    Public Shared Function create_recursively(Of T, VT)(ByVal name As String,
                                                        ByRef o As valuer(Of VT)) As Boolean
        Return create_recursively(Of T, VT)(binding_flags.static_public, name, o)
    End Function

    Public Shared Function create_recursively(Of T, VT)(ByVal obj As T, ByVal name As String) As valuer(Of VT)
        Return create_recursively(Of T, VT)(obj, binding_flags.instance_public, name)
    End Function

    Public Shared Function create_recursively(Of T, VT)(ByVal name As String) As valuer(Of VT)
        Return create_recursively(Of T, VT)(binding_flags.static_public, name)
    End Function

    Public Shared Function try_get_recursively(Of T, VT)(ByVal obj As T,
                                                         ByVal bindingflags As BindingFlags,
                                                         ByVal name As String,
                                                         ByRef o As VT) As Boolean
        Dim v As valuer(Of VT) = Nothing
        Return create_recursively(obj, bindingflags, name, v) AndAlso
               assert(Not v Is Nothing) AndAlso
               v.try_get(o)
    End Function

    Public Shared Function get_recursively(Of T, VT)(ByVal obj As T,
                                                     ByVal bindingflags As BindingFlags,
                                                     ByVal name As String) As VT
        Dim o As VT = Nothing
        assert(try_get_recursively(obj, bindingflags, name, o))
        Return o
    End Function

    Public Shared Function try_set_recursively(Of T, VT)(ByVal obj As T,
                                                         ByVal bindingflags As BindingFlags,
                                                         ByVal name As String,
                                                         ByVal i As VT) As Boolean
        Dim v As valuer(Of VT) = Nothing
        Return create_recursively(obj, bindingflags, name, v) AndAlso
               assert(Not v Is Nothing) AndAlso
               v.try_set(i)
    End Function

    Public Shared Sub set_recursively(Of T, VT)(ByVal obj As T,
                                                ByVal bindingflags As BindingFlags,
                                                ByVal name As String,
                                                ByVal i As VT)
        assert(try_set_recursively(obj, bindingflags, name, i))
    End Sub

    Public Shared Function try_get_recursively(Of T, VT)(ByVal obj As T,
                                                         ByVal name As String,
                                                         ByRef o As VT) As Boolean
        Return try_get_recursively(obj, binding_flags.instance_public, name, o)
    End Function

    Public Shared Function get_recursively(Of T, VT)(ByVal obj As T, ByVal name As String) As VT
        Dim o As VT = Nothing
        assert(try_get_recursively(obj, name, o))
        Return o
    End Function

    Public Shared Function try_set_recursively(Of T, VT)(ByVal obj As T,
                                                         ByVal name As String,
                                                         ByVal i As VT) As Boolean
        Return try_set_recursively(obj, binding_flags.instance_public, name, i)
    End Function

    Public Shared Sub set_recursively(Of T, VT)(ByVal obj As T, ByVal name As String, ByVal i As VT)
        assert(try_set_recursively(obj, name, i))
    End Sub

    Public Shared Function try_get_recursively(Of T, VT)(ByVal bindingflags As BindingFlags,
                                                         ByVal name As String,
                                                         ByRef o As VT) As Boolean
        Dim v As valuer(Of VT) = Nothing
        Return create_recursively(Of T, VT)(bindingflags, name, v) AndAlso
               assert(Not v Is Nothing) AndAlso
               v.try_get(o)
    End Function

    Public Shared Function get_recursively(Of T, VT)(ByVal bindingflags As BindingFlags, ByVal name As String) As VT
        Dim o As VT = Nothing
        assert(try_get_recursively(Of T, VT)(bindingflags, name, o))
        Return o
    End Function

    Public Shared Function try_set_recursively(Of T, VT)(ByVal bindingflags As BindingFlags,
                                                         ByVal name As String,
                                                         ByVal i As VT) As Boolean
        Dim v As valuer(Of VT) = Nothing
        Return create_recursively(Of T, VT)(bindingflags, name, v) AndAlso
               assert(Not v Is Nothing) AndAlso
               v.try_set(i)
    End Function

    Public Shared Sub set_recursively(Of T, VT)(ByVal bindingflags As BindingFlags,
                                                ByVal name As String,
                                                ByVal i As VT)
        assert(try_set_recursively(Of T, VT)(bindingflags, name, i))
    End Sub

    Public Shared Function try_set_recursively(Of T, VT)(ByVal name As String, ByVal i As VT) As Boolean
        Return try_set_recursively(Of T, VT)(binding_flags.instance_public, name, i)
    End Function

    Public Shared Sub set_recursively(Of T, VT)(ByVal name As String, ByVal i As VT)
        assert(try_set_recursively(Of T, VT)(name, i))
    End Sub

    Public Shared Function try_get_recursively(Of T, VT)(ByVal name As String, ByRef o As VT) As Boolean
        Return try_get_recursively(Of T, VT)(binding_flags.instance_public, name, o)
    End Function

    Public Shared Function get_recursively(Of T, VT)(ByVal name As String) As VT
        Dim o As VT = Nothing
        assert(try_get_recursively(Of T, VT)(name, o))
        Return o
    End Function
End Class
