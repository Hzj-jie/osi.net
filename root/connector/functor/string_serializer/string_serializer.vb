
Option Explicit On
Option Infer Off
Option Strict On

Imports System.IO
Imports osi.root.delegates

' A functor to implement T to string and string to T operations.
Partial Public Class string_serializer(Of T, PROTECTOR)
    Shared Sub New()
        ' To allow T to register its own serializer in the static constructor.
        static_constructor(Of T).execute()
    End Sub

    Private Shared Function is_string() As Boolean
        Return type_info(Of T, type_info_operators.equal, String).v
    End Function

    Private NotInheritable Class object_register
        Shared Sub New()
            If string_serializer.protector(Of PROTECTOR).is_global Then
                type_resolver(Of string_serializer(Of Object), string_serializer).assert_first_register(
                    GetType(T),
                    string_serializer_object(Of T).of(string_serializer(Of T).r))
            ElseIf string_serializer.protector(Of PROTECTOR).is_json Then
                type_resolver(Of string_serializer(Of Object), json_serializer).assert_first_register(
                    GetType(T),
                    string_serializer_object(Of T).of(json_serializer(Of T).r))
            ElseIf string_serializer.protector(Of PROTECTOR).is_uri Then
                type_resolver(Of string_serializer(Of Object), uri_serializer).assert_first_register(
                    GetType(T),
                    string_serializer_object(Of T).of(uri_serializer(Of T).r))
            End If
        End Sub

        Public Shared Sub init()
        End Sub

        Private Sub New()
        End Sub
    End Class

    Public Shared Sub register(ByVal to_str As Func(Of T, StringWriter, Boolean))
        global_resolver(Of Func(Of T, StringWriter, Boolean), PROTECTOR).assert_first_register(to_str)
        object_register.init()
    End Sub

    Public Shared Sub register(ByVal from_str As _do_val_ref(Of StringReader, T, Boolean))
        global_resolver(Of _do_val_ref(Of StringReader, T, Boolean), PROTECTOR).assert_first_register(from_str)
        object_register.init()
    End Sub

    Protected Overridable Function to_str() As Func(Of T, StringWriter, Boolean)
        Return global_resolver(Of Func(Of T, StringWriter, Boolean), PROTECTOR).resolve_or_null()
    End Function

    ' Write i into StringWriter
    Public Function to_str(ByVal i As T, ByVal o As StringWriter) As Boolean
        If o Is Nothing Then
            Return False
        End If

        Dim f As Func(Of T, StringWriter, Boolean) = Nothing
        f = to_str()
        If Not f Is Nothing Then
            Return f(i, o)
        End If

        Dim f2 As Func(Of T, StringWriter, Boolean) = Nothing
        f2 = uri_serializer(Of T).r.to_str()
        assert(object_compare(f, f2) <> 0)
        assert(Not f2 Is Nothing)
        f2(i, o)
        Return True
    End Function

    Protected Overridable Function from_str() As _do_val_ref(Of StringReader, T, Boolean)
        Return global_resolver(Of _do_val_ref(Of StringReader, T, Boolean), PROTECTOR).resolve_or_null()
    End Function

    Private Function do_from_str(ByVal i As StringReader, ByRef o As T) As Boolean
        assert(Not i Is Nothing)

        Dim f As _do_val_ref(Of StringReader, T, Boolean) = Nothing
        f = from_str()
        If Not f Is Nothing Then
            Return f(i, o)
        End If

        Dim f2 As _do_val_ref(Of StringReader, T, Boolean) = Nothing
        f2 = uri_serializer(Of T).r.from_str()
        assert(object_compare(f, f2) <> 0)
        assert(Not f2 Is Nothing)
        Return f2(i, o)
    End Function

    ' Read enough characters from i and construct o.
    Public Function from_str(ByVal i As StringReader, ByRef o As T) As Boolean
        If i Is Nothing Then
            Return False
        End If

        Dim p As UInt32 = 0
        p = i.position()
        If do_from_str(i, o) Then
            Return True
        End If
        i.position(p)
        Return False
    End Function

    Protected Sub New()
    End Sub
End Class

Partial Public NotInheritable Class string_serializer
    Public NotInheritable Class protector(Of T)
        Public Shared ReadOnly is_global As Boolean = GetType(T).generic_type_is(GetType(string_serializer(Of )))
        Public Shared ReadOnly is_json As Boolean = GetType(T).generic_type_is(GetType(json_serializer(Of )))
        Public Shared ReadOnly is_uri As Boolean = GetType(T).generic_type_is(GetType(uri_serializer(Of )))

        Private Sub New()
        End Sub
    End Class

    Private Sub New()
    End Sub
End Class

' Raw string operations without type information or conversions. I.e. Int 100 from or to String "100".
' This serializer works for simple types only, complex structures should support bytes_serializer or json_serializer.
Public Class string_serializer(Of T)
    Inherits string_serializer(Of T, string_serializer(Of T))

    Public Shared ReadOnly r As string_serializer(Of T) = New string_serializer(Of T)()

    Protected Sub New()
    End Sub
End Class
